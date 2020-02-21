#if UNITY_VERSION >= 201930
  #ifndef UNIVERSAL_FALLBACK_2D_INCLUDED
  #define UNIVERSAL_FALLBACK_2D_INCLUDED
  #endif
#else
  #ifndef LIGHTWEIGHT_FALLBACK_2D_INCLUDED
  #define LIGHTWEIGHT_FALLBACK_2D_INCLUDED
  #endif
#endif

struct Attributes
{
    float4 positionOS       : POSITION;
    float2 uv               : TEXCOORD0;
};

struct Varyings
{
    float2 uv        : TEXCOORD0;
    float4 vertex : SV_POSITION;
};

Varyings vert(Attributes input)
{
    Varyings output = (Varyings)0;

    VertexPositionInputs vertexInput = GetVertexPositionInputs(input.positionOS.xyz);
    output.vertex = vertexInput.positionCS;
#if UNITY_VERSION >= 201900
    output.uv = TRANSFORM_TEX(input.uv, _BaseMap);
#else
    output.uv = TRANSFORM_TEX(input.uv, _MainTex);
#endif
    

    return output;
}

half4 frag(Varyings input) : SV_Target
{
    half2 uv = input.uv;
#if UNITY_VERSION >= 201900
    half4 texColor = SAMPLE_TEXTURE2D(_BaseMap, sampler_BaseMap, uv);
    half3 color = texColor.rgb * _BaseColor.rgb;
    half alpha = texColor.a * _BaseColor.a;
#else
    half4 texColor = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, uv);
    half3 color = texColor.rgb * _Color.rgb;
    half alpha = texColor.a * _Color.a;
#endif
    AlphaDiscard(alpha, _Cutoff);

#ifdef _ALPHAPREMULTIPLY_ON
    color *= alpha;
#endif
    return half4(color, alpha);
}

