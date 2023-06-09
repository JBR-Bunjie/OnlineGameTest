Shader "TECH_VALIDATION/VolumeCloud" {
    Properties {
        _Color ("Colot Tint", Color) = (1, 1, 1, 1)
        _MainTex ("Main Texture", 2D) = "white" {}
        _Layers ("Cloud Level", Range(8, 128)) = 16
        _SpeedX ("Speed X", float) = 0.05
        _SpeedY ("Speed Y", float) = 0.05
        _Alpha ("Cloud Alpha Value", Range(0, 1)) = 0.6
        _HeightOffset ("Height Offset", float) = 5
    }
    SubShader {
        Tags {
            "RenderPipeline"="UniversalRenderPipeline"
            "RenderType"="Transparent"
            "Queue"="Transparent-50"
        }

        HLSLINCLUDE
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

        TEXTURE2D(_MainTex);
        SAMPLER(sampler_MainTex);

        CBUFFER_START(UnityPerMaterial)
        half4 _Color;
        float4 _MainTex_ST;
        float _Layers;
        float _SpeedX;
        float _SpeedY;
        half _Alpha;
        half _HeightOffset;
        CBUFFER_END
        ENDHLSL


        Pass {
//            ZWrite Off 
            Cull Off
            Blend SrcAlpha OneMinusSrcAlpha
            
            Tags { "LightMode"="UniversalForward" }
            
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"

            struct appdata {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float3 normal : NORMAL;
                float4 tangent : TANGENT;
            };

            struct v2f {
                float4 pos : SV_POSITION;
                float4 uv : TEXCOORD0;
                float3 worldPos : TEXCOORD1;
                float3 viewDir : TEXCOORD2;
                float3 worldNormal : TEXCOORD3;
            };

            v2f vert(appdata v) {
                v2f o;
                VertexPositionInputs ver = GetVertexPositionInputs(v.vertex.xyz);
                VertexNormalInputs nor = GetVertexNormalInputs(v.normal, v.tangent);

                o.pos = ver.positionCS;
                o.worldPos = ver.positionWS;
                o.worldNormal = nor.normalWS;
                
                float3 binormal = cross( normalize(v.normal), normalize(v.tangent.xyz) ) * v.tangent.w;
                // half3x3 will construct a matrix in row-major order
                half3x3 tbnR = half3x3(v.tangent.xyz, binormal, v.normal);
                o.viewDir = mul(tbnR, GetObjectSpaceNormalizeViewDir(v.vertex.xyz));
                // half3x3 tbn = half3x3(nor.tangentWS, nor.bitangentWS, nor.normalWS);
                // o.viewDir = TransformWorldToTangent(GetWorldSpaceViewDir(o.worldPos), tbn);
                // o.viewDir = TransformObjectToTangent(GetObjectSpaceNormalizeViewDir(v.vertex.xyz), tbn);

                o.uv.xy = TRANSFORM_TEX(v.uv, _MainTex) + float2(frac(_Time.y * _SpeedX), frac(_Time.y * _SpeedY));
                o.uv.zw = v.uv;
                
                return o;
            }

            half4 frag(v2f i) : SV_Target {
                float3 viewDir = normalize(i.viewDir);
                viewDir.xy *= _HeightOffset;

                viewDir.z += 0.4;

                float3 uv = float3(i.uv.xy, 0);// + frac(speed), 0);
                float3 uv2 = float3(i.uv.zw, 0);

                float4 MainTex = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, uv2.xy);

                // Step-Length
                float3 minOffset = viewDir / (viewDir.z * _Layers);
                
                float finiNoise = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, uv.xy).r * MainTex.r;
                float3 prev_uv = uv;

                while (finiNoise > uv.z) {
                    uv += minOffset;
                    finiNoise = SAMPLE_TEXTURE2D_LOD(_MainTex, sampler_MainTex, uv.xy, 0).r * MainTex.r;
                    // finiNoise = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, uv.xy).r * MainTex.r;
                }

                float d1 = finiNoise - uv.z;
                float d2 = finiNoise - prev_uv.z;
                float w = d1 / (d1 - d2 + 0.0000001);
                uv = lerp(uv, prev_uv, w);
                half4 resultColor = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, uv.xy) * MainTex;

                half rangeClt = MainTex.a * resultColor.r + _Alpha * 0.75;
                half Alpha = abs(smoothstep(rangeClt, _Alpha, 1.0));
                Alpha = pow(Alpha, 5);

                // Light light = GetMainLight();

                return half4(resultColor.rgb * _Color.rgb, Alpha); //* light.color.rgb
            }
            ENDHLSL
        }
    }
}