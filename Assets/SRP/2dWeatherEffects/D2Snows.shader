// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Hidden/AkilliMum/SRP/D2WeatherEffects/Post/D2Snows" {

HLSLINCLUDE

        #include "Packages/com.unity.postprocessing/PostProcessing/Shaders/StdLib.hlsl"

        TEXTURE2D_SAMPLER2D(_MainTex, sampler_MainTex);
        
        float _CameraSpeedMultiplier = 1.0;
        float _UVChangeX = 1.0;
        float _UVChangeY = 1.0;
        float _Multiplier = 10;
        //float _Size = 0.5;
        float _Speed = 4;
        float _Zoom = 1.2;
        float _Direction = 0.2;
        float _DarkMode = 0;
        float _DarkMultiplier = 1;  
        float _LuminanceAdd = 0.001;
        half4 _Color = (1, 1, 1, 1);
        
        float mod(float a, float b)
        {
            return a - floor(a / b) * b;
        }
        float2 mod(float2 a, float2 b)
        {
            return a - floor(a / b) * b;
        }
        float3 mod(float3 a, float3 b)
        {
            return a - floor(a / b) * b;
        }
        float4 mod(float4 a, float4 b)
        {
            return a - floor(a / b) * b;
        } 

        float calcSnow(float2 uv)
        {
            const float3x3 p = float3x3(13.323122,23.5112,21.71123,21.1212,
                28.7312,11.9312,21.8112,14.7212,61.3934);
            
            float snow = 0.;
            for (float im=0.; im < _Multiplier; im+=1)
            {
                float2 q = uv * im*_Zoom;
                float w = _Direction * mod(im*7.238917,1.0)-_Direction*0.1*sin(_Time.y+im);
                q += float2(q.y*w, _Speed*_Time.y / (1.0+im*_Zoom*0.03));
                float3 n = float3(floor(q),31.189+im);
                float3 m = floor(n)*0.00001 + frac(n);
                float3 mp = (31314.9+m) / frac(mul(p,m));
                float3 r = frac(mp);
                float2 s = abs(mod(q,1.0) -0.5 +0.9*r.xy -0.45);
                s += 0.01*abs(2.0*frac(10.*q.yx)-1.); 
                float d = 0.6*max(s.x-s.y,s.x+s.y)+max(s.x,s.y)-.01;
                snow += smoothstep(0.1,-0.1,d)*(r.x/(1.+.02*im*_Zoom));
            }
            return snow;
        }
        
        float4 Frag(VaryingsDefault i) : SV_Target
        {
            //float4 color = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.texcoord);
            //float luminance = dot(color.rgb, float3(0.2126729, 0.7151522, 0.0721750));
            //color.rgb = lerp(color.rgb, luminance.xxx, _Blend.xxx);
            //return color;
            float2 fogUV = float2 (i.texcoord.x + _UVChangeX*_CameraSpeedMultiplier, i.texcoord.y + _UVChangeY*_CameraSpeedMultiplier);
            float snow = calcSnow(fogUV);
            float4 tex = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.texcoord);
            if(_DarkMode==1){
                half lum = tex.r*.3 + tex.g*.59 + tex.b*.11;
                return tex*(1-snow*_Color.a)+(lum+_LuminanceAdd)*snow*_Color.a*_Color*_DarkMultiplier;
            } 
            else{
                return tex*(1-snow*_Color.a)+snow*_Color.a*_Color*_DarkMultiplier;
            }
        }

    ENDHLSL

    SubShader
    {
        Cull Off ZWrite Off ZTest Always

        Pass
        {
            HLSLPROGRAM

                #pragma vertex VertDefault
                #pragma fragment Frag

            ENDHLSL
        }
    }
}
   