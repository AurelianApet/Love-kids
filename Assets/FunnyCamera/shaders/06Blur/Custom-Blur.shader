Shader "Custom/Custom-Blur" {
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
            	float offset[3];
            	float weight[3];
            	
            	offset[0] = 0.0;
            	offset[1] = 7.3846153846;
            	offset[2] = 5.2307692308;
            	weight[0] = 0.2270270270;
            	weight[1] = 0.3162162162;
            	weight[2] = 0.0702702703 ;
            	
                float2 tc = i.uv;
                float3 colortemp = float3(1.0,0.0,0.0);
                if(true)
                {
                	float2  uv =i.uv;
					colortemp = tex2D(_MainTex, uv).rgb * weight[0];
					
					for (int i=1; i<3; i++) 
					{
					  colortemp += tex2D(_MainTex, uv + float2(offset[i]/480, 0.0)).rgb * weight[i];
					  colortemp += tex2D(_MainTex, uv - float2(offset[i]/480, 0.0)).rgb * weight[i];
					}
                }
                else 
				{
				
				}
                
                return float4(colortemp, 1.0);
            }
              
            ENDCG
        }
    }
    FallBack "Diffuse"
}