using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;

//======================================================
// [GoalTrigger]
// �쐬�ҁF���{
// �ŏI�X�V���F04/17
// 
// �S�[���I�u�W�F�N�g�ɐG��Ă���V�[���J�ڂ���܂ł̊Ԃ��َq���t�H�[�J�X���鏈����������
//
// [Log]
// 04/17�@���{�@UI�𐔕b�\����A�V�[���J��
// 
//======================================================
public class GoalTrigger : MonoBehaviour
{
    // �J�ڐ�V�[����
    [Header("�J�ڐ�V�[��")]
    public string nextSceneName;

    // �N���A����UI�e�L�X�g
    [Header("UI�N���A�e�L�X�g")]
    public TextMeshProUGUI clearTextObject;

    // �J�ڂ܂ł̑ҋ@����
    [Header("�J�ڑҋ@����")]
    public float delaySecond;

    // �N���A���ɒ�������I�u�W�F�N�g
    [Header("���ڂ���Ώ�")]
    public Transform focusTarget;

    [Header("�J��������鋗���itarget����̋����j")]
    public float focusDistance = 5.0f;

    [Header("�J�����̈ړ����x")]
    public float cameraMoveSpeed = 3.0f;

    private Camera mainCamera;
    private bool isFocusing = false;

    // UI���A�N�e�B�u��
    private void Start()
    {
        mainCamera = Camera.main;

        // �Q�[���J�n���Ɉ�x�����AUI���\����
        if (clearTextObject != null)
        {
            clearTextObject.gameObject.SetActive(false);
            Debug.Log($"Start() -> clearTextObject [{clearTextObject.name}] ���\���ɂ��܂���");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // �ڐG���肪 "GoalWall" �^�O���ǂ����𔻒�
        if (other.CompareTag("GoalWall"))
        {
            Debug.Log("GoalWall �ɓ�������");
            // �V�[���J�ڎ��s
            // �N���AUI��\�����Ă���J��
            StartCoroutine(ShowClearAndLoad());
        }
    }

    // UI�\��
    private IEnumerator ShowClearAndLoad()
    {
        // UI�\��
        if (clearTextObject != null)    
            clearTextObject.gameObject.SetActive(true);
        else
            Debug.LogWarning("clearTextObject ���ݒ肳��Ă��܂���I");

        // �J�����t�H�[�J�X�J�n
        isFocusing = true;

        // ���b�ҋ@
        yield return new WaitForSeconds(delaySecond);

        // �V�[���J��
        TriggerSceneTransition();
    }

    private void Update()
    {
        if (isFocusing && focusTarget != null && mainCamera != null)
        {
            Debug.Log("clearTextObject �t�H�[�J�X�J�n");
            // ���ڃ|�W�V�����F�^�[�Q�b�g�̏�������i�J����������ʒu�j
            //Vector3 focusPos = focusTarget.position - focusTarget.forward * focusDistance + Vector3.up * 2.0f;

            // �J���������炩�Ɉړ�
            //mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, focusPos, Time.deltaTime * cameraMoveSpeed);

            // �J�������^�[�Q�b�g�Ɍ�����
            mainCamera.transform.LookAt(focusTarget);
        }
    }

    // �V�[���J�ڎ��s�֐�
    private void TriggerSceneTransition()
    {
        if (!string.IsNullOrEmpty(nextSceneName)) // �V�[�������ݒ肳��Ă���ꍇ�̂ݎ��s
        {
            Debug.Log("Loading Scene: " + nextSceneName);
            SceneManager.LoadScene(nextSceneName);
        }
        else
        {
            Debug.LogError("���̃V�[�������ݒ肳��Ă��܂���I");
        }
    }
}
