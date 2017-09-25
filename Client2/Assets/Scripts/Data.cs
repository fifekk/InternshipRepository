using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]

public class Data {

	public User user;
	public string uiTag;
	public Player player;
	public Data(User user, string uiTag){
		this.user = user;
		this.uiTag = uiTag;
	}
	public Data(Player player){
		this.player = player;
	}
}
