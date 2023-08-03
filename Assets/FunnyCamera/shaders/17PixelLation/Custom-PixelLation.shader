Shader "Custom/Custom-PixelLation" {
    Properties {
        _MainTex ("Base (RGB)", 2D) = "white" {}
         vx_offset("vx_offset", range(0,1)) = 0.8
         
         rt_w("rt_w", range(0,640)) = 640
         rt_h("rt_h", range(0,480)) = 480
         pixel_w("pixel_w", range(0,10)) = 15
         pixel_h("pixel_h", range(0,10)) = 10
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
            
            
            float vx_offset;
            float rt_w;
            float rt_h;
            float pixel_w;
            float pixel_h;
            
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
                float2 uv = i.uv;
                
                float3 tc =  float3(1.0,0.0,0.0);
             //   if(uv.x < (vx_offset - 0.005))
	             if(true)
                {
                	float dx = pixel_w * (1.0/rt_w);
                	float dy = pixel_h * (1.0/rt_h);
                	
                	float2 coord = float2(dx * floor(uv.x/dx),dy * floor(uv.y/dy));
                	tc = tex2D(_MainTex, coord).rgb;
                	
                }
                else if(uv.x >= (vx_offset + 0.005) )
                {
                	tc = tex2D(_MainTex, uv).rgb;
                } 
               // float3 color = tex2D(_MainTex, tc).rgb;
                return float4(tc, 1.0);
            }
              
            ENDCG
        }
    }
    FallBack "Diffuse"
}