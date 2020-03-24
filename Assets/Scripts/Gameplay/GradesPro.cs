using System.Collections;
using System.Collections.Generic;
using Platformer.Mechanics;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GradesPro : MonoBehaviour {

    public TextMeshProUGUI textBox;
    public PlayerController player;

    /// <summary>
    /// Show Grades component marks the grades left to deliver
    /// </summary>

    // Start is called before the first frame update
    void Start () {

        textBox.text = player.grades.ToString ();
    }

    // Update is called once per frame
    void Update () {
        textBox.text = player.grades.ToString ();
    }
}