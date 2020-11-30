using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Wysiwyg.Common;
using Wysiwyg.UI;

public class MoonShotResultUI : MonoBehaviour
{
    private GameObject gameUI;
    private TMP_Text detail;

    void Start()
    {
        gameUI = GameObject.Find("GameUI");
        detail = GameObject.Find("Detail").GetComponent<TMP_Text>();
        Hide();
    }

    public void Hide()
    {
        QUI.Show(gameUI);
        QUI.Hide(this.gameObject);
    }

    public void Show(BallMonitor ball, bool isSuccess)
    {
        QUI.Show(this.gameObject);
        QUI.Hide(gameUI);

        if (isSuccess)
        {
            detail.text = $"{ball}";
            QUI.SetText("Title", "Moon Run!");
            QUI.SetButton("Restart", "Next Level", () =>
            {
                MoonShot.NextLevel();
                Hide();
                MoonShot.Reset();
            });

        }
        else
        {
            detail.text = $"{ball}";
            QUI.SetText("Title", "Moon Out!");
            QUI.SetButton("Restart", "Retry", () =>
            {
                Hide();
                MoonShot.Reset();
            });

        } //FURTHER TODO: moon foul

        Debug.Log($"result {ball}");
        ball.Reset();

    }
}