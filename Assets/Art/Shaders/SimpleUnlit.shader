Shader "Unlit/SimpleUnlit"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Cutoff ("Alpha Cutoff", Range(0,1)) = 0.5
    }
    SubShader
    {
        Tags
        {
            "RenderType"="TransparentCutout" "Queue"="AlphaTest"
        }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_instancing
            #pragma multi_compile _ DOTS_INSTANCING_ON
            #pragma shader_feature _ALPHACLIP_ON

            #ifdef DOTS_INSTANCING_ON
            #define UNITY_DOTS_INSTANCING_ENABLED 1
            #endif

            #include "UnityCG.cginc"
            #include "UnityInstancing.cginc"

            #ifndef REAL_IS_DEFINED
            #define real  float
            #define real2 float2
            #define real3 float3
            #define real4 float4
            #endif

            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/UnityDOTSInstancing.hlsl"

            #ifdef DOTS_INSTANCING_ON
            UNITY_DOTS_INSTANCING_START(PerInstance)
                UNITY_DOTS_INSTANCED_PROP(float4x4, unity_ObjectToWorld)
                UNITY_DOTS_INSTANCED_PROP(float4x4, unity_WorldToObject)
            UNITY_DOTS_INSTANCING_END(PerInstance)

            #define unity_ObjectToWorld UNITY_ACCESS_DOTS_INSTANCED_PROP(float4x4, unity_ObjectToWorld)
            #define unity_WorldToObject UNITY_ACCESS_DOTS_INSTANCED_PROP(float4x4, unity_WorldToObject)
            #endif

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            sampler2D _MainTex;

            CBUFFER_START(UnityPerMaterial)
            float _Cutoff;
            CBUFFER_END

            v2f vert(appdata v)
            {
                v2f o;
                UNITY_SETUP_INSTANCE_ID(v);
                UNITY_TRANSFER_INSTANCE_ID(v, o);
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                UNITY_SETUP_INSTANCE_ID(i);
                fixed4 col = tex2D(_MainTex, i.uv);
                #ifdef _ALPHACLIP_ON
                clip(col.a - _Cutoff);
                #endif
                return col;
            }
            ENDCG
        }
    }
}