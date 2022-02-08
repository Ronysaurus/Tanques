﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    public Text TScore;

    private bool menu = true;

    void Start()
    {
        Time.timeScale = 0;
        TScore.fontSize = 20;
        TScore.text = "Highscore: " + PlayerPrefs.GetInt("highscore", 0);
    }
    //-----------------------------------------------------------------------------------------------
    void Update()
    {
        if (Input.GetKey(KeyCode.Space) && menu)
        {
            Time.timeScale = 1;
            Destroy(GameObject.Find("Menu/CameraMenu").gameObject);
            //Poner la puntuacion en 0
            TScore.fontSize = 26;
            TScore.text = "0";
            menu = false;
        }
    }
}
     