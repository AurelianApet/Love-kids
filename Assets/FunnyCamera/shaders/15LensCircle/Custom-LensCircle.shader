Shader "Custom/Custom-LensCircle" {
    Properties {
        lensRadiusX("lensRadiusX", range(0,1)) = 0
        lensRadiusY("lensRadiusY", range(0,1)) = 0
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
              
            sampler2D _MainTex;
            float lensRadiusX;
            float lensRadiusY;
              
            float  _Scale;
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
            	float4 color4 = tex2D(_MainTex, tc);
            	tc -= float2(0.5,0.5);
            	float dist = length(tc);
            	color4.rgb *= smoothstep(lensRadiusX,lensRadiusY,dist);
            	
                return color4;
            }
              
            ENDCG
        }
    }
    FallBack "Diffuse"
}