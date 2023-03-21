Shader "Unlit/ProcessorShader"
{
    Properties
    {
        _MainTex("Texture", 2D) = "white" {}
        _SineOffsetAmountX("Sine Offset X-Amount", Float) = 0
        _SineOffsetAmountY("Sine Offset Y-Amount", Float) = 0
        _SineOffsetTime("Sine Offset Time", Float) = 2
        _RotationAmount("Rotation Amount", Float) = 0
        _RotationSinAmount("Rotation Sin Amount", Float) = 1
        _RotationCosAmount("Rotation Cos Amount", Float) = 1
        _RotationCosMod("Rotation CosMod", Float) = 1
        _RotationSinMod("Rotation SinMod", Float) = 1
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

            #include "UnityCG.cginc"

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

            sampler2D _MainTex;
            float4 _MainTex_ST;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            float _SineOffsetAmountX;
            float _SineOffsetAmountY;
            float _SineOffsetTime;
            float _RotationAmount;
            float _RotationSinAmount;
            float _RotationCosAmount;
            float _RotationCosMod;
            float _RotationSinMod;
            
            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                float2 uv = i.uv;

                uv.x += _SineOffsetAmountX*sin(_SineOffsetTime*uv.x);
                uv.y += _SineOffsetAmountY*sin(_SineOffsetTime*uv.y);

                float acos = _RotationCosMod * cos(_RotationAmount * _RotationSinAmount);
                float asin = _RotationSinMod * sin(_RotationAmount * _RotationCosAmount);
                uv = float2(acos * uv.x - asin * uv.y,asin * uv.x + acos * uv.y);

                float2 diff = (normalize(i.uv - uv) + 0.5) * 0.5;
                return float4(diff.x,diff.y,1,1);
                fixed4 col = tex2D(_MainTex, uv);
                return col;
            }
            ENDCG
        }
    }
}
