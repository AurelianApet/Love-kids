Shader "Custom/Custom-Ripple" {
    Properties {
        _MainTex ("Base (RGB)", 2D) = "white" {}
         _Scale("Scale", range(0,1)) = 0.8
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
                
                float2 p = -1.0 + 2.0*tc;
                float len = length(p);
                
                float2 uv = tc  + ( p/len ) * cos(len *12.0 - _Scale * _Time * 4.0 ) * 0.03;
                
                float3 color = tex2D(_MainTex, uv).rgb;
                return float4(color, 1.0);
            }
              
            ENDCG
        }
    }
    FallBack "Diffuse"
}