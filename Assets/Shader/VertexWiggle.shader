Shader "Custom/VertexWiggle"
{
    Properties
    {
        [MainColor] _BaseColor("Base Color", Color) = (1, 1, 1, 1)
        [MainTexture] _BaseMap("Base Map", 2D) = "white" {}
        
        _WindStrength("Wind Strength", Range(0,1)) = 0.2
        _WindSpeed("Wind Speed", Range(0,10)) = 2.0
        _WindFrequency("Wind Frequency", Range(0,5)) = 0.7
        _LightPosition("Light Position", Vector) = (0,0,0,0)
        _LightInfluence("Light Influence", Float) = 0.5
        _InfluenceRadius("Influence Radius", Range(0,10)) = 1.0

    }

    SubShader
    {
        Tags { "RenderType" = "Opaque" "RenderPipeline" = "UniversalPipeline" }

        Pass
        {
            HLSLPROGRAM

            #pragma vertex vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            
            struct Attributes
            {
                float4 positionOS : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
                // float2 uv : TEXCOORD0;
            };

            // TEXTURE2D(_BaseMap);
            // SAMPLER(sampler_BaseMap);

            CBUFFER_START(UnityPerMaterial)
                half4 _BaseColor;
                float4 _BaseMap_ST;
                float _WindStrength;
                float _WindSpeed;
                float _WindFrequency;
                float4 _LightPosition;
                float _LightInfluence;
                float _InfluenceRadius;
            CBUFFER_END

            Varyings vert(Attributes IN)
            {
                Varyings OUT;
                float3 position = IN.positionOS.xyz;
                float3 positionWS = TransformObjectToWorld(position.xyz);

                
                float3 bladeOriginWS =
                    TransformObjectToWorld(float3(0.0, 0.0, 0.0));



                float3 lightDelta =
                    _LightPosition.xyz - bladeOriginWS;

                float distanceToLight =
                    length(lightDelta);

                float3 lightDirection =
                    normalize(lightDelta);

                float influence =
                    saturate(
                        1.0 -
                        distanceToLight / _InfluenceRadius
                    );

                influence *= influence;

                float bendFactor = pow(saturate(position.y), 2.0);
                float random =
                    frac(
                        sin(
                            dot(
                                positionWS.xz,
                                float2(12.9898, 78.233)
                            )
                        ) * 43758.5453
                    );
                float strengthVariation =
                    lerp(0.7, 1.3, random);
                float wave =
                    sin(
                        positionWS.x * _WindFrequency +
                        positionWS.z * (_WindFrequency * 0.7) +
                        _Time.y * _WindSpeed
                    );


                float2 windDir =
                    normalize(float2(1.0, 0.4));

                position.x +=
                    bendFactor *
                    ((_LightInfluence *
                    lightDirection.x *
                    influence)
                    +
                    (windDir.x *
                    wave *
                    _WindStrength * 
                    strengthVariation));

                position.z +=
                    bendFactor *
                    ((_LightInfluence *
                    lightDirection.z *
                    influence)
                    +
                    (windDir.y *
                    wave *
                    _WindStrength * 
                    strengthVariation));

                position.y -= abs(wave) * bendFactor * 0.05;


                OUT.positionHCS = TransformObjectToHClip(position);

                // OUT.positionHCS = TransformObjectToHClip(IN.positionOS.xyz);
                // OUT.uv = TRANSFORM_TEX(IN.uv, _BaseMap);
                return OUT;
            }

            half4 frag(Varyings IN) : SV_Target
            {
                // half4 color = SAMPLE_TEXTURE2D(_BaseMap, sampler_BaseMap, IN.uv) * _BaseColor;
                // return color;
                return half4(0,1,0,1);
            }
            ENDHLSL
        }
    }
}
