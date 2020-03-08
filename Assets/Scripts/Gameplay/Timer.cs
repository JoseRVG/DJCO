using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Platformer.Mechanics;
using TMPro;

public class Timer : MonoBehaviour {
    public float timeStart = 10;
    public TextMeshProUGUI textBox;
    public Transform TimerBar;
    public bool tictocflag;
    AudioSource audioData;

    public AudioClip tic;
    public AudioClip toc;
    public AudioClip tictoc;

    private Health health;


    // Start is called before the first frame update
    void Start () {
        textBox.text = timeStart.ToString ();
        TimerBar.GetComponent<Image>().fillAmount =0;
        tictocflag = true;
        audioData = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update () {
        if (timeStart > 0)
        {
            timeStart -= Time.deltaTime;
            textBox.text = Mathf.Round(timeStart).ToString();
            TimerBar.GetComponent<Image>().fillAmount += Time.deltaTime;
            if (TimerBar.GetComponent<Image>().fillAmount >= 1)
            {
                audioData.clip = tictoc;
                audioData.Play();
                TimerBar.GetComponent<Image>().fillAmount--;
                if (timeStart <= 10)
                {
                    if (tictocflag)
                    {
                        audioData.clip = tic;
                        audioData.Play();
                        tictocflag = !tictocflag;
                    }
                    else
                    {
                        audioData.clip = toc;
                        audioData.Play();
                        tictocflag = !tictocflag;
                    }
                }
            }
        }
        else
        {
            textBox.text = "0";
            health.EndGame();
        }
    }
}