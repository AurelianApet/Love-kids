Shader "Custom/Custom-Bluegrind" {
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
            
            float4 frag(v2f iUI) : COLOR
            {
                float2 tc = iUI.uv;
                
			vec2 uv = tc;
		    vec4 c = texture2D(_MainTex,uv);
		    c = sin(uv.x*10.+c*cos(c*6.28+iGlobalTime+uv.x)*sin(c+uv.y+iGlobalTime)*6.28)*.5+.5;
		    c.b+=length(c.rg);
			float4 fragColor = c;
			  
				return fragColor;
			    //return texture2D(_MainTex, uv);
			    
            }
            
            ENDCG
        }
    }
    FallBack "Diffuse"
}