Shader "Unlit/MultiplyProjectorShader"
{
    Properties
    {
        _ShadowTex("Cookie", 2D) = "gray" {}
        _Color("Tint Color", Color) = (0, 0, 0, 1) // âeÇ¡Ç€Ç≠çï
    }

    SubShader
    {
        Tags { "Queue"="Transparent" "IgnoreProjector"="True" }
        Pass
        {
            ZWrite Off
            ColorMask RGB
            Blend DstColor Zero
            Offset -1, -1

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            sampler2D _ShadowTex;
            fixed4 _Color;

            struct appdata_t
            {
                float4 vertex : POSITION;
                float4 texcoord : TEXCOORD0;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float4 uv : TEXCOORD0;
            };

            v2f vert(appdata_t v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.texcoord;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                fixed4 tex = tex2D(_ShadowTex, i.uv.xy);
return tex * _Color * tex.a;

            }

            ENDCG
        }
    }

}
