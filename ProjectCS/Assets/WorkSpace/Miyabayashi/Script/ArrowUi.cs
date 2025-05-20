//======================================================
// ArrowUiスクリプト
// 作成者：宮林
// 最終更新日：5/7
// 
// [Log]５/7 宮林　スナック方向への矢印表示
//======================================================
using UnityEngine;
using UnityEngine.UI;

public class ArrowUi : MonoBehaviour
{
    public Transform target;
    public Camera cam;
    public Image leftArrow;
    public Image rightArrow;
    public Image upArrow;

    void Update()
    {
        Vector3 toTarget = (target.position - cam.transform.position).normalized;
        Vector3 camForward = cam.transform.forward;
        Vector3 camRight = cam.transform.right;

        Vector3 viewportPos = cam.WorldToViewportPoint(target.position);

        // 全ての矢印を非表示にしてから条件チェック
        leftArrow.enabled = false;
        rightArrow.enabled = false;
        upArrow.enabled = false;

        // --- 背後 or 正面 ---
        bool isBehind = viewportPos.z < 0;
        bool isInXRange = viewportPos.x >= 0 && viewportPos.x <= 1;
        bool isInYRange = viewportPos.y >= 0 && viewportPos.y <= 1;

        if (!isBehind && isInXRange && isInYRange)
        {
            // 完全に画面内なら非表示のまま
            return;
        }

        // --- 左右方向の判定 ---
        float dot = Vector3.Dot(camRight, toTarget);

        if (isBehind)
        {
            // 背後にある → 左右方向は反転（カメラから見た反対方向）
            if (dot < 0)
            {
                rightArrow.enabled = true;
            }
            else
            {
                leftArrow.enabled = true;
            }
        }
        else
        {
            // 正面だが画面外
            if (!isInXRange)
            {
                if (dot < 0)
                {
                    leftArrow.enabled = true;
                }
                else
                {
                    rightArrow.enabled = true;
                }
            }
            else if (!isInYRange && viewportPos.y > 1.0f)
            {
                upArrow.enabled = true;
            }
        }
    }
}

