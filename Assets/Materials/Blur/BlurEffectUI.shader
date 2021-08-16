Shader "BlurEffect_UI"
{
    Properties
    {
        _Texture("Base (RGB)", 2D) = "" {}
        _BlurSize("Blur size", Float) = 0
    }
    Subshader
    {
        Pass
        {
            ZTest Always Cull Off ZWrite Off
            Fog { Mode off }
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata_t
            {
                float4 vertex   : POSITION;
                float4 color    : COLOR;
                float2 texcoord : TEXCOORD0;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct v2f
            {
                float4 vertex   : SV_POSITION;
                fixed4 color : COLOR;
                float2 texcoord  : TEXCOORD0;
                float4 worldPosition : TEXCOORD1;
                float4 pos : TEXCOORD2;
                UNITY_VERTEX_OUTPUT_STEREO
            };


            sampler2D _Texture;
            float4 _Texture_ST;
            uniform half4 _MainTex_TexelSize;
            uniform half _BlurSize;
            sampler2D _GrabPassTexture;

            v2f vert(appdata_t v)
            {
                v2f OUT;
                UNITY_SETUP_INSTANCE_ID(v);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(OUT);
                OUT.worldPosition = v.vertex;
                OUT.vertex = UnityObjectToClipPos(OUT.worldPosition);

                OUT.texcoord = TRANSFORM_TEX(v.texcoord, _Texture);

                OUT.pos = ComputeScreenPos(OUT.vertex);

                OUT.color = v.color;
                return OUT;
            }


            half4 frag(v2f i) : SV_Target
            {
                float2 uv = i.pos.xy / i.pos.w;
                // uv.y = 1.0 - uv.y;

                half blur = _BlurSize;
                blur = max(1, blur);

                fixed4 col = (0, 0, 0, 0);
                float weight_total = 0;

                [loop]
                for (float x = -blur; x < blur; x += 1)
                {
                    float distance_normalized = abs(x / blur);
                    float weight = exp(-0.5 * pow(distance_normalized, 2) * 5.0);
                    weight_total += weight;
                    col += tex2D(_Texture, uv+ half2(x*0.01, 0)) * weight;
                }
                [loop]
                for (float y = -blur; y < blur; y += 1)
                {
                    float distance_normalized = abs(y / blur);
                    float weight = exp(-0.5 * pow(distance_normalized, 2) * 5.0);
                    weight_total += weight;
                    col += tex2D(_Texture, uv + half2(0, y*0.01)) * weight;
                }

                col /= weight_total;

#ifdef UNITY_UI_CLIP_RECT
                col.a *= UnityGet2DClipping(i.worldPosition.xy, _ClipRect);
#endif

#ifdef UNITY_UI_ALPHACLIP
                clip(col.a - 0.001);
#endif

                return col;
            }

            ENDCG
        }

    }

        Fallback Off
}