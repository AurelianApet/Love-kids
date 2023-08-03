Shader "Custom/Custom-Sobel" {
    Properties {
        _MainTex ("Base (RGB)", 2D) = "white" {}
        
		ImageWidth("ImageWidth",range(0,1000)) = 640
		ImageHeight("ImageHeight",range(0,1000)) = 480
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
            
            float ImageWidth;
            float ImageHeight;
            
			vec2 textureCoordinate;
			vec2 leftTextureCoordinate;
			vec2 rightTextureCoordinate;

			vec2 topTextureCoordinate;
			vec2 topLeftTextureCoordinate;
			vec2 topRightTextureCoordinate;

			vec2 bottomTextureCoordinate;
			vec2 bottomLeftTextureCoordinate;
			vec2 bottomRightTextureCoordinate;
			
			float edgeStrength;
			
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
                edgeStrength = 1.0;
                bottomLeftTextureCoordinate = float2(tc.x - 1/ImageWidth,tc.y - 1/ImageHeight);
                topRightTextureCoordinate = float2(tc.x + 1/ImageWidth,tc.y + 1/ImageHeight);
                topLeftTextureCoordinate = float2(tc.x - 1/ImageWidth,tc.y + 1/ImageHeight);
                bottomRightTextureCoordinate = float2(tc.x + 1/ImageWidth,tc.y - 1/ImageHeight);
                
                leftTextureCoordinate = float2(tc.x - 1/ImageWidth,tc.y);
                rightTextureCoordinate = float2(tc.x + 1/ImageWidth,tc.y);
                bottomTextureCoordinate = float2(tc.x,tc.y - 1/ImageHeight);
                topTextureCoordinate = float2(tc.x,tc.y + 1/ImageHeight);
                
				float bottomLeftIntensity = texture2D(_MainTex, bottomLeftTextureCoordinate).r;
				float topRightIntensity = texture2D(_MainTex, topRightTextureCoordinate).r;
				float topLeftIntensity = texture2D(_MainTex, topLeftTextureCoordinate).r;
				float bottomRightIntensity = texture2D(_MainTex, bottomRightTextureCoordinate).r;
				float leftIntensity = texture2D(_MainTex, leftTextureCoordinate).r;
				float rightIntensity = texture2D(_MainTex, rightTextureCoordinate).r;
				float bottomIntensity = texture2D(_MainTex, bottomTextureCoordinate).r;
				float topIntensity = texture2D(_MainTex, topTextureCoordinate).r;
				float h = -topLeftIntensity - 2.0 * topIntensity - topRightIntensity + bottomLeftIntensity + 2.0 * bottomIntensity + bottomRightIntensity;
				float v = -bottomLeftIntensity - 2.0 * leftIntensity - topLeftIntensity + bottomRightIntensity + 2.0 * rightIntensity + topRightIntensity;

				float mag = length(vec2(h, v)) * edgeStrength;
			   
			    return vec4(vec3(mag,mag,mag), 1.0);
				
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}