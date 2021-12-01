using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TimerController : MonoBehaviour
{
    public float timeRemaining;
    public bool timerIsRunning = false;
    public bool HasBeenPressed = false;
    public bool timeHasRunOut = false;
    public Text timeText;
    public GameObject LoseText;
    public GameObject WinText;
    public GameObject StartTimerText;

    private RubyController rubyController;

    private void Start()
    {
        // Starts the timer automatically
        timerIsRunning = true;
        StartTimerText.gameObject.SetActive(true);
        timeText.GetComponent<Text>().enabled = false;

        GameObject rubyControllerObject = GameObject.FindWithTag("rubyController");
        rubyController = rubyControllerObject.GetComponent<RubyController>();

    }

    void Update()
    {

        if (timerIsRunning)
        {
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;
                DisplayTime(timeRemaining);
            }
            else
            {
                timeRemaining = 0;
                timerIsRunning = false;
                timeHasRunOut = true;

                if (HasBeenPressed == true)
                {
                rubyController.gameOver = true;
                rubyController.LoseText.gameObject.SetActive(true);
                rubyController.BackgroundMusic.gameObject.SetActive(false);
                rubyController.speed = 0.0f;
                }
                else
                {

                }
            }
        
        if(Input.GetKeyDown(KeyCode.T))
        {
            if(HasBeenPressed == false)
            {
                timeText.GetComponent<Text>().enabled = true;
                StartTimerText.gameObject.SetActive(false);
                timerIsRunning = true;
                timeRemaining = 30;
                HasBeenPressed = true;
            }
            else
            {
                
            }
        }

        if(LoseText.activeSelf == true)
        {
            timerIsRunning = false;
        }

        if(WinText.activeSelf == true)
        {
            timerIsRunning = false;
        }

        }
    }

    void DisplayTime(float timeToDisplay)
    {
        timeToDisplay += 1;

        float minutes = Mathf.FloorToInt(timeToDisplay / 60); 
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);

        timeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}