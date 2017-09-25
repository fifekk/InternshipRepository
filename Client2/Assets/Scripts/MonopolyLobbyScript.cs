using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class MonopolyLobbyScript : MonoBehaviour {
	public Dropdown colorPicker;
	public Color playerColor;
	public InputField nameInput;
	
	public Color SetColor(){
		
		switch(colorPicker.value){
			case 0:
			playerColor = Color.red;
			break;
			case 1:
			playerColor = Color.blue;
			break;
			case 2:
			playerColor = Color.green;
			break;
			case 3:
			playerColor = Color.yellow;
			break;
		}
		return playerColor;

	}
	public string SetName(){
		string playerName = nameInput.text;
		return playerName;
	}
	
	
}
