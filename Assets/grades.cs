﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class grades : MonoBehaviour
{
    public int startingGrades = 6;
    public Text textBox;
    // Start is called before the first frame update
    void Start()
    {
        textBox.text = startingGrades.ToString();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
