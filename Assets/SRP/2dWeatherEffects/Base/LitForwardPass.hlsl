#if UNITY_VERSION >= 201930
  #ifndef UNIVERSAL_FORWARD_LIT_PASS_INCLUDED
  #define UNIVERSAL_FORWARD_LIT_PASS_INCLUDED
  #endif

  #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
#else
  #ifndef LIGHTWEIGHT_FORWARD_LIT_PASS_INCLUDED
  #define LIGHTWEIGHT_FORWARD_LIT_PASS_INCLUDED
  #endif

  #include "Packages/com.unity.render-pipelines.lightweight/ShaderLibrary/Lighting.hlsl"
#endif
    
#if UNITY_VERSION < 201900
  #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/UnityInstancing.hlsl"
#endif

struct Attributes
{
    float4 positionOS   : POSITION;
    float3 normalOS     : NORMAL;
    float4 tangentOS    : TANGENT;
    float2 texcoord     : TEXCOORD0;
    float2 lightmapUV   : TEXCOORD1;
    UNITY_VERTEX_INPUT_INSTANCE_ID
};

struct Varyings
{
    float2 uv                       : TEXCOORD0;
    DECLARE_LIGHTMAP_OR_SH(lightmapUV, vertexSH, 1);

#ifdef _ADDITIONAL_LIGHTS
    float3 positionWS               : TEXCOORD2;
#endif

#ifdef _NORMALMAP
    half4 normalWS                  : TEXCOORD3;    // xyz: normal, w: viewDir.x
    half4 tangentWS                 : TEXCOORD4;    // xyz: tangent, w: viewDir.y
    half4 bitangentWS               : TEXCOORD5;    // xyz: bitangent, w: viewDir.z
#else
    half3 normalWS                  : TEXCOORD3;
    half3 viewDirWS                 : TEXCOORD4;
#endif

    half4 fogFactorAndVertexLight   : TEXCOORD6; // x: fogFactor, yzw: vertex light

#ifdef _MAIN_LIGHT_SHADOWS
    float4 shadowCoord              : TEXCOORD7;
#endif
    float4 screenPos                : TEXCOORD8;
    float distance                  : TEXCOORD9;
    float3 worldPos                 : TEXCOORD10;
    float3 viewDir                  : TEXCOORD11;
    float3 eyePos                   : TEXCOORD12;

    float4 positionCS               : SV_POSITION;
    UNITY_VERTEX_INPUT_INSTANCE_ID
    UNITY_VERTEX_OUTPUT_STEREO
};

void InitializeInputData(Varyings input, half3 normalTS, out InputData inputData)
{
    inputData = (InputData)0;

#ifdef _ADDITIONAL_LIGHTS
    inputData.positionWS = input.positionWS;
#endif

#ifdef _NORMALMAP
    half3 viewDirWS = half3(input.normalWS.w, input.tangentWS.w, input.bitangentWS.w);
    inputData.normalWS = TransformTangentToWorld(normalTS,
        half3x3(input.tangentWS.xyz, input.bitangentWS.xyz, input.normalWS.xyz));
#else
    half3 viewDirWS = input.viewDirWS;
    inputData.normalWS = input.normalWS;
#endif

    inputData.normalWS = NormalizeNormalPerPixel(inputData.normalWS);

//#if SHADER_HINT_NICE_QUALITY
    viewDirWS = SafeNormalize(viewDirWS);
//#endif

    inputData.viewDirectionWS = viewDirWS;
#if defined(_MAIN_LIGHT_SHADOWS) && !defined(_RECEIVE_SHADOWS_OFF)
    inputData.shadowCoord = input.shadowCoord;
#else
    inputData.shadowCoord = float4(0, 0, 0, 0);
#endif
    inputData.fogCoord = input.fogFactorAndVertexLight.x;
    inputData.vertexLighting = input.fogFactorAndVertexLight.yzw;
    inputData.bakedGI = SAMPLE_GI(input.lightmapUV, input.vertexSH, inputData.normalWS);
}

///////////////////////////////////////////////////////////////////////////////
//                  Vertex and Fragment functions                            //
///////////////////////////////////////////////////////////////////////////////

