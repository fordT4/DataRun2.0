#if UNITY_VERSION >= 201930
  #ifndef UNIVERSAL_DEPTH_ONLY_PASS_INCLUDED
  #define UNIVERSAL_DEPTH_ONLY_PASS_INCLUDED
  #endif

  #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
#else
  #ifndef LIGHTWEIGHT_DEPTH_ONLY_PASS_INCLUDED
  #define LIGHTWEIGHT_DEPTH_ONLY_PASS_INCLUDED
  #endif

  #include "Packages/com.unity.render-pipelines.lightweight/ShaderLibrary/Core.hlsl"
#endif

struct Attributes
{
    float4 position     : POSITION;
    float2 texcoord     : TEXCOORD0;
    UNITY_VERTEX_INPUT_INSTANCE_ID
};

struct Varyings
{
    float2 uv           : TEXCOORD0;
    float4 positionCS   : SV_POSITION;
    UNITY_VERTEX_INPUT_INSTANCE_ID
    UNITY_VERTEX_OUTPUT_STEREO
};

Varyings DepthOnlyVertex(Attributes input)
{
    Varyings output = (Varyings)0;
    UNITY_SETUP_INSTANCE_ID(input);
    UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(output);
    
#if UNITY_VERSION >= 201900
    output.uv = TRANSFORM_TEX(input.texcoord, _BaseMap);
#else
    output.uv = TRANSFORM_TEX(input.texcoord, _MainTex);
#endif
    
    output.positionCS = TransformObjectToHClip(input.position.xyz);
    return output;
}

half4 DepthOnlyFragment(Varyings input) : SV_TARGET
{
#if UNITY_VERSION >= 201930
    UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input);
#endif

#if UNITY_VERSION >= 201900
    Alpha(SampleAlbedoAlpha(input.uv, TEXTURE2D_ARGS(_BaseMap, sampler_BaseMap)).a, _BaseColor, _Cutoff);
#else
    Alpha(SampleAlbedoAlpha(input.uv, TEXTURE2D_PARAM(_MainTex, sampler_MainTex)).a, _Color, _Cutoff);
#endif
    
    return 0;
}
