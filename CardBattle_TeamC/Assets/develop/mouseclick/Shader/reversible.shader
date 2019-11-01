Shader "Unlit/reversible"
{
	Properties
	{
		_MainTex("表面テクスチャ", 2D) = "white" {}
		_MainTex2("裏面テクスチャ", 2D) = "white"{}

		_Angle("角度", float)=11.0
	}
		SubShader
	{
		Tags { "RenderType" = "Opaque" }
		LOD 100
		Cull off

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 3.0

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
			sampler2D _MainTex2;
			

			float _Angle;

			v2f vert(appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				// Pivot
				float2 pivot = float2(0.5, 0.5);
				// Rotation Matrix
				float cosAngle = cos(_Angle);
				float sinAngle = sin(_Angle);
				float2x2 rot = float2x2(cosAngle, -sinAngle, sinAngle, cosAngle);

				// Rotation consedering pivot
				float2 uv = v.uv.xy - pivot;
				o.uv = mul(rot, uv);
				o.uv += pivot;

				//o.uv = v.uv;
				return o;
			}

			fixed4 frag(v2f i, fixed facing : VFACE) : SV_Target
			{
				
				return (facing > 0) ? tex2D(_MainTex, i.uv) : tex2D(_MainTex2, i.uv);
			}
			ENDCG
		}
	}
}
