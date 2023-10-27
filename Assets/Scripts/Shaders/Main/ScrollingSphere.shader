Shader "Main/ScrollingSphere" {
    // https://blog.csdn.net/qq_39738178/article/details/125806754
    Properties {
        _MainColor ("Main Color", Color) = (1, 1, 1, 1)
        _Thickness ("Line Thickness", float) = 0.1
        _RotatingSpeed ("Rotating Speed", float) = 1
        _R ("_R", float) = 0.1
    }
    SubShader {
        Tags {
            "RenderType" = "Opaque" "RenderPipeline" = "UniversalPipeline" "Queue" = "Transparent"
        }

        HLSLINCLUDE
        #pragma target 4.0
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"

        CBUFFER_START(UnityPerMaterial)
            half4 _MainColor;
            float _Thickness;
            float _RotatingSpeed;
            float _R;
        CBUFFER_END
        ENDHLSL

        Pass {
            Name "Main"
            Tags {
                "LightMode" = "UniversalForward"
            }

            //            Blend SrcAlpha OneMinusSrcAlpha
            //            Cull Front
            Blend One Zero, One Zero
            //            Cull Back
            Cull Off
            ZTest LEqual
            ZWrite On

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma geometry geo

            struct a2v {
                float4 vertex: POSITION;
            };

            struct v2g {
                float4 pos: SV_POSITION;
                float4 positionCS : TEXCOORD0;
            };

            struct g2f {
                float4 pos : SV_POSITION;
                float3 barycentric : TEXCOORD0;
            };

            v2g vert(a2v v) {
                v2g o;
                float sinT = sin(_Time.x * 8);
                float cosT = cos(_Time.x * 8);
                // xRotate
                float3 dynamicPos = float3(v.vertex.x, cosT * v.vertex.y - sinT * v.vertex.z,
                                           sinT * v.vertex.y + cosT * v.vertex.z);
                // yRotate
                dynamicPos = float3(dynamicPos.x * cosT + dynamicPos.z * sinT, dynamicPos.y,
                                    -sinT * dynamicPos.x + cosT * dynamicPos.z);
                // Apply
                VertexPositionInputs vertexPos = GetVertexPositionInputs(dynamicPos);
                o.pos = vertexPos.positionCS;
                o.positionCS = vertexPos.positionCS;
                return o;
            }

            [maxvertexcount(3)]
            void geo(triangle v2g p[3], inout TriangleStream<g2f> g2fStream) {
                //计算点在屏幕空间中的位置:首先positionCS中的值还没有进行齐次除法，需要先除以positionCS.w转换为
                //NDC坐标，范围[-1,1]，然后*0.5+0.5后转为[0,1],最后乘以_ScreenParams转换为最终的屏幕空间位置
                float2 p0 = _ScreenParams.xy * (p[0].positionCS.xy / p[0].positionCS.w * 0.5 + 0.5);
                float2 p1 = _ScreenParams.xy * (p[1].positionCS.xy / p[1].positionCS.w * 0.5 + 0.5);
                float2 p2 = _ScreenParams.xy * (p[2].positionCS.xy / p[2].positionCS.w * 0.5 + 0.5);

                //edge vectors
                float2 v0 = p2 - p1;
                float2 v1 = p2 - p0;
                float2 v2 = p1 - p0;

                //area of the triangle
                float area = abs(v1.x * v2.y - v1.y * v2.x);

                //values based on distance to the edges
                float dist0 = area / length(v0);
                float dist1 = area / length(v1);
                float dist2 = area / length(v2);

                g2f pIn;

                //add the first point
                pIn.pos = p[0].positionCS;
                pIn.barycentric = float3(dist0, 0, 0);
                g2fStream.Append(pIn);

                //add the second point
                pIn.pos = p[1].positionCS;
                pIn.barycentric = float3(0, dist1, 0);
                g2fStream.Append(pIn);

                //add the third point
                pIn.pos = p[2].positionCS;
                pIn.barycentric = float3(0, 0, dist2);
                g2fStream.Append(pIn);
            }

            half4 frag(g2f i): SV_Target {
                float3 ndcPos = i.pos / 2 + 0.5;
                half4 col = _MainColor * (1 - ndcPos.z);

                float val = min(i.barycentric.x, min(i.barycentric.y, i.barycentric.z));

                //calculate power to 2 to thin the line
                val = exp2(-1 / _Thickness * val * val);
                // val = exp2( -1/1 * val * val );

                //丢弃不在边线上的
                if (val < 0.5f) discard;

                return col;
            }
            ENDHLSL
        }

        Pass {
            // WHY NOT RENDER FEATURE?!
            Tags {
                "LightMode" = "SRPDefaultPass"
            }

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            struct a2v {
                float4 vertex: POSITION;
                float2 uv : TEXCOORD2;
            };

            struct v2f {
                float4 pos : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            v2f vert(a2v v) {
                v2f o;
                float sinT = sin(_Time.x * 8 * _RotatingSpeed);
                float cosT = cos(_Time.x * 8 * _RotatingSpeed);
                // xRotate
                float3 dynamicPos = float3(v.vertex.x, cosT * v.vertex.y - sinT * v.vertex.z,
                   sinT * v.vertex.y + cosT * v.vertex.z);
                // yRotate
                dynamicPos = float3(dynamicPos.x * cosT + dynamicPos.z * sinT, dynamicPos.y,
    -sinT * dynamicPos.x + cosT * dynamicPos.z);
                // Apply
                VertexPositionInputs vertexPos = GetVertexPositionInputs(dynamicPos);
                // VertexPositionInputs vertexPos = GetVertexPositionInputs(v.vertex.xyz);
                o.pos = vertexPos.positionCS;

                o.uv = v.uv;
                return o;
            }

            half RR(float2 uv, half2 offset) {
                uv += offset;
                half sphere = distance(uv, 0.5);
                sphere = step(sphere, _R);
                return sphere;
            }

            half4 frag(v2f i): SV_Target {
                half mask = RR(i.uv, float2(0.5, 0.5))
                    + RR(i.uv, float2(-0.5, 0.5))
                    + RR(i.uv, float2(0.5, -0.5))
                    + RR(i.uv, float2(-0.5, -0.5));


                clip(mask - 0.001);

                float3 ndcPos = i.pos / 2 + 0.5;
                half4 col = _MainColor * (1 - ndcPos.z);

                return col;
            }
            ENDHLSL
        }
    }
}