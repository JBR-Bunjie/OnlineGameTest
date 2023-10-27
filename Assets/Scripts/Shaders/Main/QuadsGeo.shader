Shader "Main/QuadsGeo" {
    Properties {
        _MainColor ("Main Color", Color) = (1, 1, 1, 1)
    }
    SubShader {
        Tags {
            "RenderType" = "Opaque" "RenderPipeline" = "UniversalPipeline" "Queue" = "Geometry"
        }

        HLSLINCLUDE
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"

        CBUFFER_START(UnityPerMaterial)
        half4 _MainColor;
        CBUFFER_END
        ENDHLSL

        Pass {
            Name "ForwardLit"
            Tags {
                "LightMode" = "UniversalForward"
            }

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            struct a2v {
                float4 vertex: POSITION;
                float2 uv: TEXCOORD0;
            };

            struct v2f {
                float4 pos: SV_POSITION;
            };

            v2f vert(a2v v) {
                v2f o;
                VertexPositionInputs vertexPos = GetVertexPositionInputs(v.vertex.xyz);
                o.pos = vertexPos.positionCS;
                return o;
            }

            half4 frag(v2f i): SV_Target {
                return _MainColor;
            }
            ENDHLSL
        }
    }
}