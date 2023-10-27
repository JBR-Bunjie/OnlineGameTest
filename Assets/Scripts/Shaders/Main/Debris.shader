Shader "Main/Debris" {
    // https://blog.csdn.net/qq_39738178/article/details/125806754
    Properties {
        _MainColor ("Main Color", Color) = (1, 1, 1, 1)
        _Thickness ("Line Thickness", float) = 0.1
        _RotatingSpeed ("Rotating Speed", float) = 1
        _R ("_R", float) = 0.5
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
            // WHY NOT RENDER FEATURE?!
            Tags {
                "LightMode" = "UniversalForward"
            }
        
//            Blend SrcAlpha OneMinusSrcAlpha
//            AlphaTest 

            Cull Off
//            ZTest LEqual
//            ZWrite On
        
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