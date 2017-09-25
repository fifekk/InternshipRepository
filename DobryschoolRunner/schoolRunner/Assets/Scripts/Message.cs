using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class Message {

	
	public Action action;
	public string data;
	public Message(Action action, string data){
		this.action = action;
		this.data = data;
	}
	
}
