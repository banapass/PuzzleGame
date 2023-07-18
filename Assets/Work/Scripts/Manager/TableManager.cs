using UnityEngine;
using Utility;

public class TableManager : Singleton<TableManager>
{
    public BlockInfo[] BlockInfos { get; private set; }
    private const string BlockData_Path = "Data/BlockData";
    protected override void Awake()
    {
        base.Awake();

    }
    public void Init()
    {
        ParsingJson();
    }
    private void ParsingJson()
    {
        var _blockData = Resources.Load<TextAsset>(BlockData_Path);

        BlockInfos = JsonWrapper.FromJson<BlockInfo>(_blockData.text);
    }

}