using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupManager : MonoBehaviour
{
    [SerializeField] private GameObject FadeImage;

    [SerializeField] private GameObject PopupWin;
    [SerializeField] private GameObject PopupLose;


    public void ShowPopupWin()
    {
        FadeImage.SetActive(true);
        PopupWin.SetActive(true);
        PopupWin.transform.localScale = new Vector3(0f, 0f, 0f);
        PopupWin.transform.DOScale(1.5f, .2f).SetEase(Ease.InCirc);
    }

    public void ShowPopupLose()
    {
        FadeImage.SetActive(true);
        PopupLose.SetActive(true);
        PopupLose.transform.localScale = new Vector3(0f, 0f, 0f);
        PopupLose.transform.DOScale(1.5f, .2f).SetEase(Ease.InCirc);
    }

    public void HidePopup()
    {
        FadeImage.SetActive(false);
        PopupWin.SetActive(false);
        PopupLose.SetActive(false);
    }
}
