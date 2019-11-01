Shader "Unlit/CardReversible"
{
	Properties
	{
		_Color("Color", Color) = (1,1,1,1)
		_MainTex("表面テクスチャ", 2D) = "white" {}
		_MainTex1("裏面テクスチャ", 2D) = "white"{}
		_Angle("角度", float) = 11.0
	}
		SubShader
		{
			Tags {
				"Queue" = "AlphaTest"
				"RenderType" = "TransparentCutout"
			}
			LOD 100
			Cull off

			Pass{
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
				sampler2D _MainTex1;

				

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

				//void surf(Input IN, inout SurfaceOutputStandard o)
				//{
				//	// Albedo comes from a texture tinted by color
				//	fixed4 c = tex2D(_MainTex, IN.uv_MainTex)*_Color;
				//	o.Albedo = c.rgb;
				//	// Metallic and smoothness come from slider variables

				//	o.Alpha = c.a;
				//}

				fixed4 frag(v2f i, fixed facing : VFACE) : SV_Target
				{

					return (facing > 0) ? tex2D(_MainTex, i.uv) : tex2D(_MainTex1, i.uv);
				}
				ENDCG

			}
		}

			FallBack "Diffuse"
		//SubShader
		//{
		//	Tags {
		//		"Queue" = "AlphaTest"
		//		"RenderType" = "TransparentCutout"
		//	}
		//			
		//	LOD 200
		//	Cull off
		//	CGPROGRAM
		//	// Physically based Standard lighting model, and enable shadows on all light types 
		//	#pragma surface surf Standard fullforwardshadows alphatest:_Cutoff
		//	// Use shader model 3.0 target, to get nicer looking lighting
		//	#pragma target 3.0

		//	sampler2D _MainTex1;

		//	struct Input
		//	{
		//		float2 uv_MainTex1;
		//	};

		//	half _Glossiness;
		//	half _Metallic;
		//	fixed4 _Color;


		//	void surf(Input IN, inout SurfaceOutputStandard o)
		//	{
		//		// Albedo comes from a texture tinted by color
		//		fixed4 c = tex2D(_MainTex1, IN.uv_MainTex1)*_Color;
		//		o.Albedo = c.rgb;
		//		// Metallic and smoothness come from slider variables
		//		
		//		o.Alpha = c.a;
		//	}
		//	ENDCG
		//}
		//FallBack "Transparent/Cutout/Diffuse"
}