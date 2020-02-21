#if UNITY_VERSION >= 201930
  #ifndef UNIVERSAL_INPUT_SURFACE_INCLUDED
  #define UNIVERSAL_INPUT_SURFACE_INCLUDED
  #endif
  
  #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
  #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Packing.hlsl"
  #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/CommonMaterial.hlsl"
#else
  #ifndef LIGHTWEIGHT_INPUT_SURFACE_INCLUDED
  #define LIGHTWEIGHT_INPUT_SURFACE_INCLUDED
  #endif
  
  #include "Packages/com.unity.render-pipelines.lightweight/ShaderLibrary/Core.hlsl"
  #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Packing.hlsl"
  #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/CommonMaterial.hlsl"
#endif   

#if UNITY_VERSION >= 201900
TEXTURE2D(_BaseMap);            SAMPLER(sampler_BaseMap);
#else
TEXTURE2D(_MainTex);            SAMPLER(sampler_MainTex);
#endif
TEXTURE2D(_BumpMap);            SAMPLER(sampler_BumpMap);
TEXTURE2D(_EmissionMap);        SAMPLER(sampler_EmissionMap);

// Must match Lightweigth ShaderGraph master node
struct SurfaceData
{
    half3 albedo;
    half3 specular;
    half  metallic;
    half  smoothness;
    half3 normalTS;
    half3 emission;
    half  occlusion;
    half  alpha;
};

///////////////////////////////////////////////////////////////////////////////
//                      Material Property Helpers                            //
///////////////////////////////////////////////////////////////////////////////
half Alpha(half albedoAlpha, half4 color, half cutoff)
{
#if !defined(_SMOOTHNESS_TEXTURE_ALBEDO_CHANNEL_A) && !defined(_GLOSSINESS_FROM_BASE_ALPHA)
    half alpha = albedoAlpha * color.a;
#else
    half alpha = color.a;
#endif

#if defined(_ALPHATEST_ON)
    clip(alpha - cutoff);
#endif

    return alpha;
}

#if UNITY_VERSION >= 201900
half4 SampleAlbedoAlpha(float2 uv, TEXTURE2D_PARAM(albedoAlphaMap, sampler_albedoAlphaMap))
#else
half4 SampleAlbedoAlpha(float2 uv, TEXTURE2D_ARGS(albedoAlphaMap, sampler_albedoAlphaMap))
#endif
{
    return SAMPLE_TEXTURE2D(albedoAlphaMap, sampler_albedoAlphaMap, uv);
}

#if UNITY_VERSION >= 201900
half3 SampleNormal(float2 uv, TEXTURE2D_PARAM(bumpMap, sampler_bumpMap), half scale = 1.0h)
#else
half3 SampleNormal(float2 uv, TEXTURE2D_ARGS(bumpMap, sampler_bumpMap), half scale = 1.0h)
#endif
{
#ifdef _NORMALMAP
    half4 n = SAMPLE_TEXTURE2D(bumpMap, sampler_bumpMap, uv);
    #if BUMP_SCALE_NOT_SUPPORTED
        return UnpackNormal(n);
    #else
        return UnpackNormalScale(n, scale);
    #endif
#else
    return half3(0.0h, 0.0h, 1.0h);
#endif
}

#if UNITY_VERSION >= 201900
half3 SampleEmission(float2 uv, half3 emissionColor, TEXTURE2D_PARAM(emissionMap, sampler_emissionMap))
#else
half3 SampleEmission(float2 uv, half3 emissionColor, TEXTURE2D_ARGS(emissionMap, sampler_emissionMap))
#endif
{
#ifndef _EMISSION
    return 0;
#else
    return SAMPLE_TEXTURE2D(emissionMap, sampler_emissionMap, uv).rgb * emissionColor;
#endif
}
