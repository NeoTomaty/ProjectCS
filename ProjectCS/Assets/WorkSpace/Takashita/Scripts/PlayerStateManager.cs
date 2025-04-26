//====================================================
// スクリプト名：PlayerStateManager
// 作成者：高下
// 内容：プレイヤーの状態を管理するクラス
// 最終更新日：04/26
// 
// [Log]
// 04/26 高下 スクリプト作成
//
//====================================================

// ******* このスクリプトの使い方 ******* //
// 1. このスクリプトはプレイヤーにアタッチする

using UnityEngine;

public class PlayerStateManager : MonoBehaviour
{
    public enum LiftingState
    {
        Normal,      // 通常状態
        LiftingPart, // リフティングパート
    }

    private LiftingState State = LiftingState.Normal;

   
    public void SetLiftingState(LiftingState state)
    {
        State = state;
    }

    public LiftingState GetLiftingState()
    {
        return State; 
    }
}
