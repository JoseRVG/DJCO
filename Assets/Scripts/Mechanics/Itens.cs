using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer.Mechanics {
    public class Itens : MonoBehaviour {
        private PlayerController player;
        public Animator anim;
        public bool Door;
        public int ID;
        // Start is called before the first frame update
        void Start () {
            player = FindObjectOfType<PlayerController> ();
            anim = GetComponent<Animator>();
        }
        // Update is called once per frame
        void OnTriggerEnter2D (Collider2D other) {
            if (other.tag == "Player") {
                player.onDoor = true;
                if(ID==player.DoorNum){
                    player.DoorControl = true;
                    anim.SetBool("doorISopen",true);
                    
                }
                else
                {
                    player.DoorControl = false;
                }
                
            }
        }

        void OnTriggerExit2D (Collider2D other) {
            if (other.tag == "Player") {
                player.onDoor = false;
                player.DoorControl = false;
                anim.SetBool("doorISopen",false);
            }
        }
    }
}