Shader "Unlit/ProcessorShader"
{
    Properties
    {
        _MainTex("Texture", 2D) = "white" {}
        _Noise("Noise Texture", 2D) = "white" {}
        _SineOffsetAmountX("Sine Offset X-Amount", Float) = 0
        _SineOffsetAmountY("Sine Offset Y-Amount", Float) = 0
        _SineOffsetTime("Sine Offset Time", Float) = 2
        _RotationAmount("Rotation Amount", Float) = 0
        _RotationSinAmount("Rotation Sin Amount", Float) = 1
        _RotationCosAmount("Rotation Cos Amount", Float) = 1
        _RotationCosMod("Rotation CosMod", Float) = 1
        _RotationSinMod("Rotation SinMod", Float) = 1
        _XOffset("X Offset", Float) = 0
        _YOffset("Y Offset", Float) = 0
        
        _NoiseXOffset("Noise X Offset", Float) = 0
        _NoiseYOffset("Noise Y Offset", Float) = 0
        _NoiseScale("Noise Scale", Float) = 1
        _NoiseXAmount("Noise X Amount", Float) = 0
        _NoiseYAmount("Noise Y Amount", Float) = 0
        
        _PNoiseXOffset("Perlin Noise X Offset", Float) = 0
        _PNoiseYOffset("Perlin Noise Y Offset", Float) = 0
        _PNoiseScale("Perlin Noise Scale", Float) = 1
        _PNoiseXAmount("Perlin Noise X Amount", Float) = 0
        _PNoiseYAmount("Perlin Noise Y Amount", Float) = 0
        
        _VNoiseXOffset("Vornoi Noise X Offset", Float) = 0
        _VNoiseYOffset("Vornoi Noise Y Offset", Float) = 0
        _VNoiseScale("Vornoi Noise Scale", Float) = 1
        _VNoiseXAmount("Vornoi Noise X Amount", Float) = 0
        _VNoiseYAmount("Vornoi Noise Y Amount", Float) = 0
        
        _RNoiseXOffset("Radial Noise X Offset", Float) = 0
        _RNoiseYOffset("Radial Noise Y Offset", Float) = 0
        _RNoiseScale("Radial Noise Scale", Float) = 1
        _RNoiseXAmount("Radial Noise X Amount", Float) = 0
        _RNoiseYAmount("Radial Noise Y Amount", Float) = 0
        
        _Scale("Scale", Float) = 1
        _ShaderMode("Shader Mode", Int) = 0
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
            sampler2D _Noise;

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
            float _XOffset;
            float _YOffset;
            float _Scale;

            float _NoiseXOffset;
            float _NoiseYOffset;
            float _NoiseScale;
            float _NoiseXAmount;
            float _NoiseYAmount;

            float _PNoiseXOffset;
            float _PNoiseYOffset;
            float _PNoiseScale;
            float _PNoiseXAmount;
            float _PNoiseYAmount;

            float _VNoiseXOffset;
            float _VNoiseYOffset;
            float _VNoiseScale;
            float _VNoiseXAmount;
            float _VNoiseYAmount;

            float _RNoiseXOffset;
            float _RNoiseYOffset;
            float _RNoiseScale;
            float _RNoiseXAmount;
            float _RNoiseYAmount;
            
            int _ShaderMode;
            
            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                float2 uv = i.uv;

                uv += float2(_XOffset,_YOffset);

                uv *= _Scale;
                
                uv.x += _SineOffsetAmountX*sin(_SineOffsetTime*uv.x);
                uv.y += _SineOffsetAmountY*sin(_SineOffsetTime*uv.y);

                float acos = _RotationCosMod * cos(_RotationAmount * _RotationSinAmount);
                float asin = _RotationSinMod * sin(_RotationAmount * _RotationCosAmount);
                uv = float2(acos * uv.x - asin * uv.y,asin * uv.x + acos * uv.y);

                float noise = tex2D(_Noise, uv + float2(_NoiseXOffset,_NoiseYOffset) * _NoiseScale).r;
                uv += float2(_NoiseXAmount * noise,_NoiseYAmount * noise);
                float perlin = tex2D(_Noise,uv + float2(_PNoiseXOffset,_PNoiseYOffset) * _PNoiseScale).g;
                uv += float2(_PNoiseXAmount * perlin,_PNoiseYAmount * perlin);
                float vornoi = tex2D(_Noise,uv + float2(_VNoiseXOffset,_VNoiseYOffset) * _VNoiseScale).b;
                uv += float2(_VNoiseXAmount * vornoi,_VNoiseYAmount * vornoi);
                float radial = tex2D(_Noise,uv + float2(_RNoiseXOffset,_RNoiseYOffset) * _RNoiseScale).a;
                uv += float2(_RNoiseXAmount * radial,_RNoiseYAmount * radial);
                //return float4((uv-i.uv).x,(uv-i.uv).y,1,1);
                float2 diff = (normalize(uv - i.uv) + 0.5) * 0.5;
                if(_ShaderMode == 0) return tex2D(_MainTex, uv);
                if(_ShaderMode == 1) return float4(diff.x,diff.y,1,1);
                return float4(0,0,0,1);
            }
            ENDCG
        }
    }
}
