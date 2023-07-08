using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class BlockCreateEditor : MonoBehaviour
{
    [SerializeField] Block baseBlock;
    [SerializeField] Block selectedBlock;
    [SerializeField] Sprite blockSprite;
    [SerializeField] Color blockColor;


    public Block SelectedBlock
    {
        get { return selectedBlock; }
        set
        {
            selectedBlock = value;
            if (selectedBlock != null)
            {

            }
            else
            {

            }
        }
    }

    HashSet<Vector2Int> createdCoord;

    [MenuItem("Block/Save Block")]
    private static void SavePrefab()
    {
        GameObject _prefabToSave = Selection.activeGameObject;
        if (_prefabToSave == null) return;

        string _prefabPath = EditorUtility.SaveFilePanel("Save Prefab", "", "NewPrefab", "prefab");

        if (!string.IsNullOrEmpty(_prefabPath))
        {
            string assetPath = _prefabPath.Substring(_prefabPath.IndexOf("Assets/"));
            PrefabUtility.SaveAsPrefabAsset(_prefabToSave, assetPath);
            Debug.Log("Prefab saved: " + assetPath);
        }
    }

    [MenuItem("Block/Load Block")]
    private static void LoadPrefab()
    {
        string _loadPath = EditorUtility.OpenFilePanel("Load File", "", "prefab");

        if (string.IsNullOrEmpty(_loadPath)) return;

        string _assetPath = _loadPath.Substring(_loadPath.IndexOf("Assets/"));
        Block _block = AssetDatabase.LoadAssetAtPath<Block>(_assetPath);
        Instantiate(_block);

    }

    private void Awake()
    {
        createdCoord = new HashSet<Vector2Int>() { Vector2Int.zero };
        SelectedBlock = Instantiate(baseBlock);

    }
}
