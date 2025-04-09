//======================================================
// [�X�N���v�g��]Enemy1
// �쐬�ҁF�X�e���S
// �ŏI�X�V���F4/01
//
// [Log]
// 3/31  �X�e�@�X�N���v�g�쐬
//======================================================

using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public int score = 0;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    public void AddScore(int value)
    {
        score += value;
        Debug.Log("Score: " + score);
    }
}