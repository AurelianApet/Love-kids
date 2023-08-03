Shader "Custom/Custom-Stretch" {
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
                float2 normCoord = 2.0 * tc -1.0;
                float2 center = float2(0.5,0.5);
                float2 normCenter = 2.0 * center -1.0;
                
                normCoord -= normCenter;
              
               	float2 s = sign(normCoord);
             
                normCoord = abs( normCoord);
                normCoord = 0.5 * normCoord + 0.5 * smoothstep(0.25,0.5,normCoord) * normCoord;
                normCoord = s * normCoord;
                
                normCoord += normCenter;
                
                float2 newTc = normCoord /2.0f + 0.5;
                
                float3 color = tex2D(_MainTex, newTc).rgb;
                return float4(color, 1.0);
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}