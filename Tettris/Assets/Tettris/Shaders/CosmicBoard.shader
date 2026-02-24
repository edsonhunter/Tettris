Shader "Custom/CosmicBoard"
{
    Properties
    {
        _PulseSpeed ("Pulse Speed", Float) = 2.0
        _PulseIntensity ("Pulse Intensity", Float) = 0.4
    }
    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" }
        Blend SrcAlpha OneMinusSrcAlpha
        ZWrite Off

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
                fixed4 color : COLOR;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                fixed4 color : COLOR;
            };

            float _PulseSpeed;
            float _PulseIntensity;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                o.color = v.color;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = i.color;
                
                // Add a subtle cosmic pulse to the highlight color
                float pulse = sin(_Time.y * _PulseSpeed) * 0.5 + 0.5;
                col.rgb += col.rgb * pulse * _PulseIntensity;
                
                // Outer glow per cell
                float2 uvCenter = abs(i.uv - 0.5) * 2.0; 
                float edge = max(uvCenter.x, uvCenter.y);
                edge = pow(edge, 4.0); 
                
                fixed3 edgeGlow = fixed3(0.5, 0.7, 1.0) * edge * 0.3;
                col.rgb += edgeGlow;
                
                return col;
            }
            ENDCG
        }
    }
}
