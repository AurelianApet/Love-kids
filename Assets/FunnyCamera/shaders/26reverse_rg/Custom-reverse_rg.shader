Shader "Custom/Custom-reverse_rg" {
    Properties {
        _MainTex ("Base (RGB)", 2D) = "white" {}
           [MaterialToggle] _isFlip("is Flip", float) = 1
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
                float2 uv = i.uv;
                float3 tc =  tex2D(_MainTex, uv).rgb;
                tc = float3(1 -tc.r,1 -tc.g,tc.b);
               	//tc = 1-tc;
                return float4(tc, 1.0);
            }
              
            ENDCG
        }
    }
    FallBack "Diffuse"
}