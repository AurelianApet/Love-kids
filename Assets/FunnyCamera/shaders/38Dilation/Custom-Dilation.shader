Shader "Custom/Custom-Dilation" {
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
			 	float2 offsets = float2(offsetX,offsetY);
                centerTextureCoordinate = tc;
                oneStepPositiveTextureCoordinate = tc + offsets;
                oneStepNegativeTextureCoordinate = tc - offsets;
                
                twoStepsPositiveTextureCoordinate = tc + 2*offsets;
                twoStepsNegativeTextureCoordinate = tc - 2*offsets;
                
                threeStepsPositiveTextureCoordinate = tc + 3*offsets;
                threeStepsNegativeTextureCoordinate = tc - 3*offsets;
                
                fourStepsPositiveTextureCoordinate = tc + 4 *offsets;
                fourStepsNegativeTextureCoordinate = tc - 4 * offsets;
                
                float centerIntensity = texture2D(_MainTex, centerTextureCoordinate).r;
                float centerIntensityG = texture2D(_MainTex, centerTextureCoordinate).g;
                float centerIntensityB = texture2D(_MainTex, centerTextureCoordinate).b;
                
				float oneStepPositiveIntensity = texture2D(_MainTex, oneStepPositiveTextureCoordinate).r;
				float oneStepPositiveIntensityG = texture2D(_MainTex, oneStepPositiveTextureCoordinate).g;
				float oneStepPositiveIntensityB = texture2D(_MainTex, oneStepPositiveTextureCoordinate).b;
				
				float oneStepNegativeIntensity = texture2D(_MainTex, oneStepNegativeTextureCoordinate).r;
				float oneStepNegativeIntensityG = texture2D(_MainTex, oneStepNegativeTextureCoordinate).g;
				float oneStepNegativeIntensityB = texture2D(_MainTex, oneStepNegativeTextureCoordinate).b;
				
				float twoStepsPositiveIntensity = texture2D(_MainTex, twoStepsPositiveTextureCoordinate).r;
				float twoStepsPositiveIntensityG = texture2D(_MainTex, twoStepsPositiveTextureCoordinate).g;
				float twoStepsPositiveIntensityB = texture2D(_MainTex, twoStepsPositiveTextureCoordinate).b;
				
				float twoStepsNegativeIntensity = texture2D(_MainTex, twoStepsNegativeTextureCoordinate).r;
				float twoStepsNegativeIntensityG = texture2D(_MainTex, twoStepsNegativeTextureCoordinate).g;
				float twoStepsNegativeIntensityB = texture2D(_MainTex, twoStepsNegativeTextureCoordinate).b;
				
				float threeStepsPositiveIntensity = texture2D(_MainTex, threeStepsPositiveTextureCoordinate).r;
				float threeStepsPositiveIntensityG = texture2D(_MainTex, threeStepsPositiveTextureCoordinate).g;
				float threeStepsPositiveIntensityB = texture2D(_MainTex, threeStepsPositiveTextureCoordinate).b;
				
				
				float threeStepsNegativeIntensity = texture2D(_MainTex, threeStepsNegativeTextureCoordinate).r;
				float threeStepsNegativeIntensityG = texture2D(_MainTex, threeStepsNegativeTextureCoordinate).g;
				float threeStepsNegativeIntensityB = texture2D(_MainTex, threeStepsNegativeTextureCoordinate).b;
				
		//		float fourStepsPositiveIntensity = texture2D(_MainTex, fourStepsPositiveTextureCoordinate).r;
		//		float fourStepsNegativeIntensity = texture2D(_MainTex, fourStepsNegativeTextureCoordinate).r;
				
				float minValue = max(centerIntensity, oneStepPositiveIntensity);
			    minValue = max(minValue, oneStepNegativeIntensity);
			    minValue = max(minValue, twoStepsPositiveIntensity);
			    minValue = max(minValue, twoStepsNegativeIntensity);
			    minValue = max(minValue, threeStepsPositiveIntensity);
			    minValue = max(minValue, threeStepsNegativeIntensity);
			    
			    	float minValueG = max(centerIntensityG, oneStepPositiveIntensityG);
			    minValueG = max(minValueG, oneStepNegativeIntensityG);
			    minValueG = max(minValueG, twoStepsPositiveIntensityG);
			    minValueG = max(minValueG, twoStepsNegativeIntensityG);
			    minValueG = max(minValueG, threeStepsPositiveIntensityG);
			    minValueG = max(minValueG, threeStepsNegativeIntensityG);
			    
			    	float minValueB = max(centerIntensityB, oneStepPositiveIntensityB);
			    minValueB = max(minValueB, oneStepNegativeIntensityB);
			    minValueB = max(minValueB, twoStepsPositiveIntensityB);
			    minValueB = max(minValueB, twoStepsNegativeIntensityB);
			    minValueB = max(minValueB, threeStepsPositiveIntensityB);
			    minValueB = max(minValueB, threeStepsNegativeIntensityB);
			    
		//	    minValue = max(minValue, fourStepsPositiveIntensity);
		//	    minValue = max(minValue, fourStepsNegativeIntensity);
				
			    return float4(float3(minValue,minValueG,minValueB),1.0);
            }
            
            ENDCG
        }
    }
    FallBack "Diffuse"
}