Shader "Custom/UIGradientWithRadius"
{
    Properties
    {
        _Color1 ("Top Color", Color) = (1,0,0,1)
        _Color2 ("Bottom Color", Color) = (0,0,1,1)
        _RadiusColor ("Right Edge Color", Color) = (0,1,0,1)
        _Radius ("Radius", Float) = 0.5
    }
    SubShader
    {
        Tags { "Queue" = "Overlay" "IgnoreProjector" = "True" "RenderType" = "Transparent" }
        LOD 100
        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha
        Stencil {
            Ref 1
            Comp Always
            Pass Replace
        }

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

            fixed4 _Color1;
            fixed4 _Color2;
            fixed4 _RadiusColor;
            float _Radius;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // Gradient dọc (Top to Bottom)
                float verticalGradient = i.uv.y;

                // Gradient bán kính (bên phải)
                float horizontalGradient = saturate((i.uv.x - (1.0 - _Radius)) / _Radius);

                // Kết hợp gradient
                fixed4 verticalColor = lerp(_Color2, _Color1, verticalGradient);
                fixed4 finalColor = lerp(verticalColor, _RadiusColor, horizontalGradient);

                return finalColor;
            }
            ENDCG
        }
    }
}
