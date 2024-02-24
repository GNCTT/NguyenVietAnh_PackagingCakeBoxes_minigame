using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/LevelDataScriptableObject", order = 1)]
public class LevelData : ScriptableObject
{
    [SerializeField] public int widthBoard;
    [SerializeField] public int heightBoard;

    [SerializeField] public TileData[] tileDatas;
    [SerializeField] public int cakeNumber;
}
