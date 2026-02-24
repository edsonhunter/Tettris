Shader "Custom/CosmicPiece"
{
    Properties
    {
        _Color ("Main Color", Color) = (1,1,1,1)
        _RimColor ("Rim Color", Color) = (1, 1, 1, 1)
        _RimPower ("Rim Power", Range(0.1, 8.0)) = 2.0
        _PulseSpeed ("Pulse Speed", Float) = 3.0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float3 worldNormal : TEXCOORD0;
                float3 viewDir : TEXCOORD1;
            };

            fixed4 _Color;
            fixed4 _RimColor;
            float _RimPower;
            float _PulseSpeed;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.worldNormal = UnityObjectToWorldNormal(v.normal);
                o.viewDir = WorldSpaceViewDir(v.vertex);
                // Inherit color purely from material's _Color property
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float3 normal = normalize(i.worldNormal);
                float3 viewDir = normalize(i.viewDir);
                
                float NdotV = 1.0 - saturate(dot(normal, viewDir));
                float rim = pow(NdotV, _RimPower);
                
                float pulse = sin(_Time.y * _PulseSpeed) * 0.5 + 0.5;
                
                fixed4 base = _Color * 0.8;
                // Core pulse 
                fixed4 core = _RimColor * pulse * 0.4 * (1.0 - rim);
                // Glowing Edge
                fixed4 edge = _RimColor * rim * 1.5;
                
                return base + core + edge;
            }
            ENDCG
        }
    }
}
