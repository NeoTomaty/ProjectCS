Shader "Unlit/StripeBarrier"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _StripeWidth ("Stripe Width", Float) = 10.0
        _StripeAngle ("Stripe Angle", Range(0,360)) = 45
        _ScrollSpeed ("Scroll Speed", Float) = 0.1
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }
        LOD 100

        Pass
        {
            ZWrite Off
            Blend SrcAlpha OneMinusSrcAlpha
            Cull Off

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            float4 _Color;
            float _StripeWidth;
            float _StripeAngle;
            float _ScrollSpeed;

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

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                // 中心基準のUVに変換
                float2 centeredUV = i.uv - 0.5;

                // 角度指定に基づく方向ベクトル
                float rad = radians(_StripeAngle);
                float2 direction = float2(cos(rad), sin(rad));

                // 時間経過でUVを移動させる
                float scroll = _Time * _ScrollSpeed;
                float stripeCoord = dot(centeredUV, direction) + scroll;

                // stripeCoordにより縞パターン生成：片側だけ（ON/OFF）
                float pattern = fmod(stripeCoord * _StripeWidth, 1.0);
                float mask = step(0.5, pattern); // 0〜0.5: 表示 / 0.5〜1: 透明

                fixed4 col = _Color;
                col.a *= mask;

                return col;
            }
            ENDCG
        }
    }
}