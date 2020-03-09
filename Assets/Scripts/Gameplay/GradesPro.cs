using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Platformer.Mechanics;

public class GradesPro : MonoBehaviour
{

    public TextMeshProUGUI textBox;

    public PlayerController player;


    // Start is called before the first frame update
    void Start()
    {

        textBox.text = player.grades.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        textBox.text = player.grades.ToString();
    }
}