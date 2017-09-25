using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
//[Serializable]
public class Player : User {
	

    public string name;
	public Color playerColor;
	public string pawnName;
	public static List<string> propertyList = new List<string>();

    public Player(int connectionID,int hostID,int channelID,string playerName,Color playerColor,string pawnName): base(connectionID, hostID, channelID) {
		this.connectionID = connectionID;
		this.hostID = hostID;
		this.channelID = channelID;
		this.name = playerName;
		this.playerColor = playerColor;
		this.pawnName = pawnName;

	}
}
