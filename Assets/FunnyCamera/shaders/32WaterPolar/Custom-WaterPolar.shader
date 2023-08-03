Shader "Custom/Custom-WaterPolar" {
    Properties {
        _MainTex ("Base (RGB)", 2D) = "white" {}
        
		CenterX("CenterX",range(0,1)) = 0.5
		CenterY("CenterY",range(0,1)) = 0.5
          
		PixelSizeX("PixelSizeX",range(0,0.1)) = 0.05
		PixelSizeY("PixelSizeY",range(0,0.1)) = 0.05
 
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
            
            float4 _MainTex_ST;
            
            float CenterX;
			float CenterY;
			float PixelSizeX;
			float PixelSizeY;
			
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
                vec2 center = vec2(CenterX,CenterY);
                
               	vec2 normCoord = 2.0 * tc - 1.0;
				vec2 normCenter = 2.0 * center - 1.0;
				
				normCoord -= normCenter;
				float r = length(normCoord);
				float phi = atan2(normCoord.y, normCoord.x);
				
				r = r - fmod(r, PixelSizeX) + 0.03;
				phi = phi - fmod(phi, PixelSizeY);
				normCoord.x = r * cos(phi);
				normCoord.y = r * sin(phi);
				normCoord += normCenter;
				vec2 textureCoordinateToUse = normCoord / 2.0 + 0.5;
				
				return	texture2D(_MainTex, textureCoordinateToUse);
				
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}