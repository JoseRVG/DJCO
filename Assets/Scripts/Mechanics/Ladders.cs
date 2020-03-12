using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer.Mechanics {
    public class Ladders : MonoBehaviour {
        private PlayerController player;
        private EnemyController enemy;
        // Start is called before the first frame update
        void Start () {
            player = FindObjectOfType<PlayerController> ();
            enemy = FindObjectOfType<EnemyController> ();
        }

        // Update is called once per frame
        void OnTriggerEnter2D (Collider2D other) {
            if (other.tag == "Player") {
                player.onLadder = true;
            } else if (other.tag == "Enemy") {
                enemy.onLadder = true;
            }
        }

        void OnTriggerExit2D (Collider2D other) {
            if (other.tag == "Player") {
                player.onLadder = false;
            } else if (other.tag == "Enemy") {
                enemy.onLadder = false;
            }
        }
    }
}