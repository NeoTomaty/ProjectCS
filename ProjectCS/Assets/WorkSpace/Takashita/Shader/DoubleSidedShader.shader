//====================================================
// �J�X�^���V�F�[�_�[�i�I�u�W�F�N�g���ʕ`��j
// �쐬�ҁF����
// �ŏI�X�V���F04/26
// 
// [Log]
// 04/26 ���� �V�F�[�_�[�쐬
//
//====================================================
Shader "Custom/DoubleSidedShader"
{
    Properties
    {
         _Color ("Main Color", Color) = (1,1,1,1) // �F�{�����x
    }
    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" }
        LOD 100

        Pass
        {
            Cull Off           // ���\�ǂ�����`��
            ZWrite Off         // �������̂Ȃ̂Ő[�x�������ݖ���
            Blend SrcAlpha OneMinusSrcAlpha  // ���u�����h�i�������j
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
