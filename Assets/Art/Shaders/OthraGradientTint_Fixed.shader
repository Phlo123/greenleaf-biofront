
Shader "Shader Graphs/OthraGradientTint"
{
    Properties
    {
        _BaseColor("Base Color", 2D) = "white" {}
        _TopTintColor("Top Tint Color", Color) = (1, 1, 1, 1)
        _BottomTintColor("Bottom Tint Color", Color) = (0, 0.5, 0, 1)
        _GradientHeightMin("Gradient Height Min", Float) = 0
        _GradientHeightMax("Gradient Height Max", Float) = 2
        [Normal] _NormalMap("Normal Map", 2D) = "bump" {}
    }

    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        Pass
        {
            Name "ForwardLit"
            Tags { "LightMode"="UniversalForward" }

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION;
                float3 normalOS : NORMAL;
                float2 uv : TEXCOORD0;
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
                float2 uv : TEXCOORD0;
                float3 worldPos : TEXCOORD1;
            };

            sampler2D _BaseColor;
            sampler2D _NormalMap;
            float4 _BaseColor_ST;
            float4 _TopTintColor;
            float4 _BottomTintColor;
            float _GradientHeightMin;
            float _GradientHeightMax;

            Varyings vert(Attributes IN)
            {
                Varyings OUT;
                float3 worldPos = TransformObjectToWorld(IN.positionOS.xyz);
                OUT.worldPos = worldPos;
                OUT.uv = TRANSFORM_TEX(IN.uv, _BaseColor);
                OUT.positionHCS = TransformWorldToHClip(worldPos);
                return OUT;
            }

            half4 frag(Varyings IN) : SV_Target
            {
                float3 baseColor = tex2D(_BaseColor, IN.uv).rgb;

                // Gradient logic
                float height = IN.worldPos.y;
                float gradientT = saturate((height - _GradientHeightMin) / (_GradientHeightMax - _GradientHeightMin));
                float3 tint = lerp(_BottomTintColor.rgb, _TopTintColor.rgb, gradientT);

                return half4(baseColor * tint, 1.0);
            }
            ENDHLSL
        }
    }
    FallBack "Diffuse"
}
