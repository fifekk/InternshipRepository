using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class Connection {

	public Connection(int host, int connection, int channel)
    {
        this.hostID = host;
        this.connenctionID = connection;
        this.channelID = channel;
    }
    public int hostID;
    public int connenctionID;
    public int channelID;

}