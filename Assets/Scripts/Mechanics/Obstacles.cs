using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer.Mechanics {
    public class Obstacles : MonoBehaviour {
        public int ID;
        private PlayerController player;
        public GameObject obs;
        // Start is called before the first frame update
        void Start () {
            player = FindObjectOfType<PlayerController> ();
            obs.SetActive (false);
        }

        public void update () {
            if (player.ObstacleNum == ID)
                obs.SetActive (true);
        }
    }
}