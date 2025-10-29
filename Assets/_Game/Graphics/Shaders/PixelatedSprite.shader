Shader "Tools/PixelateSprite"
{
    Properties
    {
        _MainTex ("Sprite Texture", 2D) = "white" {}
        _PixelSize ("Pixel Size", Range(32, 2048)) = 2048
        _Color ("Tint", Color) = (1,1,1,1)
    }

    SubShader
    {
        Tags {
            "Queue"="Transparent"
            "RenderType"="Transparent"
            "IgnoreProjector"="True"
            "CanUseSpriteAtlas"="True"
        }

        Blend SrcAlpha OneMinusSrcAlpha
        Cull Off
        ZWrite Off

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float _PixelSize;
            fixed4 _Color;
            fixed4 _RendererColor; // couleur du SpriteRenderer (injectée par Unity)

            struct appdata_t
            {
                float4 vertex : POSITION;
                float2 texcoord : TEXCOORD0;
                fixed4 color : COLOR;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                fixed4 color : COLOR;
                float4 vertex : SV_POSITION;
            };

            v2f vert (appdata_t IN)
            {
                v2f OUT;
                OUT.vertex = UnityObjectToClipPos(IN.vertex);
                OUT.uv = TRANSFORM_TEX(IN.texcoord, _MainTex);

                // Combine couleur du SpriteRenderer + vertex + teinte du mat
                OUT.color = IN.color * _Color * _RendererColor;

                return OUT;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // Pixelation
                float2 pixelUV = (floor(i.uv * _PixelSize) + 0.5) / _PixelSize;
                fixed4 c = tex2D(_MainTex, pixelUV);

                // Applique la teinte/alpha
                return c * i.color;
            }
            ENDCG
        }
    }
}
