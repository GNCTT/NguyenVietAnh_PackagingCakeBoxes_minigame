using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasManager : MonoBehaviour
{
    [SerializeField] private GameObject openScene;
    [SerializeField] private SelectLevel selectLevelScene;

    public static CanvasManager Instance;

    public void Start()
    {
        Instance = this;
    }

    public void OnClickPlayBtn()
    {
        openScene.SetActive(false);
    }

    public void OnSelectLevel()
    {
        selectLevelScene.gameObject.SetActive(false);
    }

    public void OnBackHome()
    {
        openScene.SetActive(true);
        selectLevelScene.OnStart();
        selectLevelScene.gameObject.SetActive(true);
    }
}
