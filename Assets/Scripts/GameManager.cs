using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] Board board;
    [SerializeField] InputManager inputManager;

    [SerializeField] private int cakeNumber = 1;
    private int cakeMatched;

    [SerializeField] private Timer timer;
    [SerializeField] private float timeLimit = 45f;
    private float currentTime = 0f;

    private bool isOver;

    [SerializeField] PopupManager popupManager;

    [SerializeField] private LevelData[] levelDatas;

    private int currentLevel;
    public int levelMax;

    public static GameManager Instance;

    private void Start()
    {
        Instance = this;
        currentLevel = 0;
        levelMax = PlayerPrefs.GetInt("LevelMax", 0);
        //Setup();
    }

    public void Setup()
    {
        if (currentLevel >= levelDatas.Length)
        {
            CanvasManager.Instance.OnBackHome();
            return;
        }
        var levelData = levelDatas[currentLevel];
        board.Setup(this, levelData);
        board.InitBoardData();
        board.InitVirtualBoard();
        this.cakeNumber = levelData.cakeNumber;
        cakeMatched = 0;
        isOver = false;
        inputManager.Enable = true;

        timer.Setup(timeLimit);
        currentTime = 0;

        StartCoroutine(CalculorTime());
    }

    public void OnMatch()
    {
        cakeMatched++;
        if (cakeMatched == cakeNumber)
        {
            OnGameWin();
        }
    }

    private void OnGameWin()
    {
        isOver = true;
        Debug.Log("Win");
        popupManager.ShowPopupWin();
        inputManager.Enable = false;
        if (currentLevel == levelMax)
        {
            levelMax++;
            PlayerPrefs.SetInt("LevelMax", levelMax);
        }
    }

    private void OnGameLose()
    {
        isOver = true;
        Debug.Log("Lose");
        popupManager.ShowPopupLose();
        inputManager.Enable = false;
    }

    public void OnGameReset()
    {
        StopAllCoroutines();
        Reset();
        Setup();
    }

    public void OnGameNext()
    {
        currentLevel++;
        OnGameReset();

    }

    public void LoadLevel(int level)
    {
        Debug.Log("Load Level: " + level);
        this.currentLevel = level;
        OnGameReset();
    }

    private void OnGamePause()
    {

    }

    private void Reset()
    {
        board.Reset();
    }


    private IEnumerator CalculorTime()
    {
        while (currentTime < timeLimit && !isOver)
        {
            yield return new WaitForSeconds(1f);
            currentTime += 1f;
            timer.UpdateTime(currentTime);
        }
        if (currentTime == timeLimit)
        {
            OnGameLose();
        }
    }

}
