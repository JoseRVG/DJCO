using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Platformer.Mechanics;

public class nextroom : MonoBehaviour
{
    public Text textBox;
    public PlayerController player;


    // Start is called before the first frame update
    void Start()
    {

        textBox.text = player.DoorNum.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        textBox.text = player.DoorNum.ToString();
    }
}