// Used in Standard (Physically Based) shader
Varyings LitPassVertex(Attributes input)
{
    Varyings output = (Varyings)0;

    UNITY_SETUP_INSTANCE_ID(input);
#if UNITY_VERSION >= 201930
    UNITY_TRANSFER_INSTANCE_ID(input, output);
#endif
    UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(output);

    VertexPositionInputs vertexInput = GetVertexPositionInputs(input.positionOS.xyz);
    output.screenPos = ComputeScreenPos(vertexInput.positionCS);
    //output.worldPos = mul(unity_ObjectToWorld, vertexInput.positionCS);
    //output.distance = distance(_WorldSpaceCameraPos, mul(unity_ObjectToWorld, input.positionOS));
    VertexNormalInputs normalInput = GetVertexNormalInputs(input.normalOS, input.tangentOS);
    half3 viewDirWS = GetCameraPositionWS() - vertexInput.positionWS;
    
    //#if !SHADER_HINT_NICE_QUALITY
    //viewDirWS = SafeNormalize(viewDirWS);
    //#endif

    half3 vertexLight = VertexLighting(vertexInput.positionWS, normalInput.normalWS);
    half fogFactor = ComputeFogFactor(vertexInput.positionCS.z);
    
    #if UNITY_VERSION >= 201900
    output.uv = TRANSFORM_TEX(input.texcoord, _BaseMap);
    #else
    output.uv = TRANSFORM_TEX(input.texcoord, _MainTex);
    #endif

#ifdef _NORMALMAP
    output.normalWS = half4(normalInput.normalWS, viewDirWS.x);
    output.tangentWS = half4(normalInput.tangentWS, viewDirWS.y);
    output.bitangentWS = half4(normalInput.bitangentWS, viewDirWS.z);
#else
    
    #if UNITY_VERSION >= 201900
    output.normalWS = NormalizeNormalPerVertex(normalInput.normalWS);
    #else
    output.normalWS = normalInput.normalWS;
    #endif
    
    output.viewDirWS = viewDirWS;
#endif
    
    OUTPUT_LIGHTMAP_UV(input.lightmapUV, unity_LightmapST, output.lightmapUV);
    OUTPUT_SH(output.normalWS.xyz, output.vertexSH);

    output.fogFactorAndVertexLight = half4(fogFactor, vertexLight);

#ifdef _ADDITIONAL_LIGHTS
    output.positionWS = vertexInput.positionWS;
#endif

#if defined(_MAIN_LIGHT_SHADOWS) && !defined(_RECEIVE_SHADOWS_OFF)
    output.shadowCoord = GetShadowCoord(vertexInput);
#endif

    output.positionCS = vertexInput.positionCS;
    
    //output.viewDir = viewDirWS;
    
    //output.eyePos = mul(UNITY_MATRIX_MV, vertexInput.positionCS);

    return output;
}

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

float hash(float n) 
{ 
    return frac(sin(n)*753.5453123); 
}

float noise(in float3 x)
{
    float3 p = floor(x);
    float3 f = frac(x);
    f = f*f*(3.0 - 2.0*f);

    float n = p.x + p.y*157.0 + 113.0*p.z;
    return lerp(
                lerp(
                    lerp(hash(n + 0.0),   hash(n + 1.0),   f.x),
                    lerp(hash(n + 157.0), hash(n + 158.0), f.x), 
                    f.y),
                lerp(
                    lerp(hash(n + 113.0), hash(n + 114.0), f.x),
                    lerp(hash(n + 270.0), hash(n + 271.0), f.x), 
                    f.y),
                f.z);
}

float fog(in float2 uv)
{
    float direction = _Time.y * _Speed;
    float Vdirection = _Time.y * _VSpeed;
    float color = 0.0;
    float total = 0.0;
    float k = 0.0;

    for (float i=0; i<6; i++)
    {
        k = pow(2.0, i); 
        color += noise(float3((uv.x * _Size + direction * (i+1.0)*0.2) * k, 
                        (uv.y * _Size + Vdirection * (i + 1.0)*0.2) * k,
                        0.0)) 
                        / k; 
        total += 1.0/k;
    }
    color /= total;
    
    return clamp(color, 0.0, 1.0);

}

