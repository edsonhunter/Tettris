Shader "Custom/CosmicBackground"
{
    Properties
    {
        _BgColor ("Background Color", Color) = (0.02, 0.02, 0.04, 1)
        _NebulaColor ("Nebula Color", Color) = (0.2, 0.1, 0.4, 1)
        _StarColor ("Star Color", Color) = (1, 1, 1, 1)
        _Speed ("Speed", Float) = 0.2
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" "Queue"="Background" }
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

            float4 _BgColor;
            float4 _NebulaColor;
            float4 _StarColor;
            float _Speed;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            float random(float2 uv)
            {
                return frac(sin(dot(uv, float2(12.9898, 78.233))) * 43758.5453123);
            }

            float noise(float2 uv)
            {
                float2 i = floor(uv);
                float2 f = frac(uv);
                float a = random(i);
                float b = random(i + float2(1.0, 0.0));
                float c = random(i + float2(0.0, 1.0));
                float d = random(i + float2(1.0, 1.0));
                float2 u = f * f * (3.0 - 2.0 * f);
                return lerp(lerp(a, b, u.x), lerp(c, d, u.x), u.y);
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float2 uv = i.uv;
                uv.y -= _Time.y * _Speed * 0.1;

                // Nebula
                float n = noise(uv * 3.0 + _Time.y * 0.02) + noise(uv * 6.0 - _Time.y * 0.01) * 0.5;
                fixed4 col = lerp(_BgColor, _NebulaColor, n * 0.6);

                // Stars 1
                float2 s1 = uv * 30.0;
                s1.y -= _Time.y * _Speed * 0.5;
                float r1 = random(floor(s1));
                if (r1 > 0.985) col += _StarColor * (sin(_Time.y * 3.0 + r1 * 50.0) * 0.5 + 0.5);

                // Stars 2
                float2 s2 = uv * 50.0;
                s2.y -= _Time.y * _Speed * 0.8;
                float r2 = random(floor(s2));
                if (r2 > 0.99) col += _StarColor * 0.8 * (sin(_Time.y * 5.0 + r2 * 50.0) * 0.5 + 0.5);

                return saturate(col);
            }
            ENDCG
        }
    }
}
