//====================================================
// �X�N���v�g���FGlowAndGrowOnContact
// �쐬�ҁF�X�e
// ���e�F�X�e�[�W�Z���N�g��Object�ɐڐG������A�I�u�W�F�N�g�������đ傫���Ȃ�
// �ŏI�X�V���F05/04
//
// [Log]
// 05/04 �X�e �X�N���v�g�쐬
//====================================================

using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class GlowAndGrowOnContact : MonoBehaviour
{
    public string playerTag = "Player";
    public Color glowColor = Color.yellow;
    public float emissionIntensity = 2.0f;
    public float scaleMultiplier = 1.2f;      // �傫���Ȃ�{��
    public float scaleSpeed = 5.0f;           // �g��/�k���̃X�s�[�h

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
        // �X�P�[���̍X�V�i�X���[�Y�ɕω��j
        transform.localScale = Vector3.Lerp(transform.localScale, _targetScale, Time.deltaTime * scaleSpeed);

        // �v���C���[���ڐG���Ă���Ԃ������点��
        if (_isPlayerInContact)
        {
            SetEmission(glowColor * emissionIntensity); // �G�~�b�V������ݒ�
        }
        else
        {
            SetEmission(_originalEmission); // ���̃G�~�b�V�����ɖ߂�
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