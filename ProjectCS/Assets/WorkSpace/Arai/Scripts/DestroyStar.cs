//======================================================
// [DestroyStar]
// 作成者：荒井修
// 最終更新日：05/12
// 
// [Log]
// 05/12　荒井　他のオブジェクトがぶつかってきたら自分を消滅させるように実装
// 05/12　荒井　エフェクトを出すように実装
//======================================================
using UnityEngine;

public class DestroyStar : MonoBehaviour
{
    [SerializeField] private GameObject Effect; // エフェクトオブジェクト

    private void OnTriggerEnter(Collider other)
    {
        // エフェクトを出す
        if (Effect != null)
        {
            GameObject EffectInstance = Instantiate(Effect, transform.position, Quaternion.Euler(-90f, 0f, 0f));
            Destroy(EffectInstance, EffectInstance.GetComponent<ParticleSystem>().main.duration);   // エフェクトの継続時間が終わったらに消す
        }

        Destroy(gameObject);
    }
}
