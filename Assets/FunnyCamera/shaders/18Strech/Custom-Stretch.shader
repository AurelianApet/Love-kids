Shader "Custom/Custom-Stretch" {
    Properties {
        _MainTex ("Base (RGB)", 2D) = "white" {}
        imageWidthFactor("imageWidthFactor",range(0,640)) = 1.0
        imageHeightFactor("imageHeightFactor",range(0,480)) = 1.0
        intensity("intensity",range(0,1)) = 1.0
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
            vec3 W;
            
            float intensity;
			float imageWidthFactor; 
			float imageHeightFactor; 
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
				W = vec3(0.2125, 0.7154, 0.0721);
				vec3 textureColor = texture2D(_MainTex, tc).rgb;

				vec2 stp0 = vec2(1.0 / imageWidthFactor, 0.0);
				vec2 st0p = vec2(0.0, 1.0 / imageHeightFactor);
				vec2 stpp = vec2(1.0 / imageWidthFactor, 1.0 / imageHeightFactor);
				vec2 stpm = vec2(1.0 / imageWidthFactor, -1.0 / imageHeightFactor);

				float i00   = dot( textureColor, W);
				float im1m1 = dot( texture2D(_MainTex, tc - stpp).rgb, W);
				float ip1p1 = dot( texture2D(_MainTex, tc + stpp).rgb, W);
				float im1p1 = dot( texture2D(_MainTex, tc - stpm).rgb, W);
				float ip1m1 = dot( texture2D(_MainTex, tc + stpm).rgb, W);
				float im10 = dot( texture2D(_MainTex, tc - stp0).rgb, W);
				float ip10 = dot( texture2D(_MainTex, tc + stp0).rgb, W);
				float i0m1 = dot( texture2D(_MainTex, tc - st0p).rgb, W);
				float i0p1 = dot( texture2D(_MainTex, tc + st0p).rgb, W);
				float h = -im1p1 - 2.0 * i0p1 - ip1p1 + im1m1 + 2.0 * i0m1 + ip1m1;
				float v = -im1m1 - 2.0 * im10 - im1p1 + ip1m1 + 2.0 * ip10 + ip1p1;

				float mag = 1.0 - length(vec2(h, v));
				vec3 target = vec3(mag,mag,mag);

				float4 color4 = vec4(mix(textureColor, target, intensity), 1.0);
            	
                return color4;
            }
              
            ENDCG
        }
    }
    FallBack "Diffuse"
}