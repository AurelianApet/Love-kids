Shader "Custom/Custom-Cartoon" {
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
			#define iGlobalTime _Time.y  
			#define iResolution _ScreenParams
			#define M_PI 3.1415926535897932384626433832795
			
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
            
            float4 frag(v2f iUI) : COLOR
            {
                float2 tc = iUI.uv;
                
				vec2 uv =tc;// fragCoord.xy / iResolution.xy;
				vec3 col = texture2D(_MainTex, uv).rgb;
				float lum = col.r + col.g + col.b;
				vec2 deriv = vec2(ddx(lum), ddy(lum));
				float edge = sqrt(dot(deriv,deriv));
				if(edge > 0.09) discard;
				float4 fragColor = vec4(col,1.0);
				
				return fragColor;
			    //return texture2D(_MainTex, uv);
			    
            }
            
            ENDCG
        }
    }
    FallBack "Diffuse"
}