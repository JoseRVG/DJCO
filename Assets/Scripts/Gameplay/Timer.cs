using System.Collections;
using System.Collections.Generic;
using Platformer.Core;
using Platformer.Mechanics;
using Platformer.Model;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Platformer.Gameplay {
    public class Timer : MonoBehaviour {
        public float timeStart = 60;
        public float startTimer = 0;
        public string timerBackup;
        public TextMeshProUGUI textBox;
        public Transform TimerBar;
        public GameObject obj;
        public GameObject objColor;
        public bool tictocflag;
        public bool startgame = true;
        AudioSource audioData;
        private PlayerController player;
        private Color color;
        public AudioClip tic;
        public AudioClip toc;
        public AudioClip tictoc;
        Vector3 temp ;
        public Health health;
        PlatformerModel model = Simulation.GetModel<PlatformerModel> ();
        /// <summary>
        /// Timer component marks the time left to deliver the grades
        /// </summary>

        // Start is called before the first frame update
        void Start () {
            temp = new Vector3((Screen.width / 2) - (obj.GetComponent<RectTransform>().sizeDelta.x / 2), (Screen.height / 2) - (obj.GetComponent<RectTransform>().sizeDelta.y / 2), 0);
            textBox.text = timeStart.ToString ();
            TimerBar.GetComponent<Image> ().fillAmount = 0;
            tictocflag = true;
            audioData = GetComponent<AudioSource> ();
            player = FindObjectOfType<PlayerController> ();
            obj.transform.position = temp;
            obj.transform.localScale = new Vector3 (6.0f, 6.0f, 3.0f);
        }

        // Update is called once per frame
        void Update () {
            if (timeStart < 10)
            { objColor.GetComponent<Image> ().color = Color.red;}
            else {
                ColorUtility.TryParseHtmlString("#3BC8C1", out color);
                objColor.GetComponent<Image> ().color = color;
            }
            if (startgame) {
                if (startTimer <= 4) {
                    startTimer += Time.deltaTime;
                    if (startTimer < 3)
                        textBox.text = Mathf.Round (startTimer).ToString ();
                    else
                        textBox.text = "GO";
                    TimerBar.GetComponent<Image> ().fillAmount += Time.deltaTime;
                    if (TimerBar.GetComponent<Image> ().fillAmount >= 1) {
                        audioData.clip = tictoc;
                        audioData.Play ();
                        TimerBar.GetComponent<Image> ().fillAmount--;
                        if (timeStart <= 10) {
                            if (tictocflag) {
                                audioData.clip = tic;
                                audioData.Play ();
                                tictocflag = !tictocflag;
                            } else {
                                audioData.clip = toc;
                                audioData.Play ();
                                tictocflag = !tictocflag;
                            }
                        }
                    }
                } else {
                    startgame = false;
                    var player = model.player;
                    temp = new Vector3(((Screen.width * 29/30)- obj.GetComponent<RectTransform>().sizeDelta.x ), ((Screen.height * 29 / 30) - obj.GetComponent<RectTransform>().sizeDelta.y), 0);
                    obj.transform.position = temp;
                    obj.transform.localScale = new Vector3 (2.0f, 2.0f, 1.0f);
                    player.controlEnabled = true;
                }
            } else if (timeStart > 0) {
                timeStart -= Time.deltaTime;
                textBox.text = Mathf.Round (timeStart).ToString ();
                timerBackup = Mathf.Round (timeStart).ToString ();
                TimerBar.GetComponent<Image> ().fillAmount += Time.deltaTime;
                if (TimerBar.GetComponent<Image> ().fillAmount >= 1) {
                    audioData.clip = tictoc;
                    audioData.Play ();
                    TimerBar.GetComponent<Image> ().fillAmount--;
                    if (timeStart <= 5) {
                        if (tictocflag) {
                            audioData.clip = player.audioTimerRunningOut;
                            audioData.Play ();
                            tictocflag = !tictocflag;
                        } else {
                            audioData.clip = player.audioTimerRunningOut;
                            audioData.Play ();
                            tictocflag = !tictocflag;
                        }
                    }
                }
            } else {
                textBox.text = timerBackup;
                if (player.grades > 0) {
                    health.EndGame ();
                    var player = model.player;
                    player.controlEnabled = false;
                }
            }
        }
    }
}