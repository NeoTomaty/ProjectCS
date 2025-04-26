//====================================================
// カスタムシェーダー（オブジェクト両面描画）
// 作成者：高下
// 最終更新日：04/26
// 
// [Log]
// 04/26 高下 シェーダー作成
//
//====================================================
Shader "Custom/DoubleSidedShader"
{
    Properties
    {
         _Color ("Main Color", Color) = (1,1,1,1) // 色＋透明度
    }
    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" }
        LOD 100

        Pass
        {
            Cull Off           // 裏表どちらも描画
            ZWrite Off         // 透明物体なので深度書き込み無効
            Blend SrcAlpha OneMinusSrcAlpha  // αブレンド（透明化）
            Lighting Off 


            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            float4 _Color;
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

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
              return _Color;
            }
            ENDCG
        }
    }
}
