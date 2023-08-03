Shader "Custom/Custom-OnlyRed" {
    Properties {
        _MainTex ("Base (RGB)", 2D) = "white" {}
      
    }
    
    SubShader {
        Tags { "RenderType"="Opaque" }
        LOD 200
          
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
            
            #define vec2 float2
			#define vec3 float3
			#define vec4 float4
			#define mix lerp  
			#define texture2D tex2D  
			#define iGlobalTime _Time 
			#define iResolution _ScreenParams
			#define M_PI 3.1415926535897932384626433832795
			#define intensity 0.4
            sampler2D _MainTex;
            float4 _MainTex_ST;
           
            struct v2f
            {
                float4 pos : POSITION;
                float2 uv : TEXCOORD0;
            };

            v2f vert(appdata_full v)
            {
				v2f o;
				o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
				o.uv = TRANSFORM_TEX(v.texcoord, _MainTex);
				return o;
            }
			
			float mod(float a, float b)
			{
				return a - b * floor(a/b);
			}

            float4 frag(v2f iUI) : COLOR
            {
                float2 tc = iUI.uv;
                vec2 uv = tc;
                
				vec4 target = vec4(1.,0.,0.,1.);
				vec4 tempcolor = texture2D(_MainTex,uv);

				vec3 diff = tempcolor.xyz - target.xyz;
				float luminance = dot(tempcolor, vec4(0.299, 0.587, 0.114, 0.));
				vec4 bgColor1 = vec4(luminance, luminance, luminance, 1.0);

				if(dot(diff,diff) < intensity) 
				{
					bgColor1 = tempcolor;
				}
				return bgColor1;
			    //return texture2D(_MainTex, uv);
			    
            }
            
            ENDCG
        }
    }
    FallBack "Diffuse"
}