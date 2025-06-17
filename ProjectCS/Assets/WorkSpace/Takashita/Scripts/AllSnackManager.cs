//====================================================
// スクリプト名：AllSnackManager
// 作成者：高下
// 内容：ステージ上のすべてのスナックを管理するスクリプト
// 最終更新日：06/16
// 
// [Log]
// 06/16 高下 スクリプト作成
//====================================================
using System.Collections.Generic;
using UnityEngine;

public class AllSnackManager : MonoBehaviour
{
    public class SnackData
    {
        public GameObject Snack;
        public GameObject LiftingArea;
        public FallPointCalculator FPC;
        public float DistanceToGround;

        public SnackData(GameObject snack, GameObject liftingArea)
        {
            Snack = snack;
            LiftingArea = liftingArea;
            FPC = snack.GetComponent<FallPointCalculator>();
            DistanceToGround = 0f;
        }
    }


    private List<SnackData> SnackList = new List<SnackData>();


    private void Start()
    {

    }

    private void Update()
    {
        foreach (var data in SnackList)
        {
            data.DistanceToGround = data.FPC.GetDistanceToGround();
        }
    }

    public void AddSnackData(GameObject snack, GameObject liftingArea)
    {
        SnackList.Add(new SnackData(snack, liftingArea));
    }

    public GameObject GetClosestSnackToGround()
    {
        SnackData closest = null;
        float minDistance = float.MaxValue;

        foreach (var data in SnackList)
        {
            if (data.DistanceToGround < minDistance)
            {
                minDistance = data.DistanceToGround;
                closest = data;
            }
        }

        return closest?.Snack;
    }
}
