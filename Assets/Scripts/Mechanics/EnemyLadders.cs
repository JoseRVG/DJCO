using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer.Mechanics {
    public class EnemyLadders : MonoBehaviour {
        private EnemyController enemy;
        // Start is called before the first frame update
        void Start () {
            enemy = FindObjectOfType<EnemyController> ();
        }

        // Update is called once per frame
        void OnTriggerEnter2D (Collider2D other) {
            if (other.tag == "Enemy") {
                enemy.onLadder = true;
            }
        }

        void OnTriggerExit2D (Collider2D other) {
            if (other.tag == "Enemy") {
                enemy.onLadder = false;
            }
        }
    }
}