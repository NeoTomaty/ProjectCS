//SphereSpawner.cs
//�쐬��:��������
//�A�^�b�`:SpherePrefab�ɃA�^�b�`
//�ŏI�X�V��:2025/04/23
//[Log]
//04/23�@�����@�t�B�[�o�[�^�C���ɓ������狅�̂��~���Ă��鏈��

using UnityEngine;

public class SphereSpawner : MonoBehaviour
{
    //�X�|�[�����鋅�̂̃v���n�u
    public GameObject SpherePrefab;

    //���̂̃X�|�[���Ԋu
    public float SpawnInterval = 0.5f;

    //�X�|�[�����L�����ǂ����������t���O
    private bool IsSpawning = false;

    //�Đ�����h���t���O
    private bool CanSpawn = true;

    void Start()
    {

    }

    //���̂̃X�|�[�����J�n����֐�
    public void StartSpawning()
    {
        //�Đ�����������Ă���Ƃ��̂݃X�|�[�����J�n
        if (CanSpawn)
        {
            IsSpawning = true;

            //�w�肳�ꂽ�Ԋu�ŋ��̂��X�|�[������
            InvokeRepeating("SpawnSphere", 0.0f, SpawnInterval);
        }
    }

    //���̂̃X�|�[�����~����֐�
    public void StopSpawning()
    {
        IsSpawning = false;

        //�X�|�[���̌J��Ԃ��Ăяo�����L�����Z��
        CancelInvoke("SpawnSphere");
    }

    //�Đ�����h���֐�
    public void DisableSpawning()
    {
        CanSpawn = false; //�Đ�����h��
    }

    //���̂��X�|�[������֐�
    void SpawnSphere()
    {
        //�X�|�[�����L���̂Ƃ��̂݋��̂𐶐�
        if (IsSpawning)
        {
            //���̂������_���Ȉʒu�ɐ���
            GameObject Sphere = Instantiate(SpherePrefab,
                new Vector3(Random.Range(-10.0f, 10.0f), 10.0f,
                    Random.Range(-10.0f, 10.0f)), Quaternion.identity);

            //�������ꂽ���̂Ƀ^�O��ǉ�
            Sphere.tag = "Sphere";
        }
    }
}
