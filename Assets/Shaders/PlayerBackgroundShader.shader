Shader "Hidden/PlayerBackgroundShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
    }
    SubShader
    {
        // No culling or depth
        Cull Off ZWrite Off ZTest Always

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            //TEXTURE2D_SAMPLER2D(_CameraDepthTexture, sampler_CameraDepthTexture);
            sampler2D _CameraDepthTexture;
            float _Scale = 300;
            float4 _MainTex_TexelSize;

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;

            };

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;

                return o;
            }

            sampler2D _MainTex;

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);

                float depth = tex2D(_CameraDepthTexture, i.uv).r;
                if (depth != 0)//if this is not part of the background just stop the shader with default color
                    return col;
                
                //float halfScaleFloor = floor(_Scale * 0.5);
                //float halfScaleCeil = ceil(_Scale * 0.5);
                float halfScaleFloor = floor(5 * 0.5);
                float halfScaleCeil = ceil(5 * 0.5);


                // Sample the pixels in an X shape, roughly centered around i.texcoord.
                // we use the above variables to ensure we offset exactly one pixel at a time.
                float2 bottomLeftUV = i.uv - float2(_MainTex_TexelSize.x, _MainTex_TexelSize.y) * halfScaleFloor;
                float2 topRightUV = i.uv + float2(_MainTex_TexelSize.x, _MainTex_TexelSize.y) * halfScaleCeil;
                float2 bottomRightUV = i.uv + float2(_MainTex_TexelSize.x * halfScaleCeil, -_MainTex_TexelSize.y * halfScaleFloor);
                float2 topLeftUV = i.uv + float2(-_MainTex_TexelSize.x * halfScaleFloor, _MainTex_TexelSize.y * halfScaleCeil);

                bool bottomLeftValid = tex2D(_CameraDepthTexture, bottomLeftUV).r != 0;
                bool topRightValid = tex2D(_CameraDepthTexture, topRightUV).r != 0;
                bool bottomRightValid = tex2D(_CameraDepthTexture, bottomRightUV).r != 0;
                bool topLeftValid = tex2D(_CameraDepthTexture, topLeftUV).r != 0;
                
                // just invert the colors
                fixed4 colInvert;
                colInvert.rgb = 1 - col.rgb;
                

                if (!bottomLeftValid & !topRightValid & !bottomRightValid & !topLeftValid)//checks if to far out to render effect
                    return col;


                return float4(0.5f, 0.9f, 0.95f, 0); //effect
            }
            ENDCG
        }
    }
}
