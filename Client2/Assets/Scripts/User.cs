using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

//[Serializable]
public class User {

	public int connectionID;
	public int hostID;
	public int channelID;
	public User(int connectionID, int hostID, int channelID){
		this.connectionID = connectionID;
		this.hostID = hostID;
		this.channelID = channelID;
	}
}
