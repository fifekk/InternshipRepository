using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class Server : MonoBehaviour
{

    int connectionID;
    int maxConnections = 10;
    int reliableChannelID;
    int hostID;
    int socketPort = 8888;
    byte error;
    bool isGameStarted = false;
    private static string moveDecision;
    public static string MoveDecision { get { return moveDecision; } }

    HashSet<Connection> connectionSet = new HashSet<Connection>();
    List<User> userList = new List<User>();
    List<Player> playerList = new List<Player>();
    public PlayerMotor playerMotor;

    // Use this for initialization
    void Start()
    {
        Config.IniWriteValue("Server", "IP", Config.IniReadValue("Server", "IP"));
        Debug.Log(Config.IniReadValue("Server", "IP"));
        Scene scene = SceneManager.GetActiveScene();
        if (scene.name == "HomeScene")
        {
            
            NetworkTransport.Init();
            ConnectionConfig config = new ConnectionConfig();
            reliableChannelID = config.AddChannel(QosType.ReliableSequenced);
            HostTopology topology = new HostTopology(config, maxConnections);
            hostID = NetworkTransport.AddHost(topology, socketPort, null);//testy na komputerze jako standalone build
            //hostID = NetworkTransport.AddWebsocketHost(topology, socketPort, null);//testy jeżeli client jest zbudowany jako WebGL
            Debug.Log("Socket open. Host ID is " + hostID);
        }
        // NetworkTransport.Init();
        // ConnectionConfig config = new ConnectionConfig();
        // reliableChannelID = config.AddChannel(QosType.ReliableSequenced);
        // HostTopology topology = new HostTopology(config, maxConnections);
        // hostID = NetworkTransport.AddHost(topology, socketPort, null);
        // //hostID = NetworkTransport.AddWebsocketHost(topology, socketPort, null);
        // Debug.Log("Socket open. Host ID is " + hostID);

        PlayerMotor.server = this;


    }

    void Update()
    {
        Scene scene = SceneManager.GetActiveScene();
        if (scene.name == "schoolRunner" && !playerMotor.isDead)
        {
            playerMotor.move();

        }

        // if(isGameStarted)
        // {
        //     Debug.Log("uwaga");
        //     playerMotor.move();
        // }
        int recHostID;
        int recConnectionID;
        int recChannelID;
        byte[] recBuffer = new byte[1024];
        int bufferSize = 1024;
        int datasize;
        NetworkEventType recNetworkEvent = NetworkTransport.Receive(out recHostID, out recConnectionID, out recChannelID, recBuffer, bufferSize, out datasize, out error);
        switch (recNetworkEvent)
        {
            case NetworkEventType.ConnectEvent:
                connectionSet.Add(new Connection(recHostID, recConnectionID, recChannelID));
                Debug.Log("Somebody connected");
                break;
            case NetworkEventType.DataEvent:
                string receivedString = Encoding.Unicode.GetString(recBuffer, 0, datasize);
                Message msg = JsonUtility.FromJson<Message>(receivedString);
                Action action = msg.action;
                if (action.message == "GameSelection")
                {
                    userList.Add(new User(recConnectionID, recHostID, recChannelID));
                    ReplyForChangeUI(action.gameID, recConnectionID);
                    UnityEngine.SceneManagement.SceneManager.LoadScene("schoolRunner");
                    PlayerMotor.PauseGame();
                    //playerMotor.move();
                }
                else if (action.message == "playerMotorMove")
                {
                    playerMotor.move();
                }
                else if (action.message == "LeftMove")
                {
                    PlayerMotor.moveDecision = "Left";
                    //playerMotor.LeftMove();

                }
                else if (action.message == "RightMove")
                {
                    PlayerMotor.moveDecision = "Right";
                    //playerMotor.RightMove();
                }
                else if (action.message == "Jump")
                {
                    PlayerMotor.moveDecision = "Jump";
                    //playerMotor.Jump();
                }
                else if (action.message == "ZeroPosition")
                {
                    PlayerMotor.moveDecision = "ZeroPosition";
                }
                else if (action.message == "Pause")
                {
                    Debug.Log("Pauza");
                    PlayerMotor.PauseGame();
                }
                else if (action.message == "StartGame")
                {

                    isGameStarted = true;

                    playerMotor.move();
                    Debug.Log("Uruchamiam grę!");


                }
                else if(action.message == "ReloadScene")
                {
                    SceneManager.LoadScene("schoolRunner");
                }

                break;
            case NetworkEventType.DisconnectEvent:
                Debug.Log("Somebody Disconnected");
                break;

        }
        // if (scene.name == "schoolRunner")
        // {
        //     if (playerMotor.isDead)
        //     {
        //         DieMsg();
        //     }
        // }

    }
    public static string GetMoveDecision()
    {
        return moveDecision;
    }
    public void ReplyForChangeUI(int gameID, int connectionID)
    {
        if (gameID == 1)
        {
            string reply = "UI2";
            Action changeUIAction = new Action(gameID, "changeUI");
            //Data changeUIData = new Data(reply);
            Message replyForChangeUI = new Message(changeUIAction, reply);
            sendToClient(replyForChangeUI, connectionID);
        }
        else if (gameID == 2)
        {
            string reply = "UI3";
            Action changeUIAction = new Action(gameID, "changeUI");
            //Data chaneUIData = new Data(reply);
            Message replyForChangeUI = new Message(changeUIAction, reply);
            sendToClient(replyForChangeUI, connectionID);
        }
        else if (gameID == 4)
        {
            string reply = "UI4";
            Action changeUIAction = new Action(gameID, "changeUI");
            Message replyForChangeUI = new Message(changeUIAction, reply);
            sendToClient(replyForChangeUI, connectionID);

        }

    }
    public void DieMsg()
    {
        Action deathAction = new Action(2, "Death");
        Message deathMsg = new Message(deathAction, "Death");
        sendToClient(deathMsg, connectionID);
    }
    public void sendToClient(Message message, int thisConnectionID)
    {
        string msg = JsonUtility.ToJson(message);
        byte[] bufferOdsylany = Encoding.Unicode.GetBytes(msg);
        Connection[] connections = new Connection[5];
        connectionSet.CopyTo(connections);
        //Connection connection = connections[0];
        Connection connection = connections.Where(s => s.connenctionID == thisConnectionID).FirstOrDefault();

        NetworkTransport.Send(connection.hostID, connection.connenctionID, connection.channelID, bufferOdsylany, msg.Length * sizeof(char), out error);
    }
}