// Used in Standard (Physically Based) shader
half4 LitPassFragment(Varyings input) : SV_Target
{
#if UNITY_VERSION >= 201930
    UNITY_SETUP_INSTANCE_ID(input);
#endif
    UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input);

    //calculate surface data (normals etc.)
    SurfaceData surfaceData;
    InitializeStandardLitSurfaceData(input.uv, surfaceData);

    //wetness
    float2 screenUV = (input.screenPos.xy) / (input.screenPos.w+FLT_MIN);

    float eyeIndex = 0;
    #ifdef UNITY_SINGLE_PASS_STEREO
       eyeIndex = unity_StereoEyeIndex;
    #else
    //When not using single pass stereo rendering, eye index must be determined by testing the
    //sign of the horizontal skew of the projection matrix.
    if (unity_CameraProjection[0][2] > 0) {
       eyeIndex = 1.0;
    } else {
       eyeIndex = 0.0;
    }
    #endif

    //#if UNITY_SINGLE_PASS_STEREO  //!!LWRP does not need that, i suppose it already corrects it with UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input);
        //If Single-Pass Stereo mode is active, transform the
        //coordinates to get the correct output UV for the current eye.
       //float4 scaleOffset = unity_StereoScaleOffset[unity_StereoEyeIndex];
       //screenUV = (screenUV - scaleOffset.zw) / scaleOffset.xy;
    //#endif
    
    //#if UNITY_VERSION >= 201900
    //half3 nor = SampleNormal(input.uv, TEXTURE2D_ARGS(_RefractionTex, sampler_RefractionTex), _BumpScale);
    //#else
    //half3 nor = SampleNormal(input.uv, TEXTURE2D_PARAM(_RefractionTex, sampler_RefractionTex), _BumpScale);
    //#endif
    //SAMPLE_TEXTURE2D_LOD(_ReflectionTexOther, sampler_ReflectionTexOther, screenUV1,_LODLevel).rgb / 2 +
    //SAMPLE_TEXTURE2D(_ReflectionTexOther, sampler_ReflectionTexOther, screenUV1).rgb / 2 +
        
    //calculate output
    InputData inputData;
    InitializeInputData(input, surfaceData.normalTS, inputData);
    
    //if(_Surface != 1){ //manipulate if surface is not transparent
    //    if(_EnableMask>0){
    //        surfaceData.albedo = surfaceData.albedo * (1-mask.a) +
    //            surfaceData.albedo * mask.a * (1-_ReflectionIntensity);
    //            
    //        surfaceData.emission = reflection * _ReflectionIntensity * pow(mask.a,_MaskEdgeDarkness);
    //    }
    //    else{
    //        surfaceData.albedo   = (1-_ReflectionIntensity) * surfaceData.albedo;
    //        surfaceData.emission = _ReflectionIntensity     * reflection;
    //    }
    //}

    half4 color = LightweightFragmentPBR(inputData, surfaceData.albedo, surfaceData.metallic, 
                surfaceData.specular, surfaceData.smoothness, surfaceData.occlusion, surfaceData.emission, surfaceData.alpha);
    
    //float2 fogUV = float2 (input.uv.x + _UVChangeX*_CameraSpeedMultiplier, input.uv.y + _UVChangeY*_CameraSpeedMultiplier);
    float2 fogUV = float2 (screenUV.x + _UVChangeX*_CameraSpeedMultiplier, screenUV.y + _UVChangeY*_CameraSpeedMultiplier);
    float f = fog(fogUV);
    //float m = min(f*_Density, 1.);
    float m = f*_Density*_Color.a; //user can fade-in/out with this
    
    float top =    _TopFade    > 0 ? (1-input.uv.y*_TopFade)   : 1;
    float right =  _RightFade  > 0 ? (1-input.uv.x*_RightFade) : 1;
    float bottom = _BottomFade > 0 ?    input.uv.y*_BottomFade : 1;
    float left =   _LeftFade   > 0 ?    input.uv.x*_LeftFade   : 1;
    
    color = top * right * bottom * left * m * color;
        
    // if(_Surface != 1 && _ReflectionIntensity == 1)
    // { //if Surface is not transparent and full mirror just return full reflection    
    //     if(_EnableMask>0)
    //     {
    //         color.rgb = color.rgb * (1-mask.a) +
    //             reflection * mask.a;
    //     }
    //     else
    //     {
    //         color.rgb = reflection;
    //     }
    // }

    //if(_Surface == 1){ //transparent mix of reflection for glass like objects
    //    color = color*(1-_ReflectionIntensity+surfaceData.alpha) + 
    //            half4(reflection,1)*(_ReflectionIntensity-surfaceData.alpha);
    //}
    
    color.rgb = MixFog(color.rgb, inputData.fogCoord);
    
    return color;
}
