Shader "Custom/ButtonGradientShader"
{
    Properties
    {
        _MainTex("Dummy Texture", 2D) = "white" {}
        _TopColor("Top Color", Color) = (0.9,0.9,0.9,1) // Slightly darker than white
        _BottomColor("Bottom Color", Color) = (0.8,0.8,0.8,1) // Dark gray
        _Bias("Bias", Range(0,1)) = 0.5 // Default to 0.5 (no bias)
        _Smoothness("Smoothness", Range(0,1)) = 0.5 // Controls the curvature
    }

    SubShader
    {
        Tags { "Queue" = "Transparent" "RenderType" = "Transparent" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _TopColor;
            float4 _BottomColor;
            float _Bias;
            float _Smoothness;

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            half4 frag(v2f i) : SV_Target
            {
                float grad = smoothstep(_Bias - _Smoothness, _Bias + _Smoothness, i.uv.y);
                half4 col = lerp(_BottomColor, _TopColor, grad);
                return col;
            }
            ENDCG
        }
    }
}
