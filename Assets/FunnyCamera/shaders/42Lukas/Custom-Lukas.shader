Shader "Custom/Custom-Lukas" {
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
            
             float dxsize = 1.;
			 int blurSize = 4;
			// Size of a pixel.
			vec2 duv ;

			// Get blurred pixel color, less noise
			vec4 GetPixels(vec2 uv) {
			   vec4 color = vec4(0,0,0,0);
				
			    for (int i = 0; i < blurSize; ++i) {
			        for (int j = 0; j < blurSize; ++j) {
			          color += texture2D(_MainTex, uv + duv * vec2(i, j));
			        }
			    }
			    return color / float(blurSize * blurSize);
			}

			vec4 GetDx(vec4 color, vec2 uv) {
			    vec4 colorx = GetPixels(uv + vec2(duv.x, 0.));
			    return color - colorx;
			}

			vec4 GetDy(vec4 color, vec2 uv) {
			    vec4 colorx = GetPixels(uv + vec2(0., duv.y));
			    return color - colorx;
			}
			
			float mod(float a, float b)
			{
				return a - b * floor(a/b);
			}

            float4 frag(v2f iUI) : COLOR
            {
            
                float2 tc = iUI.uv;
                vec2 uv = tc;  
                dxsize = 1;
                blurSize = 4;
               	duv = vec2(1.0,1.0) / iResolution.xy * dxsize;
                
			    vec4 baseColor = GetPixels(uv);

			    // Try to capture skin color
			    vec4 skinColor = vec4(.38, .28, .28, 1.);
			    float skind = length(skinColor - baseColor);
			    skind = smoothstep(.16, 0.09, skind);

			    // Derivatives and edges.
			    vec4 dx = GetDx(baseColor, uv);
			    vec4 dy = GetDy(baseColor, uv);
			    vec4 mag = abs(dx) + abs(dy);
			    vec4 cut = smoothstep(.03, .06, mag);  // edge cut
			    
			    // Replace face color with wacky blue waves.
			    vec4 waves = vec4(.3,.3,.3,.3) + vec4(.3,.3,.3,.3) * sin(uv.xxyy * vec4(201., 97., 55., 333.) + iGlobalTime * vec4(-2.1,4.,-1.,9.));
			    vec4 faceColor = vec4(0.3, 0.1, 1., 1.) * (waves.x + waves.y + waves.z + waves.w);
			    
			    // face color, original color, and edges.
			    float4 fragColor = faceColor * skind + (1. - skind) * baseColor + cut;
			    
				return fragColor;
			    //return texture2D(_MainTex, uv);
			    
            }
            
            ENDCG
        }
    }
    FallBack "Diffuse"
}