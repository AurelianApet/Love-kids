Shader "Custom/Custom-NightVersion" {
    Properties {
        _MainTex ("Base (RGB)", 2D) = "white" {}
        noiseTex ("noiseTex", 2D) = "white" {}
        maskTex ("maskTex", 2D) = "white" {}
        luminanceThreshold ("luminanceThreshold ",range(0,1)) = 0.2
        colorAmplification ("colorAmplification ",range(0,10)) = 4
        effectCoverage ("effectCoverage ",range(0,1)) = 0.5
        
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
			
            sampler2D _MainTex;
			 sampler2D noiseTex; 
			 sampler2D maskTex; 
 
            float4 _MainTex_ST;
            
			 float elapsedTime; // seconds
			 float luminanceThreshold; // 0.2
			 float colorAmplification; // 4.0
			 float effectCoverage; // 0.5

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
       
  			
            float4 frag(v2f i) : COLOR
            {   
            	float2 tc = i.uv;
            	float4 finalColor = float4(0,0,0,0);
				if (true) 
				{
					vec2 uv;           
					uv.x = 0.4*sin(elapsedTime*50.0);                                 
					uv.y = 0.4*cos(elapsedTime*50.0);                                 
					float m = texture2D(maskTex, tc.xy).r;
					vec3 n = texture2D(noiseTex,(tc.xy*3.5) + uv).rgb;
					vec3 c = texture2D(_MainTex, tc.xy + (n.xy*0.005)).rgb;

					float lum = dot(vec3(0.30, 0.59, 0.11), c);
					if (lum < luminanceThreshold)
					  c *= colorAmplification; 

					vec3 visionColor = vec3(0.1, 0.95, 0.2);
					finalColor.rgb = (c + (n*0.2)) * visionColor * m;
				}
				else
				{
					finalColor = texture2D(_MainTex,tc.xy);
				}
				return finalColor;
				
            }
              
            ENDCG
        }
    }
    FallBack "Diffuse"
}