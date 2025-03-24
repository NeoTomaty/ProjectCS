//�t�@�C����:CameraTracking.cs
//�쐬��:��������
//�쐬��:2025/03/24
//�X�V����:
//2025/03/24:���ō쐬
//�T�v:�v���C���[�ɃJ�����Ǐ]�@�\������

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTracking : MonoBehaviour
{
    //�v���C���[�I�u�W�F�N�g���i�[����ϐ�
    private GameObject player;

    //�J������y�������̃I�t�Z�b�g
    public float yOffset;

    //�J������z�������̃I�t�Z�b�g
    public float zOffset;

    // Start is called before the first frame update
    void Start()
    {
        //"Player"�Ƃ������O�̃I�u�W�F�N�g���V�[������T����player�Ɋi�[
        player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        //�v���C���[�̌��݂�x���W���擾
        float x = player.transform.position.x;

        //�v���C���[�̌��݂�y���W���擾
        float y = player.transform.position.y;

        //�v���C���[�̌��݂�z���W���擾
        float z = player.transform.position.z;

        //�J�����̈ʒu���v���C���[�̈ʒu�ɃI�t�Z�b�g���������ʒu�ɐݒ�
        transform.position = new Vector3(x, y + yOffset, z + zOffset);
    }
}