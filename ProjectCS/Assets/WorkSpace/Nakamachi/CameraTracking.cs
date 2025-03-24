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
    private GameObject player;
    public float yOffset; //y�������̃I�t�Z�b�g
    public float zOffset; //z�������̃I�t�Z�b�g

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        float x = player.transform.position.x;
        float y = player.transform.position.y;
        float z = player.transform.position.z;
        transform.position = new Vector3(x, y + yOffset, z + zOffset);
    }
}