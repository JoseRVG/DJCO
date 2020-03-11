using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Platformer.Gameplay {
    public class GameOver : MonoBehaviour {
        public void Exit () {
            Debug.Log ("Aplication Exit");
            Application.Quit ();
        }

        public void Restart () {
            Debug.Log ("Aplication Restart");
            SceneManager.LoadScene (SceneManager.GetActiveScene ().buildIndex);
        }
    }
}