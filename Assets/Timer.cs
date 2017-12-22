using System;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    int seconds;
    DateTime start = DateTime.MinValue;
    public Text timer;
    public Image timerbox;
    public Image tutorialbox;
    public Text tutorialtext;
    public Button button;
    public Button blocker;

    public bool flag1 = false;

    public static int secondsElapsed = 0;

    // Use this for initialization
    void Start()
    {
        startTiming(10 * 60);
        hideTimer();
        Button btn = button.GetComponent<Button>();
        btn.onClick.AddListener(showTimer);
    }

    // Update is called once per frame
    void Update()
    {
        updateTime();
    }

    public void startTiming(int seconds)
    {
        this.seconds = seconds;
        start = DateTime.Now;
        updateTime();
    }

    public void showTimer()
    {
        if (!flag1)
        {
            tutorialtext.text = "The planet suddenly starts vibrating, and the movement is getting more violent!\n\nYou should consider escaping it.";
            button.GetComponentInChildren<Text>().text = "Got it!";
            flag1 = !flag1;
            blocker.interactable = true;
        }
        else
        {
            button.gameObject.SetActive(false);
            tutorialtext.enabled = false;
            tutorialbox.enabled = false;
        }
    }

    public void hideTimer()
    {
        timer.enabled = false;
        timerbox.enabled = false;
    }

    void updateTime()
    {
        if (start == DateTime.MinValue) return;
        if (timer.enabled == false) return;

        DateTime now = DateTime.Now;
        TimeSpan elapsed = now - start;
        secondsElapsed = elapsed.Seconds;
        int minutesElapsed = elapsed.Minutes;
        int scalarElapsed = secondsElapsed + minutesElapsed * 60;
        int timeRemaining = seconds - scalarElapsed;

        int secondsRemaining = timeRemaining % 60;
        int minutesRemaining = timeRemaining / 60;
        string timeLeft = minutesRemaining.ToString() + ":";
        if (secondsRemaining < 10) timeLeft += '0';
        timeLeft += secondsRemaining.ToString();

        timer.text = timeLeft;

        int colorCutoff = 5 * 60;
        if (timeRemaining <= colorCutoff)
        {
            float percentRemaining = (float)timeRemaining / colorCutoff;
            if (secondsRemaining % 2 == 0)
                timer.color = Color.clear;
            else
                timer.color = new Color(1.0f, percentRemaining, percentRemaining);
        }

        if (timeRemaining <= 0)
            endGame(0);
    }

    public static void endGame(int type)
    {
        if (type == 0)
        {
            if (Tech.techStatus[2] == Tech.LEVEL.L3)
            {
                    UnityEngine.SceneManagement.SceneManager.LoadScene("fullywin", UnityEngine.SceneManagement.LoadSceneMode.Single);
            }
            else
            {
                UnityEngine.SceneManagement.SceneManager.LoadScene("lose", UnityEngine.SceneManagement.LoadSceneMode.Single);
            }
        }
        else
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("starve", UnityEngine.SceneManagement.LoadSceneMode.Single);
        }
    }
}
