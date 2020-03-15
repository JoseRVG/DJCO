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
        public string timerBackup;
        public TextMeshProUGUI textBox;
        public Transform TimerBar;
        public bool tictocflag;
        AudioSource audioData;
        private PlayerController player;
        public AudioClip tic;
        public AudioClip toc;
        public AudioClip tictoc;

        public Health health;

        // Start is called before the first frame update
        void Start () {
            textBox.text = timeStart.ToString ();
            TimerBar.GetComponent<Image> ().fillAmount = 0;
            tictocflag = true;
            audioData = GetComponent<AudioSource> ();
            player = FindObjectOfType<PlayerController> ();
        }

        // Update is called once per frame
        void Update () {
            if (timeStart > 0) {
                timeStart -= Time.deltaTime;
                textBox.text = Mathf.Round (timeStart).ToString ();
                timerBackup = Mathf.Round (timeStart).ToString ();
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
                textBox.text = timerBackup;
                if (player.grades > 0) {
                    health.EndGame ();
                    PlatformerModel model = Simulation.GetModel<PlatformerModel> ();
                    var player = model.player;
                    player.controlEnabled = false;
                }
            }
        }
    }
}