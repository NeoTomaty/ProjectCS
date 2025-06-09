//====================================================
// スクリプト名：StageSelectChangeModel
// 作成者：高下
// 内容：ステージセレクトで2つのモデルを切り替えるスクリプト
// 最終更新日：06/09
// 
// [Log]
// 06/09 高下 スクリプト作成
//====================================================
using UnityEngine;

public class StageSelectChangeModel : MonoBehaviour
{

    [SerializeField] private GameObject Model1;
    [SerializeField] private GameObject Model2;

    void Start()
    {
        Model1.SetActive(true);
        Model2.SetActive(false);
    }

    public void SetChangeModel(bool isSpecialMode)
    {
        Model1.SetActive(isSpecialMode);
        Model2.SetActive(!isSpecialMode);
    }
}
