//======================================================
// [TutorialGanarateSnack]
// 作成者：荒井修
// 最終更新日：06/27
// 
// [Log]
// 06/26　荒井　壁に一定回数ぶつかるとスナックが生成されるように実装
// 06/27  高下 参照オブジェクトの追加
// 06/27  荒井 処理を全体的に変更
//======================================================
using UnityEngine;

public class TutorialGanarateSnack : MonoBehaviour
{
    [Header("生成するスナック")]
    [SerializeField] private GameObject SnackObject;

    [Header("生成までの衝突回数")]
    [SerializeField] private int ToGanarateIndex = 3;

    [SerializeField] private TutorialDisplayTexts TutorialDisplayTextsComponent;

    [Header("開始時に非表示にするオブジェクトの参照")]
    [SerializeField] private GameObject ArrowObject;
    [SerializeField] private GameObject SnackHeightGaugeObject;

    private int CollidedIndex = 0;

    private void Start()
    {
        ArrowObject.SetActive(false);
        SnackHeightGaugeObject.SetActive(false);
    }

    public void OnCollided()
    {
        CollidedIndex++;

        if (CollidedIndex != ToGanarateIndex) return;

        if (SnackObject != null)
        {
            SnackObject.SetActive(true);
            ArrowObject.SetActive(true);
            SnackHeightGaugeObject.SetActive(true);

            TutorialDisplayTextsComponent.DisplayTutorialUI2();
        }
    }
}