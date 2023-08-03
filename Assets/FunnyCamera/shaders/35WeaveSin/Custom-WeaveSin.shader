Shader "Custom/Custom-WeaveSin" {
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
            
            float4 frag(v2f i) : COLOR
            {
                float2 tc = i.uv;
                float2 temptc = i.uv;
             //  	tc.x = tc.x + (sin(tc.y*10 + _Time * 40)*0.1);
           	//s	tc.x = tc.x + (cos(tc.y*10)*0.1);
         
           		tc.x =  (tc.x + (cos(tc.y* 30)*0.05));
               
			    return texture2D(_MainTex,tc);
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}