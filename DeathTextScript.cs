﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;


public class DeathTextScript : MonoBehaviour {

    Text text;

	// Use this for initialization
	void Start () {
        text = GetComponent<Text>();
	}
	
	// Update is called once per frame
	void Update () {

        text.text = "Your score: " + ScoreKeeper.score;
	}
}
