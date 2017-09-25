using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]

public class Message {

	//public Player player;
	//public Action action;
	public Action action;
	public string data;
	public Message(Action action, string data){
		this.action = action;
		this.data = data;
	}
	// public Message(string action, string player){
	// 	this.action = action;
	// 	this.player = player;
	// }
}
