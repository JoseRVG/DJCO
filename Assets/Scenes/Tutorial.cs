using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Tutorial : MonoBehaviour {
    public Animator animator;
    public GameObject obj1;
    public GameObject obj2;
    public GameObject obj3;
    public GameObject obj4;

    public void FadeTolvl () {
        animator.SetBool ("Start", true);

    }
    public void OnFadeComplete () {
        obj1.SetActive (false);
        obj2.SetActive (true);
        obj3.SetActive (true);
        obj4.SetActive (false);
    }

    public void RestartFading () {
        SceneManager.LoadScene (SceneManager.GetActiveScene ().buildIndex);
        obj4.SetActive (false);
    }
}