Shader "TECH_VALIDATION/ChangingShape" {
    Properties {
        _MainTex ("Main Tex", 2D) = "white" { }
        _Color ("Color Tint", Color) = (1, 1, 1, 1)
    }
    SubShader {
        Tags {
            "RenderPipeline"="UniversalRenderPipeline"
            "RenderType"="Oqaue" 
            "Queue" = "Geometry"
        }

        HLSLINCLUDE
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
        
        struct a2v {
            float4 vertex: POSITION;
            float2 uv: TEXCOORD0;
        };

        struct v2f {
            float4 pos: SV_POSITION;
            float2 uv: TEXCOORD0;
        };
        
        TEXTURE2D(_MainTex);
        SAMPLER(sampler_MainTex);
        CBUFFER_START(UnityPerMaterial)
        float4 _MainTex_ST;
        half4 _Color;
        CBUFFER_END
        ENDHLSL

        Pass {
            Name "DepthNormals"
            Tags { "LightMode"="DepthNormals" }
            ZWrite On
            HLSLPROGRAM

            #pragma vertex vert
            #pragma fragment frag


            v2f vert(a2v v) {
                v2f o;
                VertexPositionInputs vertexPos = GetVertexPositionInputs(v.vertex.xyz);
                o.pos = vertexPos.positionCS;
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            half4 frag(v2f i): SV_Target {
                half4 col = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.uv) * _Color;
                return col;
            }
            
            ENDHLSL
        }
        
        Pass {
            Name "ForwardLit"
            Cull Off
//            Blend SrcAlpha OneMinusSrcAlpha
            
            Tags { "LightMode"="UniversalForward" }
            
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            // #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
            
            v2f vert(a2v v) {
                v2f o;
                VertexPositionInputs vertexPos = GetVertexPositionInputs(v.vertex.xyz);
                o.pos = vertexPos.positionCS;
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            half4 frag(v2f i): SV_Target {
                half4 col = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.uv) * _Color;
                return col;
            }
            ENDHLSL
        }
    }
}