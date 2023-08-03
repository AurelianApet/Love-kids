Shader "Custom/MirrorRight" {
    Properties {
        _MainTex("Base (RGB)", 2D) = "white" {}
        _Rotation("Rotation", Float) = 0.0
    }
 
    SubShader {
        Pass {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
 
            sampler2D _MainTex;
            fixed4 _MainTex_ST;
            float _Rotation;
            float _OffsetX;
            float _OffsetY;
 
            struct appdata {
                float4 vertex : POSITION;
                float4 texcoord : TEXCOORD0;
            };
 
            struct v2f {
                float4 pos : SV_POSITION;
                float2 uv : TEXCOORD0;
            };
   
            v2f vert (appdata v) {
         
                v2f o;
                o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
 
 				float x = v.texcoord.x ; //* _MainTex_ST.x + _MainTex_ST.z - offsetX;
                float y = v.texcoord.y; //* _MainTex_ST.y + _MainTex_ST.w - offsetY;
 				
 				
                o.uv =  float2(x, y) ;
               
                return o;
            }
   
            fixed4 frag (v2f i) : COLOR {
                return tex2D(_MainTex, i.uv);
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}