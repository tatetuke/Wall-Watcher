Shader "GaussianBlur"
{
    Properties
    {
        _MainTex("Texture", 2D) = "white" {}
        _SamplingDistance("Sampling Distance", float) = 1.0
        _SamplingCount("Sampling Count", int) = 1
        _WeightMul("Weight",float)=1.0
    }
        SubShader
        {
            Tags { "RenderType" = "Opaque" }

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
                    half2 coordV : TEXCOORD0;
                    half2 coordH : TEXCOORD1;
                    float4 vertex : SV_POSITION;
                    half2 offsetV: TEXCOORD2;
                    half2 offsetH: TEXCOORD3;
                };

                sampler2D _MainTex;
                float4 _MainTex_ST;
                float _WeightMul;
                half4 _MainTex_TexelSize;
                float _SamplingDistance;
                int _SamplingCount;

                v2f vert(appdata v)
                {
                    v2f o;
                    o.vertex = UnityObjectToClipPos(v.vertex);
                    half2 uv = TRANSFORM_TEX(v.uv, _MainTex);

                    // サンプリングポイントのオフセット
                    o.offsetV = _MainTex_TexelSize.xy * half2(0.0, 1.0) * _SamplingDistance;
                    o.offsetH = _MainTex_TexelSize.xy * half2(1.0, 0.0) * _SamplingDistance;

                    // サンプリング開始ポイントのUV座標
                    o.coordV = uv - o.offsetV * ((_SamplingCount - 1) * 0.5);
                    o.coordH = uv - o.offsetH * ((_SamplingCount - 1) * 0.5);

                    return o;
                }

                fixed4 frag(v2f i) : SV_Target
                {
                   half4 col = 0;

                    // 垂直方向
                    for (int j = 0; j < _SamplingCount; j++) {
                        float distance_normalized = abs(j- _SamplingCount/2 ) / (float)_SamplingCount;
                        float weight = exp(-0.5 * distance_normalized* distance_normalized);
                        // サンプリングして重みを掛ける。後で水平方向も合成するため0.5をかける
                        col += tex2D(_MainTex, i.coordV) * weight / _WeightMul;
                        // offset分だけサンプリングポイントをずらす
                        i.coordV += i.offsetV;
                    }

                    // 水平方向
                    for (int j = 0; j < _SamplingCount; j++) {
                        float distance_normalized = abs(j - _SamplingCount / 2) / (float)_SamplingCount;
                        float weight = exp(-0.5 * distance_normalized * distance_normalized);
                        col += tex2D(_MainTex, i.coordH) * weight / _WeightMul;
                        i.coordH += i.offsetH;
                    }

                    return col;
                }
                ENDCG
            }
        }
}