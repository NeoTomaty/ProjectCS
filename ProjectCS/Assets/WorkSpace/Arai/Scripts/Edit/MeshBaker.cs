using UnityEditor;
using UnityEngine;

public class MeshBaker : MonoBehaviour
{
    [MenuItem("Tools/Bake Selected Mesh")]
    static void BakeMesh()
    {
        if (Selection.activeGameObject == null) return;

        var mf = Selection.activeGameObject.GetComponent<MeshFilter>();
        if (mf == null)
        {
            Debug.LogError("�I���I�u�W�F�N�g��MeshFilter������܂���");
            return;
        }

        Mesh bakedMesh = Instantiate(mf.sharedMesh);

        // Transform�̃X�P�[���𔽉f
        Vector3[] vertices = bakedMesh.vertices;
        Vector3 scale = mf.transform.lossyScale;

        for (int i = 0; i < vertices.Length; i++)
        {
            vertices[i] = Vector3.Scale(vertices[i], scale);
        }
        bakedMesh.vertices = vertices;
        bakedMesh.RecalculateBounds();
        bakedMesh.RecalculateNormals();

        // �A�Z�b�g�Ƃ��ĕۑ�
        string path = "Assets/WorkSpace/Arai/Baked_" + mf.sharedMesh.name + ".asset";
        AssetDatabase.CreateAsset(bakedMesh, path);
        AssetDatabase.SaveAssets();

        Debug.Log("�ۑ�����: " + path);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
