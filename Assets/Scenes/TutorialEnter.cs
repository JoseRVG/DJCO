using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialEnter : MonoBehaviour {
    public GameObject obj1;
    public Animator animator;
    public void OnFadeComplete () {
        obj1.SetActive (false);
        SceneManager.LoadScene (SceneManager.GetActiveScene ().buildIndex);
    }
    public void RestartFading () {
        obj1.SetActive (true);
    }

    public void Restart () {
        animator.SetBool ("Start", true);
        obj1.SetActive (true);
    }

    public void Fading () {
        obj1.SetActive (false);
    }
}