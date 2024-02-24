using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    [SerializeField] private int widthBoard = 5;
    [SerializeField] private int heightBoard = 5;
    [SerializeField] private TileData[] tileData;

    [SerializeField] private int[][] boardData;
    [SerializeField] private Vector2 centerPos;
    [SerializeField] private float TileSize = 1.25f;

    [SerializeField] private Tile TilePrefabs;
    private List<Tile> listTile = new List<Tile>();
    private Dictionary<Vector2Int, Vector2Int> changePosList = new Dictionary<Vector2Int, Vector2Int>();
    private Dictionary<Vector2Int, Vector2Int> changePosListMatch = new Dictionary<Vector2Int, Vector2Int>();

    private GameManager gameManager;

    public void Setup(GameManager gameManager, LevelData levelData)
    {
        this.gameManager = gameManager;
        tileData = levelData.tileDatas;

        this.widthBoard = levelData.widthBoard;
        this.heightBoard = levelData.heightBoard;
    }


    private void SetupSize(int width, int height)
    {
        this.widthBoard = width;
        this.heightBoard = height;
    }

    public void InitBoardData()
    {
        boardData = new int[heightBoard + 2][];
        for (int i = 0; i < heightBoard + 2; i++)
        {
            boardData[i] = new int[widthBoard + 2];
        }

        for (int i = 0; i < heightBoard + 2; i++)
        {
            for (int j = 0; j < widthBoard + 2; j++)
            {
                boardData[i][j] = 0;
            }
        }

        for (int i = 0; i < heightBoard + 2; i++)
        {
            for (int j = 0; j < widthBoard + 2; j++)
            {
                if (i == 0 || j == 0 || i == heightBoard + 1 || j == widthBoard + 1)
                {
                    boardData[i][j] = -1;
                }
            }
        }
        //Assign
        SetupData();
    }

    public void InitVirtualBoard()
    {
        for (int i = 1; i < heightBoard + 1; i++)
        {
            for (int j = 1; j < widthBoard + 1; j++)
            {
                var tile = Instantiate(TilePrefabs, (Vector3)CalcPositionFromCoords(new Vector2Int(j, i)), Quaternion.identity);
                tile.Setup(0, new Vector2Int(j - 1, i - 1));
                tile.transform.parent = this.transform;
                listTile.Add(tile);
            }
        }
    }

    public void SetupData()
    {
        foreach (var coords in tileData)
        {
            boardData[coords.row][coords.column] = coords.value;
            var tile = Instantiate(TilePrefabs, (Vector3)CalcPositionFromCoords(new Vector2Int(coords.column, coords.row)), Quaternion.identity);
            tile.Setup(coords.value, new Vector2Int(coords.column, coords.row));
            listTile.Add(tile);
        }
        //PrinBoardData(boardData);
    }

    public void Reset()
    {
        foreach (var tile in listTile)
        {
            tile.DestroySelf();
        }
        listTile.Clear();
        changePosList.Clear();
        changePosListMatch.Clear();
    }

    public void SlideLeft()
    {
        for (int i = 0; i < heightBoard + 2; i++)
        {
            Dictionary<int, int> changedList = new Dictionary<int, int>();
            SlideDecreaseArr(0, boardData[i], changedList);
            foreach (var pair in changedList)
            {
                changePosList.Add(new Vector2Int(pair.Key, i), new Vector2Int(pair.Value, i));
            }
        }
        
    }

    public void SlideRight()
    {
        for (int i = 0; i < heightBoard + 2; i++)
        {
            Dictionary<int, int> changedList = new Dictionary<int, int>();
            SlideIncreaseArr(widthBoard + 1, boardData[i], changedList);
            foreach (var pair in changedList)
            {
                changePosList.Add(new Vector2Int(pair.Key, i), new Vector2Int(pair.Value, i));
            }
        }
        
    }

    public void SlideUp()
    {
        for (int i = 0; i < widthBoard + 2; i++)
        {
            Dictionary<int, int> changedList = new Dictionary<int, int>();
            int[] column = new int[heightBoard + 2];
            for (int j = 0; j < heightBoard + 2; j++)
            {
                column[j] = boardData[j][i];
            }

            SlideDecreaseArr(0, column, changedList);
            SetColumn(boardData, i, column);
            foreach (var pair in changedList)
            {
                changePosList.Add(new Vector2Int(i, pair.Key), new Vector2Int(i, pair.Value));
            }
        }
        
    }

    public void SlideDown()
    {
        
        for (int i = 0; i < widthBoard + 2; i++)
        {
            Dictionary<int, int> changedList = new Dictionary<int, int>();
            int[] column = new int[heightBoard + 2];
            for (int j = 0; j < heightBoard + 2; j++)
            {
                column[j] = boardData[j][i];
            }

            SlideIncreaseArr(heightBoard + 1, column, changedList);
            SetColumn(boardData, i, column);
            foreach (var pair in changedList)
            {
                changePosList.Add(new Vector2Int(i, pair.Key), new Vector2Int(i, pair.Value));
            }
        }
        
    }

    public void SlideDecreaseArr(int start, int[]arr, Dictionary<int, int> changedList)
    {
        var index = start + 1;
        if (start >= arr.Length - 2) return;
        for (int i = start + 1; i < arr.Length; i++)
        {
            if (arr[i] >  0)
            {
                int t = arr[i];
                arr[i] = arr[index];
                arr[index] = t;
                changedList.Add(i, index);
                index++;
            }
            if (arr[i] < 0)
            {
                SlideDecreaseArr(i, arr, changedList);
                return;
            }
        }
    }

    public void SlideIncreaseArr(int start, int[] arr, Dictionary<int, int> changedList)
    {
        var index = start - 1;
        if (start <= 1) return;
        for (int i = start - 1; i >= 0; i--)
        {
            if (arr[i] > 0)
            {
                int t = arr[i];
                arr[i] = arr[index];
                arr[index] = t;
                changedList.Add(i, index);
                index--;
            }
            if (arr[i] < 0)
            {
                SlideIncreaseArr(i, arr, changedList);
                return;
            }
        }
    }

    public void SetColumn(int[][] array2D, int colIndex, int[] column)
    {
        int rows = array2D.Length;

        for (int i = 0; i < rows; i++)
        {
            array2D[i][colIndex] = column[i];
        }
    }

    public void PrinBoardData(int[][] boardData)
    {
        string strOut = "";
        for (int i = 0; i < heightBoard + 2; i++)
        {
            for (int j = 0; j < widthBoard + 2; j++)
            {
                strOut += boardData[i][j] + " ";
            }
            strOut += "\n";
        }
        Debug.Log(strOut);
    }

    public void CheckMatch(Vector2 dir)
    {
        var needMove = 0;
        var needCheck = 0;
        var add = 1;
        if (dir == Vector2.up)
        {
            needMove = 1;
            needCheck = 2;
            add = -1;
        }
        if (dir == Vector2.down)
        {
            needMove = 2;
            needCheck = 1;
            add = 1;
        }
        if (dir == Vector2.left || dir == Vector2.right) return;
        for (int i = 1; i < heightBoard + 2; i++)
        {
            for (int j = 0; j < widthBoard + 2; j++)
            {
                if (boardData[i][j] == needMove && boardData[i + add][j] == needCheck)
                {
                    boardData[i][j] = 0;
                    boardData[i + add][j] = 1;
                    changePosListMatch.Add(new Vector2Int(j, i), new Vector2Int(j, i + add));
                }
            }
        }
    }

    public void DisplayChange()
    {
        //PrinBoardData(boardData);

        DisPlayChangePos(changePosList);
        changePosList.Clear();

        foreach (var pair in changePosListMatch)
        {
            Debug.Log("Tile At: " + pair.Key + " MoveTO: " + pair.Value);
            var tile = GetTileAt(pair.Key, listTile);
            if (tile == null)
            {
                Debug.Log("error Func GetTileAt(): " + pair.Key);
            }
            tile.Coord = pair.Value;
            tile.MoveTo(CalcPositionFromCoords(pair.Value), () =>
            {
                var tileDestroy = GetTileTypeAt(pair.Value, listTile, TileType.Cake);
                if (tileDestroy != null)
                {
                    tileDestroy.DestroySelf();
                    listTile.Remove(tileDestroy);
                    gameManager.OnMatch();
                }
            });
        }
        changePosListMatch.Clear();
    }

    private void DisPlayChangePos(Dictionary<Vector2Int, Vector2Int> listChange, Action action = null)
    {
        foreach (var pair in listChange)
        {
            var tile = GetTileAt(pair.Key, listTile);
            if (tile == null)
            {
                Debug.Log("error Func GetTileAt(): " + pair.Key);
            }
            tile.Coord = pair.Value;
            tile.MoveTo(CalcPositionFromCoords(pair.Value), action);
        }
    }

    private Tile GetTileAt(Vector2Int coord, List<Tile> listTile)
    {
        foreach (var tile in listTile)
        {
            var tileCoord = tile.Coord;
            if (tileCoord == coord) return tile;
        }
        return null;
    }

    private Tile GetTileTypeAt(Vector2Int coord, List<Tile> listTile, TileType type)
    {
        foreach (var tile in listTile)
        {
            var tileCoord = tile.Coord;
            if (tileCoord == coord && tile.Type == type) return tile;
        }
        return null;
    }

    public Vector2Int CalcCoordsFromPosition(Vector2 position)
    {
        Vector2 delta = position - centerPos + new Vector2((widthBoard) * TileSize / 2, (heightBoard) * TileSize / 2);
        return new Vector2Int(Mathf.FloorToInt(delta.x / TileSize), Mathf.FloorToInt(delta.y / TileSize));
    }

    public Vector2 CalcPositionFromCoords(Vector2Int coords)
    {
        coords = ConvertCoord(coords);
        return centerPos + new Vector2(coords.x * TileSize - (widthBoard * TileSize) / 2, 
            coords.y * TileSize - (heightBoard * TileSize) / 2)
            + new Vector2(TileSize / 2, TileSize / 2);
    }

    private Vector2Int ConvertCoord(Vector2Int coord)
    {
        coord.x -= 1;
        coord.y = heightBoard - coord.y;
        return coord;
    }
}
