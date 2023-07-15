using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using System;

#if UNITY_EDITOR
public class BlockCreateEditor : MonoBehaviour
{
    [SerializeField] Block baseBlock;
    [SerializeField] Block selectedBlock;
    [SerializeField] Sprite blockSprite;
    [SerializeField] Color blockColor;

    Camera mainCamera;
    HashSet<Vector2> createdCoord;
    Vector2Int[] arrowDirection = { Vector2Int.right, Vector2Int.left, Vector2Int.up, Vector2Int.down };
    private GameObject arrowObj = null;
    private Block centerBlock = null;

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


    const string ARROW_PATH = "Prefabs/Arrow";


    private bool isInUI = false;

    public void SavePrefab()
    {
        // GameObject _prefabToSave = Selection.activeGameObject;
        if (centerBlock == null) return;

        string _prefabPath = EditorUtility.SaveFilePanel("Save Prefab", "", "NewPrefab", "prefab");

        if (!string.IsNullOrEmpty(_prefabPath))
        {
            string assetPath = _prefabPath.Substring(_prefabPath.IndexOf("Assets/"));
            Block _savedBlock = PrefabUtility.SaveAsPrefabAsset(centerBlock.gameObject, assetPath).GetComponent<Block>();

            foreach (var _coord in createdCoord)
                _savedBlock.AddBlockCoords(_coord);
            EditorUtility.SetDirty(_savedBlock);
            Debug.Log("Prefab saved: " + assetPath);
        }
    }

    public void LoadPrefab()
    {
        string _loadPath = EditorUtility.OpenFilePanel("Load File", "", "prefab");

        if (string.IsNullOrEmpty(_loadPath)) return;

        string _assetPath = _loadPath.Substring(_loadPath.IndexOf("Assets/"));
        Block _block = AssetDatabase.LoadAssetAtPath<Block>(_assetPath);
        Instantiate(_block);

    }

    private void Awake()
    {
        createdCoord = new HashSet<Vector2>() { Vector2.zero };
        SelectedBlock = Instantiate(baseBlock, Vector2.zero, Quaternion.identity);
        SelectedBlock.AddBlockCoords(Vector2Int.zero);
        centerBlock = SelectedBlock;
        mainCamera = Camera.main;
        arrowObj = Instantiate(Resources.Load<GameObject>(ARROW_PATH));
        arrowObj.SetActive(false);

    }
    private void OnEnable()
    {
        BlockBuildUI.OnMouseEnterBtn += OnMouseEnterBtn;
        BlockBuildUI.OnMouseLeaveBtn += OnMouseLeaveBtn;
    }



    private void OnDisable()
    {
        BlockBuildUI.OnMouseEnterBtn -= OnMouseEnterBtn;
        BlockBuildUI.OnMouseLeaveBtn -= OnMouseLeaveBtn;
    }
    private void OnMouseEnterBtn(MouseEnterEvent @event)
    {
        isInUI = true;
        arrowObj.SetActive(false);
    }
    private void OnMouseLeaveBtn(MouseLeaveEvent @event)
    {
        isInUI = false;
    }
    private void Update()
    {
        if (mainCamera == null) return;
        if (isInUI) return;

        Vector2 _mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        if (Input.GetMouseButtonDown(1))
        {
            RaycastHit2D _hit = Physics2D.Raycast(_mousePos, _mousePos, 10);
            if (_hit)
            {
                Block _hitBlock = _hit.collider.GetComponent<Block>();
                if (_hitBlock)
                {
                    SelectedBlock = _hitBlock;
                }
            }
        }


        if (SelectedBlock != null)
        {
            Vector2 _direction = Vector2Int.RoundToInt((_mousePos - (Vector2)SelectedBlock.transform.position).normalized);
            bool _isInDirection = IsInDirection(_direction);

            if (!_isInDirection)
            {
                arrowObj.SetActive(false);
                return;
            }
            Vector2 _nextPos = Vector2Int.RoundToInt((Vector2)SelectedBlock.transform.position + _direction);
            arrowObj.transform.position = (Vector2)SelectedBlock.transform.position + _direction;
            arrowObj.transform.rotation = Quaternion.LookRotation(Vector3.forward, _direction);
            arrowObj.SetActive(true);

            if (createdCoord.Contains(_nextPos)) return;

            if (Input.GetMouseButtonDown(0))
            {
                SelectedBlock = Instantiate(baseBlock, centerBlock.transform);
                SelectedBlock.transform.position = _nextPos;
                createdCoord.Add(_nextPos);
            }
            // arrowObj.SetActive(SelectedBlock != null);
        }

    }
    private bool IsInDirection(Vector2 _dir)
    {
        if (_dir == Vector2.zero) return false;

        for (int i = 0; i < arrowDirection.Length; i++)
        {
            if (_dir == arrowDirection[i]) return true;
        }
        return false;
    }
}
#endif