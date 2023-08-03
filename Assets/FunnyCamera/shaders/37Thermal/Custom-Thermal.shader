Shader "Custom/Custom-Thermal" {
    Properties {
        _MainTex ("Base (RGB)", 2D) = "white" {}
       offsetX("offsetX",range(0,0.01)) = 0.005
       offsetY("offsetY",range(0,0.01)) = 0.005
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
            
            
            vec2 centerTextureCoordinate;
			vec2 oneStepPositiveTextureCoordinate;
			vec2 oneStepNegativeTextureCoordinate;
			 vec2 twoStepsPositiveTextureCoordinate;
			 vec2 twoStepsNegativeTextureCoordinate;
			 vec2 threeStepsPositiveTextureCoordinate;
			 vec2 threeStepsNegativeTextureCoordinate;
			 vec2 fourStepsPositiveTextureCoordinate;
			 vec2 fourStepsNegativeTextureCoordinate;
			 
			 float offsetX;
			 float offsetY;
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
			 	vec4 pixcol = texture2D(_MainTex, tc);
				
				vec4 colors[3];
				colors[0] = vec4(0.,0.,1.,1.);
				colors[1] = vec4(1.,1.,0.,1.);
				colors[2] = vec4(1.,0.,0.,1.);
				float lum = (pixcol.r+pixcol.g+pixcol.b)/3.;
				int ix = (lum < 0.5)? 0:1;
				vec4 thermal = mix(colors[ix],colors[ix+1],(lum-float(ix)*0.5)/0.5);
				
			    return thermal;
            }
            
            ENDCG
        }
    }
    FallBack "Diffuse"
}