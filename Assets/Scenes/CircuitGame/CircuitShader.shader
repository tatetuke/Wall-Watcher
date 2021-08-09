Shader "Custom/BaseShader"
{
    Properties
    {
        _MainTex("main texture", 2D) = "white" {}
        _MainColor("Color", Color) = (1,1,1,1)
        _Thresiold("Thresiold", Range(0.0, 1.0)) = 0.0
        _EmissionTex("emission texture", 2D) = "white" {}
        [HDR] _EmissionColor("Emission Color", Color) = (0,0,0)    //追加
    }
        SubShader
        {
            Pass
            {
                CGPROGRAM
                #pragma vertex vert_img
                #pragma fragment frag

                #include "UnityCG.cginc"

                uniform sampler2D _MainTex;
        uniform sampler2D _EmissionTex;
        float _Thresiold;
        float4 _MainColor;
        float4 _EmissionColor;

        fixed4 frag(v2f_img i) : SV_Target
        {
            fixed4 emi_col = tex2D(_EmissionTex, i.uv);
        fixed a = step(emi_col.r, _Thresiold)* emi_col.a;
                    return tex2D(_MainTex, i.uv) * _MainColor*(1- a)+  a*_EmissionColor;
                }
                ENDCG
            }
        }
}