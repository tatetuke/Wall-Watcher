Shader "Custom/OutLineShader"
{
    Properties
    {
        _MainTex("main texture", 2D) = "white" {}
        _MainColor("Color", Color) = (1,1,1,1)
        _LineWidth("LineWidth", Range(0.0, 1.0)) = 0.0
        [HDR] _EmissionColor("Emission Color", Color) = (0,0,0)    //追加
    }
    SubShader
    {
        Tags { "RenderType" = "Transparent" "Queue" = "Transparent" }
         Blend SrcAlpha OneMinusSrcAlpha
        Pass
        {
            CGPROGRAM
            #pragma vertex vert_img
            #pragma fragment frag

            #include "UnityCG.cginc"

            uniform sampler2D _MainTex;
            float _LineWidth;
            float4 _MainColor;
            float4 _EmissionColor;

            float outline(sampler2D tex, float2  uv, float2  offset, float width) {
                float2 uv2 = uv + offset * width;
                return tex2D(tex, uv2).a;
            }

            fixed4 frag(v2f_img i) : SV_Target
            {
                float  lin = outline(_MainTex,i.uv,float2(1,0),_LineWidth)
            +outline(_MainTex, i.uv, float2(-1, 0), _LineWidth);
               // + outline(_MainTex,i.uv,float2(0,1),_LineWidth)
               // + outline(_MainTex,i.uv,float2(0,-1),_LineWidth);
                lin = clamp(lin, 0, 1);
                fixed4 col = tex2D(_MainTex, i.uv) * _MainColor;
                return lin * _EmissionColor;
            }


            ENDCG
        }

    }
}