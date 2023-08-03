Shader "Custom/Custom-mirrorrepeat" {
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
                
			    uv.y -= 0.5;
			    uv.x -= 0.5;
			    vec2 uv2 = uv ;
			    //uv2 *= 2.0;
				uv2.x = 4.0 * abs(uv.x) / sqrt(abs(uv.y)); 
				uv2.y = 4.0  / sqrt(abs(uv.y)) + 0.5;
			    uv = uv2;
			    uv2.y +=10 * _Time;//iGlobalTime;
			   
			    float doFlip = 0.0;
			    if(abs(uv2.x) > 1.0)
			    {
			        doFlip = fmod(floor(uv2.x),2.0);
			    } else if(uv2.x < 0.0) {
			    	doFlip = 1.0;
			    }
			     
			    float doFlipY = 0.0;
			    if(abs(uv2.y) > 1.0)
			    {
			        doFlipY = fmod(floor(uv2.y),2.0);
			    } else if(uv2.y < 0.0) {
			    	doFlipY = 1.0;
			    }
			    
			    uv2 = fmod(uv2,1.0);
			    if(doFlip == 1.0)
			        uv2.x = 1.0 - uv2.x;              
			    if(doFlipY == 1.0)
			        uv2.y = 1.0 - uv2.y;              
			    
			    float4 fragColor = vec4( 0.0, 0.0, uv.y * 0.02, 1.0 );
			    fragColor = mix(texture2D(_MainTex, uv2),vec4(0.0,0.0,0.0,1.0), fragColor.z);
			  
				return fragColor;
			    //return texture2D(_MainTex, uv);
			    
            }
            
            ENDCG
        }
    }
    FallBack "Diffuse"
}