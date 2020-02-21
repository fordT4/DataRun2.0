#if UNITY_VERSION >= 201930
  #ifndef UNIVERSAL_SHADOW_CASTER_PASS_INCLUDED
  #define UNIVERSAL_SHADOW_CASTER_PASS_INCLUDED
  #endif

  #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
  #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Shadows.hlsl"
#else
  #ifndef LIGHTWEIGHT_SHADOW_CASTER_PASS_INCLUDED
  #define LIGHTWEIGHT_SHADOW_CASTER_PASS_INCLUDED
  #endif

  #include "Packages/com.unity.render-pipelines.lightweight/ShaderLibrary/Core.hlsl"

  #if UNITY_VERSION >= 201900
    #include "Packages/com.unity.render-pipelines.lightweight/ShaderLibrary/Shadows.hlsl"
  #endif
#endif

#if UNITY_VERSION < 201900
float4 _ShadowBias; // x: depth bias, y: normal bias
#endif

float3 _LightDirection;

struct Attributes
{
    float4 positionOS   : POSITION;
    float3 normalOS     : NORMAL;
    float2 texcoord     : TEXCOORD0;
    UNITY_VERTEX_INPUT_INSTANCE_ID
};

struct Varyings
{
    float2 uv           : TEXCOORD0;
    float4 positionCS   : SV_POSITION;
};

float4 GetShadowPositionHClip(Attributes input)
{
    #if UNITY_VERSION >= 201900
      float3 positionWS = TransformObjectToWorld(input.positionOS.xyz);
      float3 normalWS = TransformObjectToWorldNormal(input.normalOS);

      float4 positionCS = TransformWorldToHClip(ApplyShadowBias(positionWS, normalWS, _LightDirection));
    #else
      float3 positionWS = TransformObjectToWorld(input.positionOS.xyz);
      float3 normalWS = TransformObjectToWorldDir(input.normalOS);

      float invNdotL = 1.0 - saturate(dot(_LightDirection, normalWS));
      float scale = invNdotL * _ShadowBias.y;

      // normal bias is negative since we want to apply an inset normal offset
      positionWS = _LightDirection * _ShadowBias.xxx + positionWS;
      positionWS = normalWS * scale.xxx + positionWS;
      float4 positionCS = TransformWorldToHClip(positionWS);
    #endif
    

#if UNITY_REVERSED_Z
    positionCS.z = min(positionCS.z, positionCS.w * UNITY_NEAR_CLIP_VALUE);
#else
    positionCS.z = max(positionCS.z, positionCS.w * UNITY_NEAR_CLIP_VALUE);
#endif

    return positionCS;
}

Varyings ShadowPassVertex(Attributes input)
{
    Varyings output;
    UNITY_SETUP_INSTANCE_ID(input);
    
    #if UNITY_VERSION >= 201900
      output.uv = TRANSFORM_TEX(input.texcoord, _BaseMap);
    #else
      output.uv = TRANSFORM_TEX(input.texcoord, _MainTex);
    #endif
    
    output.positionCS = GetShadowPositionHClip(input);
    return output;
}

half4 ShadowPassFragment(Varyings input) : SV_TARGET
{
    #if UNITY_VERSION >= 201900
      Alpha(SampleAlbedoAlpha(input.uv, TEXTURE2D_ARGS(_BaseMap, sampler_BaseMap)).a, _BaseColor, _Cutoff);
    #else
      Alpha(SampleAlbedoAlpha(input.uv, TEXTURE2D_PARAM(_MainTex, sampler_MainTex)).a, _Color, _Cutoff);
    #endif
    
    return 0;
}

