Shader "Custom/OutlineShader"
{
    Properties
    {
        _Color ("Border Color", Color) = (1,0,0,1)
        _BorderWidth ("Border Width", Range(0,0.5)) = 0.05
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }
        LOD 100

        Blend SrcAlpha OneMinusSrcAlpha
        ZWrite Off
        Cull Off

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float3 localPos : TEXCOORD0;
            };

            fixed4 _Color;
            float _BorderWidth;

            v2f vert(appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.localPos = v.vertex.xyz;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                float3 p = i.localPos;
                
                float halfX = 4.5;
                float halfZ = 4.5;

                float edgeX = step(halfX - _BorderWidth, abs(p.x));
                float edgeZ = step(halfZ - _BorderWidth, abs(p.z));
                
                float mask = saturate(edgeX + edgeZ);

                return fixed4(_Color.rgb, mask * _Color.a);
            }
            ENDCG
        }
    }
}
