using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectLevel : MonoBehaviour
{
    [SerializeField] private LevelButton[] levelButtons;

    private void Start()
    {
        OnStart();
    }

    public void OnStart()
    {
        var levelMax = PlayerPrefs.GetInt("LevelMax", 0);
        foreach (var levelButton in levelButtons)
        {
            var level = levelButton.Level;
            if (level < levelMax)
            {
                levelButton.Passed();
            }
            if (level == levelMax)
            {
                levelButton.Unlocked();
            }
            if (level > levelMax)
            {
                levelButton.Locked();
            }
        }
    }
}
