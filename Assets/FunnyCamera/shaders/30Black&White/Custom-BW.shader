Shader "Custom/Custom-BW" {
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
                float2 tc = i.uv;
                
               	float colorR = tex2D(_MainTex, tc).r;
               	float colorG = tex2D(_MainTex, tc).g;
               	float colorB = tex2D(_MainTex, tc).b;
               	
               	float colorC = (colorR + colorG +colorB)/3;
               	float3 newcolor;
               	if(colorC > 0.4)
               	{
               		newcolor = float3(1,1,1);
               	}
               	else
               	{
               		newcolor = float3(0,0,0);
               	}
                return float4(newcolor, 1.0);
            }
              
            ENDCG
        }
    }
    FallBack "Diffuse"
}