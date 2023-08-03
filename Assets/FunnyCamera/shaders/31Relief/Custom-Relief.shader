Shader "Custom/Custom-Relief" {
    Properties {  
        _MainTex ("MainTex", 2D) = "white" {}  
        _SizeW("_SizeW", range(1,1000)) = 480//size  
        _SizeH("_SizeH", range(1,1000)) = 640//size  
    }  
    SubShader {  
        pass{  
        Tags{"LightMode"="ForwardBase" }  
        Cull off  
        CGPROGRAM  
        #pragma vertex vert  
        #pragma fragment frag  
        #include "UnityCG.cginc"  
  
        float4 _Color;  
        float _SizeW;
        float _SizeH;
        
        sampler2D _MainTex;  
        float4 _MainTex_ST;  
        
        struct v2f {  
            float4 pos:SV_POSITION;  
            float2 uv_MainTex:TEXCOORD0;  
              
        };  
  
        v2f vert (appdata_full v) {  
            v2f o;  
            o.pos=mul(UNITY_MATRIX_MVP,v.vertex);  
            o.uv_MainTex = TRANSFORM_TEX(v.texcoord,_MainTex);  
            return o;  
        }  
        
        float4 frag(v2f i):COLOR  
        {
        	float2 onePixel = float2(1.0/_SizeW,1.0/_SizeH);
        	float4 color1;
        	color1.rgb = float3(0.5,0.5,0.5);
        	color1 -= tex2D(_MainTex, i.uv_MainTex - onePixel) * 5.0;
        	color1 += tex2D(_MainTex, i.uv_MainTex + onePixel) * 5.0;
        	float tempValue  = (color1.r + color1.g+ color1.b)/3.0;
        	color1.rgb = float3( tempValue,tempValue,tempValue );
        	return float4(color1.rgb, 1.0);
        }  
        ENDCG  
        }//  
  
    }   
    FallBack "Diffuse"
}