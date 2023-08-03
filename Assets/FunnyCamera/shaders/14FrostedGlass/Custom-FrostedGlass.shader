Shader "Custom/Custom-FrostedGlass" {
    Properties {
        _MainTex ("Base (RGB)", 2D) = "white" {}
        NoiseTex("NoiseTex",2D) = "white"{}
        vx_offset("vx_offset",range(0,1)) = 0.5
        PixelX("PixelX",range(0,10)) = 2
        PixelY("PixelY",range(0,10)) = 2
        Freq ("Freq ",range(0,10)) = 0.115
      
        rt_w("rt_w",range(0,1080)) = 480
        rt_h("rt_h",range(0,1920)) = 640
        
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
            sampler2D NoiseTex; // 1
			float rt_w = 480; // 
			float rt_h = 640; // 
 
            float4 _MainTex_ST;
            
			float vx_offset;
			float PixelX = 2.0;
			float PixelY = 2.0;
			float Freq = 0.115;

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
       
       
  			float mod(float a, float b)
  			{
  				return  a  - (b * floor(a/b));//mod(a,tau/slides);
  			}
			float4 spline(float x, float4 c1, float4 c2, float4 c3, float4 c4,float4 c5, float4 c6, float4 c7, float4 c8, float4 c9)
			{
				float w1, w2, w3, w4, w5, w6, w7, w8, w9;
				w1 = 0.0;
				w2 = 0.0;
				w3 = 0.0;
				w4 = 0.0;
				w5 = 0.0;
				w6 = 0.0;
				w7 = 0.0;
				w8 = 0.0;
				w9 = 0.0;
				float tmp = x * 8.0;
				if (tmp<=1.0) {
					w1 = 1.0 - tmp;
					w2 = tmp;
				}
				else if (tmp<=2.0) {
					tmp = tmp - 1.0;
					w2 = 1.0 - tmp;
					w3 = tmp;
				}
				else if (tmp<=3.0) {
					tmp = tmp - 2.0;
					w3 = 1.0-tmp;
					w4 = tmp;
				}
				else if (tmp<=4.0) {
					tmp = tmp - 3.0;
					w4 = 1.0-tmp;
					w5 = tmp;
				}
				else if (tmp<=5.0) {
					tmp = tmp - 4.0;
					w5 = 1.0-tmp;
					w6 = tmp;
				}
				else if (tmp<=6.0) {
					tmp = tmp - 5.0;
					w6 = 1.0-tmp;
					w7 = tmp;
				}
				else if (tmp<=7.0) {
					tmp = tmp - 6.0;
					w7 = 1.0 - tmp;
					w8 = tmp;
				}
				else 
				{

				tmp = clamp(tmp - 7.0, 0.0, 1.0);
				w8 = 1.0-tmp;
				w9 = tmp;
				}
				return w1*c1 + w2*c2 + w3*c3 + w4*c4 + w5*c5 + w6*c6 + w7*c7 + w8*c8 + w9*c9;
			}
			
			float3 NOISE2D(float2 p)
  			{ 
  				return tex2D(NoiseTex,p).xyz; 
  			}
  			
            float4 frag(v2f i) : COLOR
            {   
            	float2 uv = i.uv;
  				float3 tc = float3(1.0, 0.0, 0.0);
    
				//if (uv.x < (vx_offset-0.005))
				if(true)
				{
					float DeltaX = PixelX / rt_w;
					float DeltaY = PixelY / rt_h;
					float2 ox = float2(DeltaX,0.0);
					float2 oy = float2(0.0,DeltaY);
					float2 PP = uv - oy;
					float4 C00 = tex2D(_MainTex,PP - ox);
					float4 C01 = tex2D(_MainTex,PP);
					float4 C02 = tex2D(_MainTex,PP + ox);
					PP = uv;
					float4 C10 = tex2D(_MainTex,PP - ox);
					float4 C11 = tex2D(_MainTex,PP);
					float4 C12 = tex2D(_MainTex,PP + ox);
					PP = uv + oy;
					float4 C20 = tex2D(_MainTex,PP - ox);
					float4 C21 = tex2D(_MainTex,PP);
					float4 C22 = tex2D(_MainTex,PP + ox);

					float n = NOISE2D(Freq*uv).x;
					n = mod(n, 0.111111)/0.111111;
					float4 result = spline(n,C00,C01,C02,C10,C11,C12,C20,C21,C22);
					tc = result.rgb;  
				}
				else if (uv.x>=(vx_offset+0.005))
				{
					tc = tex2D(_MainTex, uv).rgb;
				}
				return float4(tc, 1.0);
            }
              
            ENDCG
        }
    }
    FallBack "Diffuse"
}