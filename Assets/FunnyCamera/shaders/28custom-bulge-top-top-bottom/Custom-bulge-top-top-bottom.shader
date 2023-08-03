Shader "Custom/Custom-bulge-top-top-bottom" {
     Properties {
        _MainTex ("Base (RGB)", 2D) = "white" {}
         _Scale("Scale", range(-1,1)) = 0.8
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
              
            float  _Scale;
            float4 _MainTex_ST;
            float _isFlip;
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
                tc -= float2(0.1,0.75);
                float dist = length(tc);
                float radius = 0.4f;
                
                tc = i.uv;
                if (dist < radius)
                {
                	tc -= float2(0.1,0.75);
                    float percent = 1.0 - ((radius - dist) / radius) * _Scale;
                    percent = percent * percent;
                    
                    tc = tc * percent;
                    tc += float2(0.1,0.75);
                }
                
                float2 temptc = tc;
                tc -= float2(0.9,0.75);
                dist = length(tc);
                radius = 0.4f;
                tc = temptc;
                if (dist < radius)
                {
                	tc -= float2(0.9,0.75);
                    float percent = 1.0 - ((radius - dist) / radius) * _Scale;
                    percent = percent * percent;
                    
                    tc = tc * percent;
                    tc += float2(0.9,0.75);
                }
                
                 float2 temptc2 = tc;
                tc -= float2(0.5,0.25);
                dist = length(tc);
                radius = 0.4f;
                tc = temptc2;
                if (dist < radius)
                {
                	tc -= float2(0.5,0.25);
                    float percent = 1.0 - ((radius - dist) / radius) * _Scale;
                    percent = percent * percent;
                    
                    tc = tc * percent;
                    tc += float2(0.5,0.25);
                }
                
                
                float3 color = tex2D(_MainTex, tc).rgb;
                return float4(color, 1.0);
            }
              
            ENDCG
        }
    }
    FallBack "Diffuse"
}