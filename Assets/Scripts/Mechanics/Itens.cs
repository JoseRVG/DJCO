using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer.Mechanics {
    public class Itens : MonoBehaviour {
        private PlayerController player;
        // Start is called before the first frame update
        void Start () {
            player = FindObjectOfType<PlayerController> ();
        }
        // Update is called once per frame
        void OnTriggerEnter2D (Collider2D other) {
            if (other.tag == "Player") {
                player.onDoor = true;
            }
        }

        void OnTriggerExit2D (Collider2D other) {
            if (other.tag == "Player") {
                player.onDoor = false;
            }
        }
    }
}