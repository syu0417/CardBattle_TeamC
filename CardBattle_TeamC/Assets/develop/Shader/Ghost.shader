Shader "Custom/Ghost"
{
	Properties{
			_Color("Color", Color) = (1,1,1,1)
			_MainTex("Albedo (RGB)", 2D) = "white" {}
			_Glossiness("Smoothness", Range(0,1)) = 0.5
			_Metallic("Metallic", Range(0,1)) = 0.0
			_Cutoff("Cutoff"      , Range(0, 1)) = 0.5
			_xhamon("X波度", Float) = 0.5
			_yhamon("Y波度", Float) = 0.5
			_zhamon("Z波度", Float) = 0.5
			_Height("高さ", Float) = 0.0
	}

	SubShader{
			Tags{
				"Queue" = "AlphaTest"
				"RenderType" = "TransparentCutout"
			}

			LOD 200
			Cull off
				
			CGPROGRAM

			// Physically based Standard lighting model, and enable shadows on all light types 
			#pragma surface surf Standard fullforwardshadows alphatest:_Cutoff   vertex:vert   

			//#pragma surface surf Lambert vertex:vert
			// Use shader model 3.0 target, to get nicer looking lighting
			#pragma target 3.0

			sampler2D _MainTex;

			struct Input
			{
				float2 uv_MainTex;
				float3 worldPos;
			};



			half _Glossiness;
			half _Metallic;
			fixed4 _Color;
			float _xhamon;
			float _yhamon;
			float _zhamon;
			float _Height;
		

			void vert(inout appdata_full v, out Input o)
			{
				UNITY_INITIALIZE_OUTPUT(Input, o);

				float distance = -sqrt((v.vertex.x * v.vertex.x) + (v.vertex.z * v.vertex.z));
				float xamp = _xhamon * sin(_Time * 50 + distance);
				float yamp = _yhamon * cos(_Time * 50 + distance);
				float zamp = _zhamon * sin(_Time * 50 + distance);

				
				
				float3 dis1 = float3(v.vertex.x + xamp, v.vertex.y + yamp, v.vertex.z + zamp);
				float3 dis2 = float3(v.vertex.x, v.vertex.y, v.vertex.z);

				o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
				//v.vertex.xyz = dis1;
				v.vertex.xyz = lerp(dis2, dis1, o.worldPos.y*0.005);
				//v.vertex.xyz = ( dis >= _Height) ? float3(v.vertex.x + xamp, v.vertex.y + yamp, v.vertex.z + zamp) : float3(v.vertex.x , v.vertex.y , v.vertex.z );
			}

			void surf(Input IN, inout SurfaceOutputStandard o)
			{
				// Albedo comes from a texture tinted by color
				fixed4 c = tex2D(_MainTex, IN.uv_MainTex)*_Color;
				o.Albedo = c.rgb;
				// Metallic and smoothness come from slider variables
				o.Metallic = _Metallic;
				o.Smoothness = _Glossiness;
				o.Alpha = c.a;
			}
			ENDCG
	}
	FallBack "Transparent/Cutout/Diffuse"
}