// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Hidden/AkilliMum/SRP/D2WeatherEffects/Post/D2RainsFast" {

HLSLINCLUDE

        #include "Packages/com.unity.postprocessing/PostProcessing/Shaders/StdLib.hlsl"

        TEXTURE2D_SAMPLER2D(_MainTex, sampler_MainTex);
        TEXTURE2D_SAMPLER2D(_NoiseTex, sampler_NoiseTex);
        //float _Blend;
        float _CameraSpeedMultiplier = 1.0;
        float _UVChangeX = 1.0;
        float _UVChangeY = 1.0;
        float _Density = 1.2;
        float _Speed = 0.1;
        float _Exposure = 5;
        float _Direction = -1.1;
        half4 _Color = (1, 1, 1, 1);
        
        
        float4 Frag(VaryingsDefault i) : SV_Target
        {
            //float4 color = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.texcoord);
            //float luminance = dot(color.rgb, float3(0.2126729, 0.7151522, 0.0721750));
            //color.rgb = lerp(color.rgb, luminance.xxx, _Blend.xxx);
            //return color;
            float2 fogUV = float2 (i.texcoord.x + _UVChangeX*_CameraSpeedMultiplier, i.texcoord.y + _UVChangeY*_CameraSpeedMultiplier);
            float2 st =  fogUV * float2(.5, .01)+float2(_Time.y*_Speed+fogUV.y*_Direction, _Time.y*_Speed);
            float r = SAMPLE_TEXTURE2D(_NoiseTex, sampler_NoiseTex, st).y 
                * SAMPLE_TEXTURE2D(_NoiseTex, sampler_NoiseTex, st*.773).x 
                * _Density;
            r = clamp(pow(abs(r), 23.0) * 13.0, 0.0, fogUV.y*.14);
            r *= _Exposure;
            float4 tex = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.texcoord);
            return tex*(1-r*_Color.a)+r*_Color*_Color.a;
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
