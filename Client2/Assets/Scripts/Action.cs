using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Action {

	public int gameID;
	public string message;

	public Action(int gameID, string message){
		this.gameID = gameID;
		this.message = message;
	}
}
