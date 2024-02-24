using System;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    [SerializeField] private Text showTimeText;
    private float timeLimit;

    public void Awake()
    {
    }

    public void Setup(float timeLimit)
    {
        this.timeLimit = timeLimit;
        TimeSpan time = TimeSpan.FromSeconds(timeLimit);
        showTimeText.text = time.ToString("mm':'ss");
    }

    public void UpdateTime(float currentTime)
    {
        float totalSeconds = timeLimit - currentTime;
        TimeSpan time = TimeSpan.FromSeconds(totalSeconds);
        showTimeText.text = time.ToString("mm':'ss");
    }
}
