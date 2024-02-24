using UnityEngine;
using UnityEngine.UI;

public class LevelButton : MonoBehaviour
{
    [SerializeField] private GameObject borderPass;
    [SerializeField] private GameObject borderNotPass;
    [SerializeField] private GameObject starWin;
    [SerializeField] private GameObject starNotPass;
    [SerializeField] private Text textNumber;

    [SerializeField] private int level = 1;
    public int Level
    {
        get
        {
            return level;
        }
        set
        {

        }
    }

    public void Start()
    {
        textNumber.text = (level + 1).ToString();
    }

    public void Passed()
    {
        borderPass.SetActive(true);
        borderNotPass.SetActive(false);
        starNotPass.SetActive(false);
        starWin.SetActive(true);
    }

    public void Unlocked()
    {
        borderPass.SetActive(true);
        borderNotPass.SetActive(false);
        starNotPass.SetActive(true);
        starWin.SetActive(false);
    }

    public void Locked()
    {
        borderPass.SetActive(false);
        borderNotPass.SetActive(true);
        starNotPass.SetActive(true);
        starWin.SetActive(false);
    }

    public void SelectLevel()
    {
        var levelMax = PlayerPrefs.GetInt("LevelMax", 0);
        if (this.level > levelMax) return;
        CanvasManager.Instance.OnSelectLevel();
        GameManager.Instance.LoadLevel(level);
    }
}
