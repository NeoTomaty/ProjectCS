//====================================================
// スクリプト名：GlowAndGrowOnContact
// 作成者：森脇
// 内容：ステージセレクトのObjectに接触したら、オブジェクトが光って大きくなる
// 最終更新日：05/04
//
// [Log]
// 05/04 森脇 スクリプト作成
//====================================================

using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class GlowAndGrowOnContact : MonoBehaviour
{
    public string playerTag = "Player";
    public Color glowColor = Color.yellow;
    public float emissionIntensity = 2.0f;
    public float scaleMultiplier = 1.2f;      // 大きくなる倍率
    public float scaleSpeed = 5.0f;           // 拡大/縮小のスピード

    private Material _material;
    private Color _originalEmission;
    private Vector3 _originalScale;
    private Vector3 _targetScale;
    private bool _isPlayerInContact = false;

    private void Start()
    {
        _material = GetComponent<Renderer>().material;
        _originalEmission = _material.GetColor("_EmissionColor");
        _material.EnableKeyword("_EMISSION");

        _originalScale = transform.localScale;
        _targetScale = _originalScale;
    }

    private void Update()
    {
        // スケールの更新（スムーズに変化）
        transform.localScale = Vector3.Lerp(transform.localScale, _targetScale, Time.deltaTime * scaleSpeed);

        // プレイヤーが接触している間だけ光らせる
        if (_isPlayerInContact)
        {
            SetEmission(glowColor * emissionIntensity); // エミッションを設定
        }
        else
        {
            SetEmission(_originalEmission); // 元のエミッションに戻す
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
            _isPlayerInContact = true;
            SetEmission(glowColor * emissionIntensity);
            _targetScale = _originalScale * scaleMultiplier;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
            _isPlayerInContact = false;
            SetEmission(_originalEmission);
            _targetScale = _originalScale;
        }
    }

    private void SetEmission(Color color)
    {
        _material.SetColor("_EmissionColor", color);
    }
}