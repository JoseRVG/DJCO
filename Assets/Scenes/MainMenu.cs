﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {
    public Animator animator;
    public int Lvl;
    public GameObject obj1;
    public GameObject obj2;

    public void Exit () {
        Application.Quit ();
    }

    public void FadeTolvl () {
        animator.SetBool ("Start", true);
        obj1.SetActive(false);
        obj2.SetActive(false);
    }

    public void OnFadeComplete () {
        SceneManager.LoadScene (Lvl);
    }
}