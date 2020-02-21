#if UNITY_VERSION >= 201930
  #ifndef UNIVERSAL_LIT_META_PASS_INCLUDED
  #define UNIVERSAL_LIT_META_PASS_INCLUDED
  #endif

  #include "../Base/MetaInput.hlsl"
#else
  #ifndef LIGHTWEIGHT_LIT_META_PASS_INCLUDED
  #define LIGHTWEIGHT_LIT_META_PASS_INCLUDED
  #endif

  #include "../Base/MetaInput.hlsl"
#endif

#if UNITY_VERSION >= 201930
  Varyings UniversalVertexMeta(Attributes input)
  {
      Varyings output;
      output.positionCS = MetaVertexPosition(input.positionOS, input.uv1, input.uv2,
          unity_LightmapST, unity_DynamicLightmapST);
      output.uv = TRANSFORM_TEX(input.uv0, _BaseMap);
      return output;
  }
#else
  #if UNITY_VERSION >= 201900
  Varyings LightweightVertexMeta(Attributes input)
  {
      Varyings output;
    
      output.positionCS = MetaVertexPosition(input.positionOS, input.uvLM, input.uvDLM, unity_LightmapST);
      output.uv = TRANSFORM_TEX(input.uv, _BaseMap);
      return output;
  }
  #endif
#endif

//will be called above 201930
half4 UniversalFragmentMeta(Varyings input) : SV_Target
{
    SurfaceData surfaceData;
    InitializeStandardLitSurfaceData(input.uv, surfaceData);

    BRDFData brdfData;
    InitializeBRDFData(surfaceData.albedo, surfaceData.metallic, surfaceData.specular, surfaceData.smoothness, surfaceData.alpha, brdfData);

    MetaInput metaInput;
    metaInput.Albedo = brdfData.diffuse + brdfData.specular * brdfData.roughness * 0.5;
    metaInput.SpecularColor = surfaceData.specular;
    metaInput.Emission = surfaceData.emission;

    return MetaFragment(metaInput);
}

//will be called under 201930
half4 LightweightFragmentMeta(Varyings input) : SV_Target
{
    SurfaceData surfaceData;
    InitializeStandardLitSurfaceData(input.uv, surfaceData);

    BRDFData brdfData;
    InitializeBRDFData(surfaceData.albedo, surfaceData.metallic, surfaceData.specular, surfaceData.smoothness, surfaceData.alpha, brdfData);

    MetaInput metaInput;
    metaInput.Albedo = brdfData.diffuse + brdfData.specular * brdfData.roughness * 0.5;
    metaInput.SpecularColor = surfaceData.specular;
    metaInput.Emission = surfaceData.emission;

    return MetaFragment(metaInput);
}

#if UNITY_VERSION >= 201930
//LWRP -> Universal Backwards Compatibility
Varyings LightweightVertexMeta(Attributes input)
{
    return UniversalVertexMeta(input);
}

half4 LightweightFragmentMeta(Varyings input) : SV_Target
{
    return UniversalFragmentMeta(input);
}
#endif