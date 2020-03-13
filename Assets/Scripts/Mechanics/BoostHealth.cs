﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer.Mechanics {
    public class BoostHealth : MonoBehaviour {
        private PlayerController player;
        public GameObject Boost;
        // Start is called before the first frame update
        void Start () {
            player = FindObjectOfType<PlayerController> ();
            Boost.SetActive(true);
        }
        // Update is called once per frame
        void OnTriggerEnter2D (Collider2D other) {
            if (other.tag == "Player") {
                player.health.currentHP += 15;
                Boost.SetActive(false);
            }
        }
    }
}