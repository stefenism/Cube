﻿Shader "Custom/UnlitAlwaysVisable"
{
    Properties
    {
        [NoScaleOffset] _Texture("Texture", 2D) = "white" {}
_Color("Color", Vector) = (1,1,1,0)

    }
        SubShader
{
    Tags
    {
        "RenderPipeline" = "LightweightPipeline"
        "RenderType" = "Transparent"
        "Queue" = "Transparent+0"
    }
    Pass
    {
        // Material options generated by graph

        Blend SrcAlpha OneMinusSrcAlpha, One OneMinusSrcAlpha

    Cull Off

    ZTest Always

    ZWrite Off

        HLSLPROGRAM
    // Required to compile gles 2.0 with standard srp library
    #pragma prefer_hlslcc gles
    #pragma exclude_renderers d3d11_9x
    #pragma target 2.0

    // -------------------------------------
    // Lightweight Pipeline keywords
    #pragma shader_feature _SAMPLE_GI

    // -------------------------------------
    // Unity defined keywords
    #pragma multi_compile _ DIRLIGHTMAP_COMBINED
    #pragma multi_compile _ LIGHTMAP_ON
    #pragma multi_compile_fog

    //--------------------------------------
    // GPU Instancing
    #pragma multi_compile_instancing

    #pragma vertex vert
    #pragma fragment frag

    // Defines generated by graph

    // Lighting include is needed because of GI
    #include "Packages/com.unity.render-pipelines.lightweight/ShaderLibrary/Core.hlsl"
    #include "Packages/com.unity.render-pipelines.lightweight/ShaderLibrary/Lighting.hlsl"
    #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
    #include "Packages/com.unity.render-pipelines.lightweight/ShaderLibrary/ShaderGraphFunctions.hlsl"
    #include "Packages/com.unity.render-pipelines.lightweight/Shaders/UnlitInput.hlsl"

    CBUFFER_START(UnityPerMaterial)
    float3 _Color;
    CBUFFER_END

    TEXTURE2D(_Texture); SAMPLER(sampler_Texture); float4 _Texture_TexelSize;
    SAMPLER(_SampleTexture2D_8286E042_Sampler_3_Linear_Repeat);
    struct VertexDescriptionInputs
    {
        float3 ObjectSpacePosition;
    };

    struct SurfaceDescriptionInputs
    {
        half4 uv0;
    };


    void Unity_Multiply_float(float3 A, float3 B, out float3 Out)
    {
        Out = A * B;
    }

    struct VertexDescription
    {
        float3 Position;
    };

    VertexDescription PopulateVertexData(VertexDescriptionInputs IN)
    {
        VertexDescription description = (VertexDescription)0;
        description.Position = IN.ObjectSpacePosition;
        return description;
    }

    struct SurfaceDescription
    {
        float3 Color;
        float Alpha;
        float AlphaClipThreshold;
    };

    SurfaceDescription PopulateSurfaceData(SurfaceDescriptionInputs IN)
    {
        SurfaceDescription surface = (SurfaceDescription)0;
        float3 _Property_BD74A14_Out_0 = _Color;
        float4 _SampleTexture2D_8286E042_RGBA_0 = SAMPLE_TEXTURE2D(_Texture, sampler_Texture, IN.uv0.xy);
        float _SampleTexture2D_8286E042_R_4 = _SampleTexture2D_8286E042_RGBA_0.r;
        float _SampleTexture2D_8286E042_G_5 = _SampleTexture2D_8286E042_RGBA_0.g;
        float _SampleTexture2D_8286E042_B_6 = _SampleTexture2D_8286E042_RGBA_0.b;
        float _SampleTexture2D_8286E042_A_7 = _SampleTexture2D_8286E042_RGBA_0.a;
        float3 _Multiply_5C56265B_Out_2;
        Unity_Multiply_float(_Property_BD74A14_Out_0, (_SampleTexture2D_8286E042_RGBA_0.xyz), _Multiply_5C56265B_Out_2);
        surface.Color = _Multiply_5C56265B_Out_2;
        surface.Alpha = _SampleTexture2D_8286E042_A_7;
        surface.AlphaClipThreshold = 1;
        return surface;
    }

    struct GraphVertexInput
    {
        float4 vertex : POSITION;
        float3 normal : NORMAL;
        float4 tangent : TANGENT;
        float4 texcoord0 : TEXCOORD0;
        float4 texcoord1 : TEXCOORD1;
        UNITY_VERTEX_INPUT_INSTANCE_ID
    };


    struct GraphVertexOutput
    {
        float4 position : POSITION;

        // Interpolators defined by graph
        float3 WorldSpacePosition : TEXCOORD3;
        float3 WorldSpaceNormal : TEXCOORD4;
        float3 WorldSpaceTangent : TEXCOORD5;
        float3 WorldSpaceBiTangent : TEXCOORD6;
        float3 WorldSpaceViewDirection : TEXCOORD7;
        half4 uv0 : TEXCOORD8;
        half4 uv1 : TEXCOORD9;

        UNITY_VERTEX_INPUT_INSTANCE_ID
        UNITY_VERTEX_OUTPUT_STEREO
    };

    GraphVertexOutput vert(GraphVertexInput v)
    {
        GraphVertexOutput o = (GraphVertexOutput)0;
        UNITY_SETUP_INSTANCE_ID(v);
        UNITY_TRANSFER_INSTANCE_ID(v, o);
        UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);

        // Vertex transformations performed by graph
        float3 WorldSpacePosition = mul(UNITY_MATRIX_M,v.vertex).xyz;
        float3 WorldSpaceNormal = normalize(mul(v.normal,(float3x3)UNITY_MATRIX_I_M));
        float3 WorldSpaceTangent = normalize(mul((float3x3)UNITY_MATRIX_M,v.tangent.xyz));
        float3 WorldSpaceBiTangent = cross(WorldSpaceNormal, WorldSpaceTangent.xyz) * v.tangent.w;
        float3 WorldSpaceViewDirection = _WorldSpaceCameraPos.xyz - mul(GetObjectToWorldMatrix(), float4(v.vertex.xyz, 1.0)).xyz;
        float4 uv0 = v.texcoord0;
        float4 uv1 = v.texcoord1;
        float3 ObjectSpacePosition = mul(UNITY_MATRIX_I_M,float4(WorldSpacePosition,1.0)).xyz;

        VertexDescriptionInputs vdi = (VertexDescriptionInputs)0;

        // Vertex description inputs defined by graph
        vdi.ObjectSpacePosition = ObjectSpacePosition;

        VertexDescription vd = PopulateVertexData(vdi);
        v.vertex.xyz = vd.Position;

        o.position = TransformObjectToHClip(v.vertex.xyz);
        // Vertex shader outputs defined by graph
        o.WorldSpacePosition = WorldSpacePosition;
        o.WorldSpaceNormal = WorldSpaceNormal;
        o.WorldSpaceTangent = WorldSpaceTangent;
        o.WorldSpaceBiTangent = WorldSpaceBiTangent;
        o.WorldSpaceViewDirection = WorldSpaceViewDirection;
        o.uv0 = uv0;
        o.uv1 = uv1;

        return o;
    }

    half4 frag(GraphVertexOutput IN) : SV_Target
    {
        UNITY_SETUP_INSTANCE_ID(IN);
        UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(IN);

        // Pixel transformations performed by graph
        float3 WorldSpacePosition = IN.WorldSpacePosition;
        float3 WorldSpaceNormal = IN.WorldSpaceNormal;
        float3 WorldSpaceTangent = IN.WorldSpaceTangent;
        float3 WorldSpaceBiTangent = IN.WorldSpaceBiTangent;
        float3 WorldSpaceViewDirection = IN.WorldSpaceViewDirection;
        float4 uv0 = IN.uv0;
        float4 uv1 = IN.uv1;


        SurfaceDescriptionInputs surfaceInput = (SurfaceDescriptionInputs)0;
        // Surface description inputs defined by graph
        surfaceInput.uv0 = uv0;


        SurfaceDescription surf = PopulateSurfaceData(surfaceInput);
        float3 Color = float3(0.5, 0.5, 0.5);
        float Alpha = 1;
        float AlphaClipThreshold = 0;
        // Surface description remap performed by graph
        Color = surf.Color;
        Alpha = surf.Alpha;
        AlphaClipThreshold = surf.AlphaClipThreshold;


 #if _AlphaClip
        clip(Alpha - AlphaClipThreshold);
#endif
        return half4(Color, Alpha);
    }
    ENDHLSL
}
Pass
{
    Name "ShadowCaster"
    Tags{"LightMode" = "ShadowCaster"}

    ZWrite On ZTest LEqual

        // Material options generated by graph
        Cull Back

        HLSLPROGRAM
        // Required to compile gles 2.0 with standard srp library
        #pragma prefer_hlslcc gles
        #pragma exclude_renderers d3d11_9x
        #pragma target 2.0

        //--------------------------------------
        // GPU Instancing
        #pragma multi_compile_instancing

        #pragma vertex ShadowPassVertex
        #pragma fragment ShadowPassFragment

        // Defines generated by graph

        #include "Packages/com.unity.render-pipelines.lightweight/ShaderLibrary/Core.hlsl"
        #include "Packages/com.unity.render-pipelines.lightweight/ShaderLibrary/Lighting.hlsl"
        #include "Packages/com.unity.render-pipelines.lightweight/ShaderLibrary/ShaderGraphFunctions.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"

        CBUFFER_START(UnityPerMaterial)
        float3 _Color;
        CBUFFER_END

        TEXTURE2D(_Texture); SAMPLER(sampler_Texture); float4 _Texture_TexelSize;
        SAMPLER(_SampleTexture2D_8286E042_Sampler_3_Linear_Repeat);
        struct VertexDescriptionInputs
        {
            float3 ObjectSpacePosition;
        };

        struct SurfaceDescriptionInputs
        {
            half4 uv0;
        };


        struct VertexDescription
        {
            float3 Position;
        };

        VertexDescription PopulateVertexData(VertexDescriptionInputs IN)
        {
            VertexDescription description = (VertexDescription)0;
            description.Position = IN.ObjectSpacePosition;
            return description;
        }

        struct SurfaceDescription
        {
            float Alpha;
            float AlphaClipThreshold;
        };

        SurfaceDescription PopulateSurfaceData(SurfaceDescriptionInputs IN)
        {
            SurfaceDescription surface = (SurfaceDescription)0;
            float4 _SampleTexture2D_8286E042_RGBA_0 = SAMPLE_TEXTURE2D(_Texture, sampler_Texture, IN.uv0.xy);
            float _SampleTexture2D_8286E042_R_4 = _SampleTexture2D_8286E042_RGBA_0.r;
            float _SampleTexture2D_8286E042_G_5 = _SampleTexture2D_8286E042_RGBA_0.g;
            float _SampleTexture2D_8286E042_B_6 = _SampleTexture2D_8286E042_RGBA_0.b;
            float _SampleTexture2D_8286E042_A_7 = _SampleTexture2D_8286E042_RGBA_0.a;
            surface.Alpha = _SampleTexture2D_8286E042_A_7;
            surface.AlphaClipThreshold = 1;
            return surface;
        }

        struct GraphVertexInput
        {
            float4 vertex : POSITION;
            float3 normal : NORMAL;
            float4 tangent : TANGENT;
            float4 texcoord0 : TEXCOORD0;
            float4 texcoord1 : TEXCOORD1;
            UNITY_VERTEX_INPUT_INSTANCE_ID
        };


        struct VertexOutput
        {
            float2 uv           : TEXCOORD0;
            float4 clipPos      : SV_POSITION;
            // Interpolators defined by graph
            float3 WorldSpacePosition : TEXCOORD3;
            float3 WorldSpaceNormal : TEXCOORD4;
            float3 WorldSpaceTangent : TEXCOORD5;
            float3 WorldSpaceBiTangent : TEXCOORD6;
            float3 WorldSpaceViewDirection : TEXCOORD7;
            half4 uv0 : TEXCOORD8;
            half4 uv1 : TEXCOORD9;

            UNITY_VERTEX_INPUT_INSTANCE_ID
            UNITY_VERTEX_OUTPUT_STEREO
        };

        float3 _LightDirection;

        VertexOutput ShadowPassVertex(GraphVertexInput v)
        {
            VertexOutput o;
            UNITY_SETUP_INSTANCE_ID(v);
            UNITY_TRANSFER_INSTANCE_ID(v, o);
            UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);

            // Vertex transformations performed by graph
            float3 WorldSpacePosition = mul(UNITY_MATRIX_M,v.vertex).xyz;
            float3 WorldSpaceNormal = normalize(mul(v.normal,(float3x3)UNITY_MATRIX_I_M));
            float3 WorldSpaceTangent = normalize(mul((float3x3)UNITY_MATRIX_M,v.tangent.xyz));
            float3 WorldSpaceBiTangent = cross(WorldSpaceNormal, WorldSpaceTangent.xyz) * v.tangent.w;
            float3 WorldSpaceViewDirection = _WorldSpaceCameraPos.xyz - mul(GetObjectToWorldMatrix(), float4(v.vertex.xyz, 1.0)).xyz;
            float4 uv0 = v.texcoord0;
            float4 uv1 = v.texcoord1;
            float3 ObjectSpacePosition = mul(UNITY_MATRIX_I_M,float4(WorldSpacePosition,1.0)).xyz;

            VertexDescriptionInputs vdi = (VertexDescriptionInputs)0;

            // Vertex description inputs defined by graph
            vdi.ObjectSpacePosition = ObjectSpacePosition;

            VertexDescription vd = PopulateVertexData(vdi);
            v.vertex.xyz = vd.Position;

            // Vertex shader outputs defined by graph
            o.WorldSpacePosition = WorldSpacePosition;
            o.WorldSpaceNormal = WorldSpaceNormal;
            o.WorldSpaceTangent = WorldSpaceTangent;
            o.WorldSpaceBiTangent = WorldSpaceBiTangent;
            o.WorldSpaceViewDirection = WorldSpaceViewDirection;
            o.uv0 = uv0;
            o.uv1 = uv1;


            float3 positionWS = TransformObjectToWorld(v.vertex.xyz);
            float3 normalWS = TransformObjectToWorldNormal(v.normal);

            float4 clipPos = TransformWorldToHClip(ApplyShadowBias(positionWS, normalWS, _LightDirection));

        #if UNITY_REVERSED_Z
            clipPos.z = min(clipPos.z, clipPos.w * UNITY_NEAR_CLIP_VALUE);
        #else
            clipPos.z = max(clipPos.z, clipPos.w * UNITY_NEAR_CLIP_VALUE);
        #endif
            o.clipPos = clipPos;

            return o;
        }

        half4 ShadowPassFragment(VertexOutput IN) : SV_TARGET
        {
            UNITY_SETUP_INSTANCE_ID(IN);
            UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(IN);

            // Pixel transformations performed by graph
            float3 WorldSpacePosition = IN.WorldSpacePosition;
            float3 WorldSpaceNormal = IN.WorldSpaceNormal;
            float3 WorldSpaceTangent = IN.WorldSpaceTangent;
            float3 WorldSpaceBiTangent = IN.WorldSpaceBiTangent;
            float3 WorldSpaceViewDirection = IN.WorldSpaceViewDirection;
            float4 uv0 = IN.uv0;
            float4 uv1 = IN.uv1;

            SurfaceDescriptionInputs surfaceInput = (SurfaceDescriptionInputs)0;

            // Surface description inputs defined by graph
            surfaceInput.uv0 = uv0;

            SurfaceDescription surf = PopulateSurfaceData(surfaceInput);

            float Alpha = 1;
            float AlphaClipThreshold = 0;

            // Surface description remap performed by graph
            Alpha = surf.Alpha;
            AlphaClipThreshold = surf.AlphaClipThreshold;

     #if _AlphaClip
            clip(Alpha - AlphaClipThreshold);
    #endif
            return 0;
        }

        ENDHLSL
    }

    Pass
    {
        Name "DepthOnly"
        Tags{"LightMode" = "DepthOnly"}

        ZWrite On
        ColorMask 0

            // Material options generated by graph
            Cull Back

            HLSLPROGRAM
            // Required to compile gles 2.0 with standard srp library
            #pragma prefer_hlslcc gles
            #pragma exclude_renderers d3d11_9x
            #pragma target 2.0

            //--------------------------------------
            // GPU Instancing
            #pragma multi_compile_instancing

            #pragma vertex vert
            #pragma fragment frag

            // Defines generated by graph

            #include "Packages/com.unity.render-pipelines.lightweight/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.lightweight/ShaderLibrary/Lighting.hlsl"
            #include "Packages/com.unity.render-pipelines.lightweight/ShaderLibrary/ShaderGraphFunctions.hlsl"
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"

            CBUFFER_START(UnityPerMaterial)
            float3 _Color;
            CBUFFER_END

            TEXTURE2D(_Texture); SAMPLER(sampler_Texture); float4 _Texture_TexelSize;
            SAMPLER(_SampleTexture2D_8286E042_Sampler_3_Linear_Repeat);
            struct VertexDescriptionInputs
            {
                float3 ObjectSpacePosition;
            };

            struct SurfaceDescriptionInputs
            {
                half4 uv0;
            };


            struct VertexDescription
            {
                float3 Position;
            };

            VertexDescription PopulateVertexData(VertexDescriptionInputs IN)
            {
                VertexDescription description = (VertexDescription)0;
                description.Position = IN.ObjectSpacePosition;
                return description;
            }

            struct SurfaceDescription
            {
                float Alpha;
                float AlphaClipThreshold;
            };

            SurfaceDescription PopulateSurfaceData(SurfaceDescriptionInputs IN)
            {
                SurfaceDescription surface = (SurfaceDescription)0;
                float4 _SampleTexture2D_8286E042_RGBA_0 = SAMPLE_TEXTURE2D(_Texture, sampler_Texture, IN.uv0.xy);
                float _SampleTexture2D_8286E042_R_4 = _SampleTexture2D_8286E042_RGBA_0.r;
                float _SampleTexture2D_8286E042_G_5 = _SampleTexture2D_8286E042_RGBA_0.g;
                float _SampleTexture2D_8286E042_B_6 = _SampleTexture2D_8286E042_RGBA_0.b;
                float _SampleTexture2D_8286E042_A_7 = _SampleTexture2D_8286E042_RGBA_0.a;
                surface.Alpha = _SampleTexture2D_8286E042_A_7;
                surface.AlphaClipThreshold = 1;
                return surface;
            }

            struct GraphVertexInput
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float4 tangent : TANGENT;
                float4 texcoord0 : TEXCOORD0;
                float4 texcoord1 : TEXCOORD1;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };


            struct VertexOutput
            {
                float2 uv           : TEXCOORD0;
                float4 clipPos      : SV_POSITION;
                // Interpolators defined by graph
                float3 WorldSpacePosition : TEXCOORD3;
                float3 WorldSpaceNormal : TEXCOORD4;
                float3 WorldSpaceTangent : TEXCOORD5;
                float3 WorldSpaceBiTangent : TEXCOORD6;
                float3 WorldSpaceViewDirection : TEXCOORD7;
                half4 uv0 : TEXCOORD8;
                half4 uv1 : TEXCOORD9;

                UNITY_VERTEX_INPUT_INSTANCE_ID
                UNITY_VERTEX_OUTPUT_STEREO
            };

            VertexOutput vert(GraphVertexInput v)
            {
                VertexOutput o = (VertexOutput)0;
                UNITY_SETUP_INSTANCE_ID(v);
                UNITY_TRANSFER_INSTANCE_ID(v, o);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);

                // Vertex transformations performed by graph
                float3 WorldSpacePosition = mul(UNITY_MATRIX_M,v.vertex).xyz;
                float3 WorldSpaceNormal = normalize(mul(v.normal,(float3x3)UNITY_MATRIX_I_M));
                float3 WorldSpaceTangent = normalize(mul((float3x3)UNITY_MATRIX_M,v.tangent.xyz));
                float3 WorldSpaceBiTangent = cross(WorldSpaceNormal, WorldSpaceTangent.xyz) * v.tangent.w;
                float3 WorldSpaceViewDirection = _WorldSpaceCameraPos.xyz - mul(GetObjectToWorldMatrix(), float4(v.vertex.xyz, 1.0)).xyz;
                float4 uv0 = v.texcoord0;
                float4 uv1 = v.texcoord1;
                float3 ObjectSpacePosition = mul(UNITY_MATRIX_I_M,float4(WorldSpacePosition,1.0)).xyz;

                VertexDescriptionInputs vdi = (VertexDescriptionInputs)0;

                // Vertex description inputs defined by graph
                vdi.ObjectSpacePosition = ObjectSpacePosition;

                VertexDescription vd = PopulateVertexData(vdi);
                v.vertex.xyz = vd.Position;

                // Vertex shader outputs defined by graph
                o.WorldSpacePosition = WorldSpacePosition;
                o.WorldSpaceNormal = WorldSpaceNormal;
                o.WorldSpaceTangent = WorldSpaceTangent;
                o.WorldSpaceBiTangent = WorldSpaceBiTangent;
                o.WorldSpaceViewDirection = WorldSpaceViewDirection;
                o.uv0 = uv0;
                o.uv1 = uv1;

                o.clipPos = TransformObjectToHClip(v.vertex.xyz);
                return o;
            }

            half4 frag(VertexOutput IN) : SV_TARGET
            {
                UNITY_SETUP_INSTANCE_ID(IN);
                UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(IN);

                // Pixel transformations performed by graph
                float3 WorldSpacePosition = IN.WorldSpacePosition;
                float3 WorldSpaceNormal = IN.WorldSpaceNormal;
                float3 WorldSpaceTangent = IN.WorldSpaceTangent;
                float3 WorldSpaceBiTangent = IN.WorldSpaceBiTangent;
                float3 WorldSpaceViewDirection = IN.WorldSpaceViewDirection;
                float4 uv0 = IN.uv0;
                float4 uv1 = IN.uv1;

                SurfaceDescriptionInputs surfaceInput = (SurfaceDescriptionInputs)0;

                // Surface description inputs defined by graph
                surfaceInput.uv0 = uv0;

                SurfaceDescription surf = PopulateSurfaceData(surfaceInput);

                float Alpha = 1;
                float AlphaClipThreshold = 0;

                // Surface description remap performed by graph
                Alpha = surf.Alpha;
                AlphaClipThreshold = surf.AlphaClipThreshold;

         #if _AlphaClip
                clip(Alpha - AlphaClipThreshold);
        #endif
                return 0;
            }
            ENDHLSL
        }
}
FallBack "Hidden/InternalErrorShader"
}
