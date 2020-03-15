Shader "Unlit/Track Shader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _ScrollXSpeed("X", Range(0,10)) = 2
        _ScrollYSpeed("Y", Range(0,10)) = 3
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            fixed _ScrollXSpeed;
            fixed _ScrollYSpeed;

            v2f vert (appdata v)
            {
                fixed xScrollValue = _ScrollXSpeed * _Time;
                fixed yScrollValue = _ScrollYSpeed * _Time;

                fixed2 uv = TRANSFORM_TEX(v.uv, _MainTex);

                fixed2 scrolledUV = uv + fixed2(xScrollValue, yScrollValue);

                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = scrolledUV;
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                fixed4 col = tex2D(_MainTex, i.uv);
                // apply fog
                UNITY_APPLY_FOG(i.fogCoord, col);
                return col;
            }
            ENDCG
        }
    }
}
