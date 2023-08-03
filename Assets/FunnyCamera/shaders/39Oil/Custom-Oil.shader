Shader "Custom/Custom-Oil" {
    Properties {
        _MainTex ("Base (RGB)", 2D) = "white" {}
       imageWidth("imageWidth",range(0,100)) = 80.0
       imageHeight("imageHeight",range(0,100)) = 90.0
       radius("radius",range(1,3)) = 1
      
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
			#define iResolution _ScreenParams
			#define M_PI 3.1415926535897932384626433832795
			
            sampler2D _MainTex;
            
            float4 _MainTex_ST;
           
			 
			 float imageWidth;
			 float imageHeight;
			 float radius;
			 float2 src_size;
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
            
            
            float4 frag(v2f vtwof) : COLOR
            {
                float2 tc = vtwof.uv;
                src_size = float2(imageWidth,imageHeight);
			 	vec2 uv = tc;
			     float n = float((radius + 1) * (radius + 1));
			     int i; int j;
			     vec3 m0 = vec3(0.0,0.0,0.0); vec3 m1 = vec3(0.0,0.0,0.0); vec3 m2 = vec3(0.0,0.0,0.0); vec3 m3 = vec3(0.0,0.0,0.0);
			     vec3 s0 = vec3(0.0,0.0,0.0); vec3 s1 = vec3(0.0,0.0,0.0); vec3 s2 = vec3(0.0,0.0,0.0); vec3 s3 = vec3(0.0,0.0,0.0);
			     vec3 c;

			     for (j = -radius; j <= 0; ++j)  {
			         for (i = -radius; i <= 0; ++i)  {
			             c = texture2D(_MainTex, uv + vec2(i,j) / src_size).rgb;
			             m0 += c;
			             s0 += c * c;
			         }
			     }

			     for (j = -radius; j <= 0; ++j)  {
			         for (i = 0; i <= radius; ++i)  {
			             c = texture2D(_MainTex, uv + vec2(i,j) / src_size).rgb;
			             m1 += c;
			             s1 += c * c;
			         }
			     }

			     for (j = 0; j <= radius; ++j)  {
			         for (i = 0; i <= radius; ++i)  {
			             c = texture2D(_MainTex, uv + vec2(i,j) / src_size).rgb;
			             m2 += c;
			             s2 += c * c;
			         }
			     }

			     for (j = 0; j <= radius; ++j)  {
			         for (i = -radius; i <= 0; ++i)  {
			             c = texture2D(_MainTex, uv + vec2(i,j) / src_size).rgb;
			             m3 += c;
			             s3 += c * c;
			         }
			     }


				float4 colorValue;
			     float min_sigma2 = 1e+2;
			     m0 /= n;
			     s0 = abs(s0 / n - m0 * m0);

			     float sigma2 = s0.r + s0.g + s0.b;
			     if (sigma2 < min_sigma2) {
			         min_sigma2 = sigma2;
			         colorValue = vec4(m0, 1.0);
			     }

			     m1 /= n;
			     s1 = abs(s1 / n - m1 * m1);
				
				
			     sigma2 = s1.r + s1.g + s1.b;
			     if (sigma2 < min_sigma2) {
			         min_sigma2 = sigma2;
			         colorValue = vec4(m1, 1.0);
			     }

			     m2 /= n;
			     s2 = abs(s2 / n - m2 * m2);

			     sigma2 = s2.r + s2.g + s2.b;
			     if (sigma2 < min_sigma2) {
			         min_sigma2 = sigma2;
			         colorValue = vec4(m2, 1.0);
			     }

			     m3 /= n;
			     s3 = abs(s3 / n - m3 * m3);

			     sigma2 = s3.r + s3.g + s3.b;
			     if (sigma2 < min_sigma2) {
			         min_sigma2 = sigma2;
			         colorValue = vec4(m3, 1.0);
			     }
				
			    return colorValue;
            }
            
            ENDCG
        }
    }
    FallBack "Diffuse"
}