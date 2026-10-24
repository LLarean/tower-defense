Shader "Game/Crystal"
{
    Properties
    {
        [Header(Translucency)]
        _Translucency("Strength", Range(0, 50)) = 1
        _TransNormalDistortion("Normal Distortion", Range(0, 1)) = 0.1
        _TransScattering("Scattering Falloff", Range(1, 50)) = 2
        _TransDirect("Direct", Range(0, 1)) = 1
        _TransAmbient("Ambient", Range(0, 1)) = 0.2
        _TransShadow("Shadow", Range(0, 1)) = 0.9
        _TranslucencyMask("Translucency Mask", 2D) = "white" {}
        _TranslucencyMaskPower("Translucency Mask Power", Range(0, 1)) = 1
        [Header(Surface)]
        _AlbedoMask("Albedo Mask", 2D) = "white" {}
        _ColorTint1("Color Tint 1", Color) = (1, 1, 1, 1)
        _ColorTint2("Color Tint 2", Color) = (1, 1, 1, 1)
        [Normal] _Normal("Normal", 2D) = "bump" {}
        _MetallicSmoothness("Metallic Smoothness", 2D) = "white" {}
        _Metallic("Metallic", Range(0, 1)) = 1
        _Smoothness("Smoothness", Range(0, 1)) = 1
        [Header(Emission)]
        _Emission("Emission", 2D) = "white" {}
        [HDR] _EmissionColor("Emission Color", Color) = (1, 1, 1, 1)
        _EmissionPower("Emission Power", Range(0, 10)) = 2
        [Toggle(_RAMPENABLED_ON)] _RampEnabled("Ramp Enabled", Int) = 0
        [Toggle(_RAMPINVERTED_ON)] _RampInverted("Ramp Inverted", Int) = 0
        _Ramp("Ramp", 2D) = "white" {}
        _RampMask("Ramp Mask", 2D) = "white" {}
    }

    SubShader
    {
        Tags { "RenderType" = "Opaque" "RenderPipeline" = "UniversalPipeline" "Queue" = "Geometry" }

        HLSLINCLUDE
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

        CBUFFER_START(UnityPerMaterial)
            float4 _TranslucencyMask_ST;
            float4 _AlbedoMask_ST;
            float4 _Normal_ST;
            float4 _MetallicSmoothness_ST;
            float4 _Emission_ST;
            float4 _RampMask_ST;
            float4 _Ramp_ST;
            half4 _ColorTint1;
            half4 _ColorTint2;
            half4 _EmissionColor;
            half _Translucency;
            half _TransNormalDistortion;
            half _TransScattering;
            half _TransDirect;
            half _TransAmbient;
            half _TransShadow;
            half _TranslucencyMaskPower;
            half _Metallic;
            half _Smoothness;
            half _EmissionPower;
            half _RampEnabled;
            half _RampInverted;
        CBUFFER_END
        ENDHLSL

        Pass
        {
            Name "ForwardLit"
            Tags { "LightMode" = "UniversalForward" }

            HLSLPROGRAM
            #pragma target 2.0
            #pragma vertex Vertex
            #pragma fragment Fragment
            #pragma shader_feature_local_fragment _RAMPENABLED_ON
            #pragma shader_feature_local_fragment _RAMPINVERTED_ON
            #pragma multi_compile _ _MAIN_LIGHT_SHADOWS _MAIN_LIGHT_SHADOWS_CASCADE _MAIN_LIGHT_SHADOWS_SCREEN
            #pragma multi_compile _ _ADDITIONAL_LIGHTS_VERTEX _ADDITIONAL_LIGHTS
            #pragma multi_compile_fragment _ _ADDITIONAL_LIGHT_SHADOWS
            #pragma multi_compile_fragment _ _SHADOWS_SOFT _SHADOWS_SOFT_LOW _SHADOWS_SOFT_MEDIUM _SHADOWS_SOFT_HIGH
            #pragma multi_compile_fragment _ _SCREEN_SPACE_OCCLUSION
            #pragma multi_compile _ _CLUSTER_LIGHT_LOOP
            #pragma multi_compile_fog
            #pragma multi_compile_instancing

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"

            TEXTURE2D(_TranslucencyMask); SAMPLER(sampler_TranslucencyMask);
            TEXTURE2D(_AlbedoMask); SAMPLER(sampler_AlbedoMask);
            TEXTURE2D(_Normal); SAMPLER(sampler_Normal);
            TEXTURE2D(_MetallicSmoothness); SAMPLER(sampler_MetallicSmoothness);
            TEXTURE2D(_Emission); SAMPLER(sampler_Emission);
            TEXTURE2D(_Ramp); SAMPLER(sampler_Ramp);
            TEXTURE2D(_RampMask); SAMPLER(sampler_RampMask);

            struct Attributes
            {
                float4 positionOS : POSITION;
                float3 normalOS : NORMAL;
                float4 tangentOS : TANGENT;
                float2 uv : TEXCOORD0;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct Varyings
            {
                float4 positionCS : SV_POSITION;
                float2 uv : TEXCOORD0;
                float3 positionWS : TEXCOORD1;
                half3 normalWS : TEXCOORD2;
                half4 tangentWS : TEXCOORD3;
                half fogFactor : TEXCOORD4;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            Varyings Vertex(Attributes input)
            {
                Varyings output = (Varyings)0;
                UNITY_SETUP_INSTANCE_ID(input);
                UNITY_TRANSFER_INSTANCE_ID(input, output);
                VertexPositionInputs position = GetVertexPositionInputs(input.positionOS.xyz);
                VertexNormalInputs normal = GetVertexNormalInputs(input.normalOS, input.tangentOS);
                output.positionCS = position.positionCS;
                output.positionWS = position.positionWS;
                output.normalWS = normal.normalWS;
                output.tangentWS = half4(normal.tangentWS, input.tangentOS.w * GetOddNegativeScale());
                output.uv = input.uv;
                output.fogFactor = ComputeFogFactor(position.positionCS.z);
                return output;
            }

            half3 Translucency(Light light, half3 normalWS, half3 viewWS, half3 albedo, half mask, half3 ambient, half shadowBlend)
            {
                half3 attenuation = light.color * lerp(1.0h, light.shadowAttenuation * light.distanceAttenuation, shadowBlend);
                half3 direction = light.direction + normalWS * _TransNormalDistortion;
                half scatter = pow(saturate(dot(viewWS, -direction)), _TransScattering);
                return albedo * attenuation * (scatter * _TransDirect + ambient * _TransAmbient) * mask * _Translucency;
            }

            half3 Emission(float2 uv)
            {
                half3 emission = SAMPLE_TEXTURE2D(_Emission, sampler_Emission, TRANSFORM_TEX(uv, _Emission)).rgb * _EmissionPower;
                #if defined(_RAMPENABLED_ON)
                    half ramp = SAMPLE_TEXTURE2D(_RampMask, sampler_RampMask, TRANSFORM_TEX(uv, _RampMask)).r;
                    #if defined(_RAMPINVERTED_ON)
                        ramp = 1.0h - ramp;
                    #endif
                    return emission * SAMPLE_TEXTURE2D(_Ramp, sampler_Ramp, float2(ramp, 0.0)).rgb;
                #else
                    return emission * _EmissionColor.rgb;
                #endif
            }

            half4 Fragment(Varyings input) : SV_Target
            {
                UNITY_SETUP_INSTANCE_ID(input);
                float2 uv = input.uv;
                half3 normalTS = UnpackNormal(SAMPLE_TEXTURE2D(_Normal, sampler_Normal, TRANSFORM_TEX(uv, _Normal)));
                half3 bitangent = input.tangentWS.w * cross(input.normalWS, input.tangentWS.xyz);
                half3 normalWS = NormalizeNormalPerPixel(TransformTangentToWorld(normalTS, half3x3(input.tangentWS.xyz, bitangent, input.normalWS)));
                half4 metallicSmoothness = SAMPLE_TEXTURE2D(_MetallicSmoothness, sampler_MetallicSmoothness, TRANSFORM_TEX(uv, _MetallicSmoothness));
                half albedoMask = SAMPLE_TEXTURE2D(_AlbedoMask, sampler_AlbedoMask, TRANSFORM_TEX(uv, _AlbedoMask)).r;
                half mask = SAMPLE_TEXTURE2D(_TranslucencyMask, sampler_TranslucencyMask, TRANSFORM_TEX(uv, _TranslucencyMask)).r * _TranslucencyMaskPower;

                InputData inputData = (InputData)0;
                inputData.positionWS = input.positionWS;
                inputData.positionCS = input.positionCS;
                inputData.normalWS = normalWS;
                inputData.viewDirectionWS = GetWorldSpaceNormalizeViewDir(input.positionWS);
                inputData.shadowCoord = TransformWorldToShadowCoord(input.positionWS);
                inputData.fogCoord = input.fogFactor;
                inputData.bakedGI = SampleSH(normalWS);
                inputData.normalizedScreenSpaceUV = GetNormalizedScreenSpaceUV(input.positionCS);
                inputData.shadowMask = half4(1, 1, 1, 1);

                SurfaceData surface = (SurfaceData)0;
                surface.albedo = lerp(_ColorTint1.rgb, _ColorTint2.rgb, albedoMask);
                surface.metallic = metallicSmoothness.r * _Metallic;
                surface.smoothness = metallicSmoothness.a * _Smoothness;
                surface.normalTS = normalTS;
                surface.emission = Emission(uv);
                surface.occlusion = 1.0h;
                surface.alpha = 1.0h;

                half4 color = UniversalFragmentPBR(inputData, surface);

                Light mainLight = GetMainLight(inputData.shadowCoord, inputData.positionWS, inputData.shadowMask);
                color.rgb += Translucency(mainLight, normalWS, inputData.viewDirectionWS, surface.albedo, mask, inputData.bakedGI, _TransShadow);

                #if defined(_ADDITIONAL_LIGHTS)
                    uint count = GetAdditionalLightsCount();
                    #if USE_CLUSTER_LIGHT_LOOP
                        UNITY_LOOP for (uint index = 0; index < min(URP_FP_DIRECTIONAL_LIGHTS_COUNT, MAX_VISIBLE_LIGHTS); index++)
                        {
                            Light light = GetAdditionalLight(index, inputData.positionWS, inputData.shadowMask);
                            color.rgb += Translucency(light, normalWS, inputData.viewDirectionWS, surface.albedo, mask, 0, 1.0h);
                        }
                    #endif
                    LIGHT_LOOP_BEGIN(count)
                        Light light = GetAdditionalLight(lightIndex, inputData.positionWS, inputData.shadowMask);
                        color.rgb += Translucency(light, normalWS, inputData.viewDirectionWS, surface.albedo, mask, 0, 1.0h);
                    LIGHT_LOOP_END
                #endif

                color.rgb = MixFog(color.rgb, inputData.fogCoord);
                return half4(color.rgb, 1.0h);
            }
            ENDHLSL
        }

        Pass
        {
            Name "ShadowCaster"
            Tags { "LightMode" = "ShadowCaster" }
            ZWrite On
            ZTest LEqual
            ColorMask 0

            HLSLPROGRAM
            #pragma target 2.0
            #pragma vertex Vertex
            #pragma fragment Fragment
            #pragma multi_compile_instancing
            #pragma multi_compile_vertex _ _CASTING_PUNCTUAL_LIGHT_SHADOW

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Shadows.hlsl"

            float3 _LightDirection;
            float3 _LightPosition;

            struct Attributes
            {
                float4 positionOS : POSITION;
                float3 normalOS : NORMAL;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            float4 Vertex(Attributes input) : SV_POSITION
            {
                UNITY_SETUP_INSTANCE_ID(input);
                float3 positionWS = TransformObjectToWorld(input.positionOS.xyz);
                float3 normalWS = TransformObjectToWorldNormal(input.normalOS);
                #if _CASTING_PUNCTUAL_LIGHT_SHADOW
                    float3 lightDirection = normalize(_LightPosition - positionWS);
                #else
                    float3 lightDirection = _LightDirection;
                #endif
                float4 positionCS = TransformWorldToHClip(ApplyShadowBias(positionWS, normalWS, lightDirection));
                return ApplyShadowClamping(positionCS);
            }

            half4 Fragment() : SV_Target
            {
                return 0;
            }
            ENDHLSL
        }

        Pass
        {
            Name "DepthOnly"
            Tags { "LightMode" = "DepthOnly" }
            ZWrite On
            ColorMask R

            HLSLPROGRAM
            #pragma target 2.0
            #pragma vertex Vertex
            #pragma fragment Fragment
            #pragma multi_compile_instancing

            struct Attributes
            {
                float4 positionOS : POSITION;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            float4 Vertex(Attributes input) : SV_POSITION
            {
                UNITY_SETUP_INSTANCE_ID(input);
                return TransformObjectToHClip(input.positionOS.xyz);
            }

            half Fragment() : SV_Target
            {
                return 0;
            }
            ENDHLSL
        }

        Pass
        {
            Name "DepthNormals"
            Tags { "LightMode" = "DepthNormals" }
            ZWrite On

            HLSLPROGRAM
            #pragma target 2.0
            #pragma vertex Vertex
            #pragma fragment Fragment
            #pragma multi_compile_instancing

            struct Attributes
            {
                float4 positionOS : POSITION;
                float3 normalOS : NORMAL;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct Varyings
            {
                float4 positionCS : SV_POSITION;
                half3 normalWS : TEXCOORD0;
            };

            Varyings Vertex(Attributes input)
            {
                UNITY_SETUP_INSTANCE_ID(input);
                Varyings output;
                output.positionCS = TransformObjectToHClip(input.positionOS.xyz);
                output.normalWS = TransformObjectToWorldNormal(input.normalOS);
                return output;
            }

            half4 Fragment(Varyings input) : SV_Target
            {
                return half4(NormalizeNormalPerPixel(input.normalWS), 0.0);
            }
            ENDHLSL
        }
    }
}
