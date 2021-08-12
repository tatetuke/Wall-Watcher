Shader "BlurEffect_MainTex"
{
    Properties
    {
        _MainTex("Base (RGB)", 2D) = "" {}
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

            struct appdata {
                half4 pos : POSITION;
                half2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 pos  : SV_POSITION;
                half2  uv  : TEXCOORD0;
            };

            sampler2D _MainTex;
            uniform half4 _MainTex_TexelSize;
            uniform half _BlurSize;

                    v2f vert(appdata v)
                    {
                        v2f o;
                        o.pos = UnityObjectToClipPos(v.pos);
                        o.uv = v.uv;
                        return o;
                    }


                    half4 frag(v2f i) : SV_Target
                    {
                        half blur = _BlurSize;
                        blur = max(1, blur);

                        fixed4 col = (0, 0, 0, 0);
                        float weight_total = 0;

                        [loop]
                        for (float x = -blur; x <= blur; x += 1)
                        {
                            float distance_normalized = abs(x / blur);
                            float weight = exp(-0.5 * pow(distance_normalized, 2) * 5.0);
                            weight_total += weight;
                            col += tex2D(_MainTex, i.uv+ half2(x*0.01, 0)) * weight;
                        }
                        [loop]
                        for (float y = -blur; y <= blur; y += 1)
                        {
                            float distance_normalized = abs(y / blur);
                            float weight = exp(-0.5 * pow(distance_normalized, 2) * 5.0);
                            weight_total += weight;
                            col += tex2D(_MainTex, i.uv + half2(0, y*0.01)) * weight;
                        }

                        col /= weight_total;
                        return col;
                    }

                    ENDCG
                }
    }

        Fallback Off
}