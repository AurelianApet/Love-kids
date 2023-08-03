Shader "Custom/Custom-CrossStitching" {
    Properties {
        _MainTex ("Base (RGB)", 2D) = "white" {}
        time("time",range(0,1)) = 0
        rt_w("rt_w",range(0,1080)) = 480
        rt_h("rt_h",range(0,1920)) = 640
        stitching_size("stitching_size",range(0,10)) = 6.0
        [MaterialToggle] _invert("invert", int) = 1
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
            
			float rt_w = 480; // GeeXLab built-in
			float rt_h = 640; // GeeXLab built-in
 
            float4 _MainTex_ST;
            
			float time;
			float stitching_size = 6.0;
			int _invert;
			int invert = 0;

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
       
			float4 PostFX(sampler2D tex, float2 uv, float time)
			{
				if(_invert)
				{
					invert = 1;
				}
				else
				{
					invert = 0;
				}
				float4 c = float4(0.0,0.0,0.0,0.0);
				float size = stitching_size;
				float2 cPos = uv * float2(rt_w, rt_h);
				float2 tlPos = floor(cPos / float2(size, size));
				tlPos *= size;
				int remX = int(mod(cPos.x, size));
				int remY = int(mod(cPos.y, size));
				if (remX == 0 && remY == 0)
					tlPos = cPos;
				float2 blPos = tlPos;
				blPos.y += (size - 1.0);
				if ((remX == remY) ||(((int(cPos.x) - int(blPos.x)) == (int(blPos.y) - int(cPos.y)))))
				{
					if (invert == 1)
						c = float4(0.2, 0.15, 0.05, 1.0);
					else
						c = tex2D(tex, tlPos * float2(1.0/rt_w, 1.0/rt_h)) * 1.4;
				}
				else
				{
					if (invert == 1)
						c = tex2D(tex, tlPos * float2(1.0/rt_w, 1.0/rt_h)) * 1.4;
					else
						c = float4(0.0, 0.0, 0.0, 1.0);
				}
				return c;
			}

  			
            float4 frag(v2f i) : COLOR
            {   
            	float2 tc = i.uv;
            	float4 tempVec3;
            	//if(tc.y >0.5)
            	if(true)
            	{
            		tempVec3 =  PostFX(_MainTex, tc, time);
            	}
            	else
            	{
            		tc.y += 0.5;
            		float4 c1 = tex2D(_MainTex, tc);
            		tempVec3 = c1;
            	}
            	
				return tempVec3;
				
            }
              
            ENDCG
        }
    }
    FallBack "Diffuse"
}