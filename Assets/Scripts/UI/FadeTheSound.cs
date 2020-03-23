using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeTheSound : MonoBehaviour
{
    public AudioSource myFx;

    public void loadLevel () {
        print("hi");
        StartCoroutine (FadeSound.StartFade (myFx, 1.328f, 0f));
    }
}
