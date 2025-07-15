//====================================================
// �X�N���v�g���FSnackLauncher
// �쐬�ҁF���{
// 
// [Log]
// 05/07 ���{�@�J�E���g�_�E���I�����Snack��ł��グ�鏈������
//====================================================

using UnityEngine;
using System.Collections;

public class SnackLauncher : MonoBehaviour
{
    private BlownAway_Ver3 BAV3;

    void Start()
    {
        BAV3 = GetComponent<BlownAway_Ver3>();

        if (!gameObject.name.EndsWith("(Clone)"))
        {
            BAV3.enabled = false;
        }
    }

    // Snack��ł��グ��֐�
    public void Launch()
    {
        StartCoroutine(EnableAndLaunch());
    }

    private IEnumerator EnableAndLaunch()
    {
        BAV3.enabled = true;

        yield return null; // ������1�t���[���҂�

        BAV3.MoveToRandomXZInRespawnArea();
        BAV3.Launch();
        BAV3.PlayLaunchEffect();
        Debug.Log("Snack��ł��グ�܂����I");
    }
}
