Shader "Shader222" {
	Properties {
     // Adds Color field we can modify
     _Color ("Main Color", Color) = (1,1,1,1)
 }
 
 SubShader {
      Tags {"Queue"="Overlay" }
     Pass{
     Color [_Color]
     ZTest Always    
     }
 }
}
