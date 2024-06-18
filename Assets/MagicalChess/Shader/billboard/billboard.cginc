//Look At Constraint --> CPU

//Precision nur hier angeben, im Shader-Menü nur Billboard als Funktion angeben
//Reihenfolge beachten von inputs und outputs
 void Billboard_float(float2 uv, float4x4 iv, out float3 opos) 
 {
     uv = (uv - float2(0.5, 0.5)) * 2.0;
     opos = mul(iv, float3(uv, 0));
 }