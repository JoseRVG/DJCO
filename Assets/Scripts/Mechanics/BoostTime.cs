using System.Collections;
using System.Collections.Generic;
using Platformer.Gameplay;
using UnityEngine;

namespace Platformer.Mechanics {
    public class BoostTime : MonoBehaviour {
        private PlayerController player;
        public Timer time;
        public GameObject Boost;
        // Start is called before the first frame update
        void Start () {
            player = FindObjectOfType<PlayerController> ();
            Boost.SetActive (true);
        }
        // Update is called once per frame
        void OnTriggerEnter2D (Collider2D other) {
            if (other.tag == "Player") {
                time.timeStart += 15;
                player.boostTimer = true;
                Boost.SetActive (false);
            }
        }
    }
}