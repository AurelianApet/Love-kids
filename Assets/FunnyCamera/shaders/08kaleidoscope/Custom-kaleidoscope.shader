Shader "Custom/Custom-kaleidoscope" {
    Properties {
        _MainTex ("Base (RGB)", 2D) = "white" {}
        
        _Multiply("_Multiply", range(0,10)) = 5
        _Offset("_Offset", range(0,500)) = 10
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

 			float _Multiply;
            float2 position;
            float2 _Offset;
            
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
                
                float2 p = uv.xy - 0.5;
                float r = length(p);
                float a = atan2(p.y,p.x);
                float slides = _Multiply;
                float tau = 2.0 * 3.1416;
                
              //  a = a  - (tau/slides) * floor(a/(tau/slides));//mod(a,tau/slides);
                a = a  - (tau/slides) * floor(a/(tau/slides));//mod(a,tau/slides);
                a = abs( a - tau/slides/2.0f );
                position = r * float2(cos(a), sin(a)) + 0.5 + _Offset;
                float3 color = tex2D(_MainTex, position).rgb;

               // float3 color = tex2D(_MainTex, tc).rgb;
                return float4(color, 1.0);
            }
              
            ENDCG
        }
    }
    FallBack "Diffuse"
}