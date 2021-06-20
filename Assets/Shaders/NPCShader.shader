﻿Shader "Custom/NPCShader" {
	Properties{
		_Color("Main Color", Color) = (1,1,1,0.5)
		_MainTex("Texture", 2D) = "white" { }
	_Width("Tex Width", Float) = 200.0
		_Height("Tex Height", Float) = 200.0
		_Thick("Line Thickness", Int) = 2
		_LineColor("Line Color",Color) = (0,0,0,1) // ※※
	}


		SubShader{
		Cull Off

		Tags{
		"Queue" = "Transparent"
		"IgnoreProjector" = "True"
		"RenderType" = "Transparent"
		"PreviewType" = "Plane"
		"CanUseSpriteAtlas" = "False"
	}
		Pass{


		Blend SrcAlpha OneMinusSrcAlpha
		CGPROGRAM

#pragma vertex vert
#pragma fragment frag
#pragma target 3.0

		fixed4 _Color;
	sampler2D _MainTex;
	float _Width;
	float _Height;
	int _Thick;
	fixed4 _LineColor; // ※※

	struct vin {
		float4 vertex: POSITION;
		float4 texcoord : TEXCOORD0;
	};
	struct v2f {
		float2 uv : TEXCOORD0;
		float4 pos : SV_POSITION;
	};

	v2f vert(vin v)
	{
		v2f o;
		o.pos = UnityObjectToClipPos(v.vertex);
		o.uv = v.texcoord;
		return o;
	}

	fixed4 frag(v2f i) : SV_Target
	{
		float4 texcol = tex2D(_MainTex, i.uv);
		float rangeSq = _Thick * _Thick; // ※※

		// このピクセルの周囲の透明度の最大値を調べる
		float alphaMax = 0.0f;
		for (int x = -_Thick; x <= _Thick; ++x)
			for (int y = -_Thick; y <= _Thick; ++y)
			{
				float alpha = tex2D(_MainTex, i.uv + float2(x / _Width, y / _Height)).a;
				if (alpha > 0.5 && x * x + y * y <= rangeSq) // ※※
					alphaMax = 1; // ※※
			}

		// このピクセルが透明なら、周囲の透明度の最大値で塗る。
		if (texcol.a < 0.5)
			return float4(_LineColor.xyz, alphaMax); // ※※
		else
			return texcol;
	}
		ENDCG
	}
	}
}