using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Platformer.Gameplay {
    public class GameOver : MonoBehaviour {

        public Animator animator;
        public GameObject Object;
        public GameObject obj1;
        public void Exit () {
            animator.SetBool ("EndFading", true);
            Object.SetActive(true);
            obj1.SetActive(false);
        }

        public void Restart () {
            animator.SetBool ("RestartFading", true);
            Object.SetActive(true);
            obj1.SetActive(false);
        }

        public void EndFading () {
            SceneManager.LoadScene (SceneManager.GetActiveScene ().buildIndex - 1);
        }
        public void RestartFading () {
            SceneManager.LoadScene (SceneManager.GetActiveScene ().buildIndex);
        }
    }
}