using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameCanvas : MonoBehaviour 
{
	public Text pointsTextBox;

	// Use this for initialization
	void Start () 
	{
		OnPointsChange();
		GameController.GetController<ConfigurationController>().AddListenerToSettingChanged(SingleGameStats.Points, OnPointsChange);
	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}

	void OnPointsChange()
	{
		pointsTextBox.text = "Points: " + GameController.GetController<ConfigurationController>().GetSetting(SingleGameStats.Points).GetValueAsLong();
	}
}
