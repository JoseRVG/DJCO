using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer.Mechanics {
    public class Itens : MonoBehaviour {
        private PlayerController player;
        public Animator anim;
        public BoxCollider2D boxCol;
        // Start is called before the first frame update
        void Start () {
            player = FindObjectOfType<PlayerController> ();
            boxCol.enabled = Random.value > 0.5f;
            anim = GetComponent<Animator>();
        }
        // Update is called once per frame
        void OnTriggerEnter2D (Collider2D other) {
            if (other.tag == "Player") {
                player.onDoor = ;
                //anim.SetBool("doorISopen",true);
            }
        }

        void OnTriggerExit2D (Collider2D other) {
            if (other.tag == "Player") {
                player.onDoor = 0;
                anim.SetBool("doorISopen",false);
            }
        }
    }
}