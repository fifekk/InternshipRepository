using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Text;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Client : MonoBehaviour
{

    public static int connectionID;
    public int maxConnections = 10;
    public static int reliableChannelID;
    public static int hostID;
    public static int socketPort = 8888;
    public static byte error;
    public Button monopolyButton;
    public Button vrButton;
    public Button playButton;
    private MonopolyLobbyScript mlp;
    private string playerName;
    private Color playerColor;
    public Dropdown colorPicker;
    public InputField nameInput;
    public GameObject decision;
    public Text diceThrowText;
    public Text propertyText;
    private string panwName;
    public int value;
    public Button diceButton;
    public Text accountBalance;
    public static ShopScrollList scrollList;
    private ArrayList[] nieruchomosciList = new ArrayList[Player.propertyList.Count];
    public Transform contentPanel;
    public SimpleObjectPool buttonObjectPool;
    public GameObject newButton;
    public GameObject prisonText;
    public GameObject startInfo;
    public GameObject propertyInfo;
    public GameObject playerNameField;
    float lowPassFilterFactor;
    Vector3 lowPassValue;
    float accelerometerUpdateInterval = 1.0f / 60.0f;
    float lowPassKernelWidthInSeconds = 1.0f;
    float shakeDetectionThreshold = 2.0f;
    public GameObject controllMenu;
    public bool buttons = false;
    public bool accelerometer = false;
    public Button leftBtn;
    public Button rightBtn;
    public Button jumpBtn1;
    public Button jumpBtn2;

    // Use this for initialization
    void Start()
    {
        // lowPassFilterFactor = accelerometerUpdateInterval / lowPassKernelWidthInSeconds;
        // shakeDetectionThreshold *= shakeDetectionThreshold;
        // lowPassValue = Input.acceleration;


        playerColor = Color.black;
        playerName = "";


        Scene scene = SceneManager.GetActiveScene();


        if (scene.name == "Client")
        {
            NetworkTransport.Init();
            Connect();
        }
        else if (scene.name == "UI2")
        {
            playButton.interactable = false;
        }
        else if (scene.name == "UI4")
        {
            decision.SetActive(false);
        }
        else if(scene.name == "UI3")
        {
            leftBtn.interactable = false;
            rightBtn.interactable = false;
            jumpBtn1.interactable = false;
            jumpBtn2.interactable = false;
        }

    }

    // Update is called once per frame
    void Update()
    {
        float x = Input.acceleration.x;
        // Debug.Log("X POSITION: " + x);
        Vector3 acceleration = Input.acceleration;
        lowPassValue = Vector3.Lerp(lowPassValue, acceleration, lowPassFilterFactor);
        Vector3 deltaAcceleration = acceleration - lowPassValue;

        int outHostId;
        int outConnectionId;
        int outChannelId;
        byte[] buffer = new byte[1024];
        int bufferSize = 1024;
        int receiveSize;
        byte error;
        NetworkEventType evnt = NetworkTransport.Receive(out outHostId, out outConnectionId, out outChannelId, buffer, bufferSize, out receiveSize, out error);
        switch (evnt)
        {
            case NetworkEventType.ConnectEvent:
                Debug.Log("Connection succesfull");
                break;
            case NetworkEventType.DisconnectEvent:
                Debug.Log("Connected, error:" + error.ToString());
                break;
            case NetworkEventType.DataEvent:
                string receivedString = Encoding.Unicode.GetString(buffer, 0, receiveSize);
                Message msg = JsonUtility.FromJson<Message>(receivedString);
                Action action = msg.action;
                if (msg.data == "UI2")
                {
                    UnityEngine.SceneManagement.SceneManager.LoadScene("UI2");
                }
                else if (msg.data == "UI3")
                {
                    UnityEngine.SceneManagement.SceneManager.LoadScene("UI3");
                }
                else if (msg.data == "UI4")
                {
                    UnityEngine.SceneManagement.SceneManager.LoadScene("UI4");
                    Action playerMotorMoveAction = new Action(2, "playerMotorMove");
                    Message playerMotorMoveMsg = new Message(playerMotorMoveAction, "playerMotorMove");
                    SendMessage(playerMotorMoveMsg);
                }
                else if (msg.data == "SetPlayButtonActive")
                {
                    playButton.interactable = true;
                }

                else if (msg.data == "diceThrow")
                {
                    diceThrowText.text = action.message;
                    Debug.Log("WYNIK RZUTU: " + action.message);
                    //diceButton.interactable = false;
                }
                else if (msg.data == "AccountBalance")
                {
                    accountBalance.text = action.message;

                }
                else if (action.message == "PosDetection")
                {
                    StartCoroutine(buyAction(msg.data));
                    Debug.Log(msg.data);
                }
                else if (action.message == "boughtProperty" || action.message == "boughtUniversity")
                {

                    StartCoroutine(addPropAction(msg.data));
                    Debug.Log(msg.data);
                    diceButton.interactable = true;

                }
                // else if(action.message == "DeactivateRollBtn")
                // {
                //     diceButton.interactable = false;
                // }
                // else if(action.message == "ActivateRollBtn")
                // {
                //     diceButton.interactable = true;
                // }
                else if (action.message == "GoToPrison")
                {
                    prisonText.SetActive(true);
                    startInfo.SetActive(true);
                    startInfo.GetComponentInChildren<Text>().text = msg.data;
                    //diceButton.interactable = false;

                }
                else if (action.message == "GoOut")
                {
                    prisonText.SetActive(false);
                    startInfo.SetActive(false);
                    diceButton.interactable = true;
                }
                else if (action.message == "StartField")
                {
                    diceButton.interactable = true;
                    startInfo.GetComponentInChildren<Text>().text = msg.data;
                    startInfo.SetActive(true);

                }
                else if (action.message == "Altum")
                {
                    diceButton.interactable = true;
                    startInfo.SetActive(true);
                    startInfo.GetComponentInChildren<Text>().text = msg.data;

                }
                else if (action.message == "Lottery")
                {
                    diceButton.interactable = true;
                    startInfo.GetComponentInChildren<Text>().text = msg.data;
                    startInfo.SetActive(true);

                }
                else if (action.message == "Casino")
                {
                    diceButton.interactable = true;
                    startInfo.GetComponentInChildren<Text>().text = msg.data;
                    startInfo.SetActive(true);

                }
                else if (action.message == "Tax")
                {
                    startInfo.GetComponentInChildren<Text>().text = msg.data;
                    startInfo.SetActive(true);
                    diceButton.interactable = true;
                }
                else if (action.message == "TheEnd")
                {
                    startInfo.GetComponentInChildren<Text>().text = msg.data;
                    startInfo.SetActive(true);
                    //diceButton.interactable = false;
                }
                else if (action.message == "Rent")
                {
                    startInfo.GetComponentInChildren<Text>().text = msg.data;
                    startInfo.SetActive(true);
                    //diceButton.interactable = true;
                }
                else if (action.message == "Visitor")
                {
                    startInfo.GetComponentInChildren<Text>().text = msg.data;
                    startInfo.SetActive(true);
                    diceButton.interactable = true;
                }
                else if (action.message == "passingStartField")
                {
                    startInfo.SetActive(true);
                    startInfo.GetComponentInChildren<Text>().text = "Stanąłeś na polu start, ale nie dostajesz jeszcze swojej nagrody za przekrocznie :)";
                    diceButton.interactable = true;
                    Debug.Log(action.message);
                }
                else if (action.message == "haventboughtproperty" || action.message == "haventboughtuniversity")
                {
                    startInfo.SetActive(false);
                    diceButton.interactable = true;
                }
                else if (action.message == "MyField")
                {
                    startInfo.SetActive(true);
                    startInfo.GetComponentInChildren<Text>().text = "Stoisz na swoim polu!";
                    diceButton.interactable = true;
                }
                else if (action.message == "YourTurn")
                {
                    startInfo.SetActive(true);
                    startInfo.GetComponentInChildren<Text>().text = msg.data;
                }
                //Mateusz
                else if (action.message == "DisplayProperty")
                {
                    Debug.Log("cooooooo");
                    propertyInfo.SetActive(true);
                    propertyInfo.GetComponentInChildren<Text>().text = msg.data;
                }
                else if (action.message == "Death")
                {
                    controllMenu.SetActive(true);
                    reloadScene();
                    Debug.Log("You died");
                }
                Debug.Log("Otrzymano odpowiedź z serwera: " + msg);
                break;
        }
        if (accelerometer)
        {
            if (x < -0.4f)
            {
                MoveLeft("LeftMove");
            }
            else if (x > 0.4f)
            {
                MoveRight("RightMove");
            }
            else if (x > (-0.4f) && x < (0.4f))
            {
                ZeroPosition("ZeroPosition");
            }
        }
        if(buttons)
        {
            jumpBtn1.interactable = true;
            jumpBtn2.interactable = true;
            leftBtn.interactable = true;
            rightBtn.interactable = true;
        }
        // if (deltaAcceleration.sqrMagnitude >= shakeDetectionThreshold)
        // {
        //     Throw("ThrowDice");
        //     Debug.Log("Shake event detected at time " + Time.time);
        // }

    }
    private void ZeroPosition(string msg)
    {
        Action zeroPosAction = new Action(2, "ZeroPosition");
        Message zeroPostMsg = new Message(zeroPosAction, msg);
        SendMessage(zeroPostMsg);
    }

    public void Connect()
    {
        ConnectionConfig config = new ConnectionConfig();
        reliableChannelID = config.AddChannel(QosType.ReliableSequenced);
        HostTopology topology = new HostTopology(config, maxConnections);
        hostID = NetworkTransport.AddHost(topology, 0);
        Debug.Log("Socket open. Host ID is " + hostID);
        connectionID = NetworkTransport.Connect(hostID, "192.168.0.107", socketPort, 0, out error);
    }

    public void Disconnect()
    {
        NetworkTransport.Disconnect(hostID, connectionID, out error);
        Destroy(gameObject);
    }

    public void SendMessage(Message message)
    {
        string msg = JsonUtility.ToJson(message);
        byte[] buffer = Encoding.Unicode.GetBytes(msg);
        NetworkTransport.Send(hostID, connectionID, reliableChannelID, buffer, msg.Length * sizeof(char), out error);

    }
    public void SelecGameClick(int gameID)
    {
        Action action = new Action(gameID, "GameSelection");
        User user = new User(connectionID, hostID, reliableChannelID);
        //Data data = new Data(user, gameID);
        Message selectGameMessage = new Message(action, JsonUtility.ToJson(user));
        SendMessage(selectGameMessage);
    }
    public Color SetColor()
    {

        switch (colorPicker.value)
        {
            case 1:
                panwName = "houses";
                playerColor = Color.red;
                break;
            case 2:
                panwName = "crosses";
                playerColor = Color.blue;
                break;
            case 3:
                panwName = "rogalddl";
                playerColor = Color.green;
                break;
            case 4:
                panwName = "goats";
                playerColor = Color.yellow;
                break;
        }
        return playerColor;

    }
    public string SetName()
    {
        string playerName = nameInput.text;
        return playerName;
    }
    public void AddPlayer()
    {
        Action action = new Action(1, "newPlayer");
        playerColor = SetColor();
        playerName = SetName();
        Player player = new Player(connectionID, hostID, reliableChannelID, playerName, playerColor, panwName);
        //Data data = new Data(player);
        Message addPlayerMessage = new Message(action, JsonUtility.ToJson(player));

        SendMessage(addPlayerMessage);
        //SendMessage(new Message("sadasa"));
    }
    public void OnPlayButtonClick(int sceneID)
    {
        Action playButtonClickAction = new Action(sceneID, "PlayButtonWasPressed");
        Message playButtonClickMessage = new Message(playButtonClickAction, "");
        SendMessage(playButtonClickMessage);
    }
    public void Throw(string message)
    {
        Action diceThrowAction = new Action(1, message);
        Message diceThrowMessage = new Message(diceThrowAction, message);
        SendMessage(diceThrowMessage);
    }
    public void setValue(int param)
    {
        value = param;
    }
    public IEnumerator buyAction(string msg)
    {
        decision.SetActive(true);
        propertyText.text = msg;
        yield return new WaitUntil(() => value > 0);
        if (value == 1)
        {
            Action buyAction = new Action(1, "buyThisProperty");
            Message buyMessage = new Message(buyAction, "buyThisProperty");
            SendMessage(buyMessage);
        }
        if (value == 2)
        {
            Action dontBuyAction = new Action(1, "dontBuyThisProperty");
            Message dontBuyMessage = new Message(dontBuyAction, "dontBuyThisProperty");
            SendMessage(dontBuyMessage);
        }
        value = 0;
        decision.SetActive(false);
    }
    public IEnumerator addPropAction(string msg)
    {
        yield return new WaitUntil(() => msg != "");
        Player.propertyList.Add(msg);
        AddButtons();
        Player.propertyList.Clear();
    }

    public void AddButtons()
    {
        for (int i = 0; i < Player.propertyList.Count; i++)
        {

            string nieruchomosci = Player.propertyList[i];
            newButton = buttonObjectPool.GetObject();
            newButton.GetComponentInChildren<Text>().text = Player.propertyList[i];
            newButton.transform.SetParent(contentPanel, false);
            SampleButton sampleButton = newButton.GetComponent<SampleButton>();
            sampleButton.Setup(nieruchomosci, scrollList);

        }
    }

    //Mateusz
    public void DisplayProperty(Button btn)
    {
        // Debug.Log(btn);
        // Debug.Log(btn.gameObject);
        // Debug.Log(btn.GetComponentInChildren<Text>().text);
        // Debug.Log(btn.gameObject.GetComponentInChildren<Text>().text);
        string text = btn.GetComponentInChildren<Text>().text;
        //string text = newButton.GetComponentInChildren<Text>().text;


        Action displayPropertyAction = new Action(1, "DisplayProperty");

        Message DisplayMessage = new Message(displayPropertyAction, text);
        SendMessage(DisplayMessage);
    }
    public void JumpMove(string msg)
    {
        Action jumpAction = new Action(2, "Jump");
        Message jumpMsg = new Message(jumpAction, msg);
        SendMessage(jumpMsg);
    }
    public void MoveRight(string msg)
    {
        Action moveRightAction = new Action(2, "RightMove");
        Message moveRightMsg = new Message(moveRightAction, msg);
        SendMessage(moveRightMsg);
    }
    public void MoveLeft(string msg)
    {

        Action moveLeftAction = new Action(2, "LeftMove");
        Message moveLeftMsg = new Message(moveLeftAction, msg);
        SendMessage(moveLeftMsg);
    }
    public void Pause(string msg)
    {

        Action pauseAction = new Action(2, "Pause");
        Message pauseMsg = new Message(pauseAction, msg);
        SendMessage(pauseMsg);
    }
    public void StartGame(string msg)
    {
        Action startGameAction = new Action(2, "StartGame");
        Message startGameMsg = new Message(startGameAction, msg);
        SendMessage(startGameMsg);

    }
    public void HideMenu()
    {
        controllMenu.SetActive(false);
    }
    //TODO: użyć poniższych metod
    public void AccelerometerSteering()
    {
        accelerometer = true;
        // float x = Input.acceleration.x;

        // if (x < -0.5f)
        // {
        //     MoveLeft("LeftMove");
        // }
        // else if (x > 0.5f)
        // {
        //     MoveRight("RightMove");
        // }
    }
    public void KinectSteering()
    {
        Action kinectAction = new Action(2, "Kinect");
        Message kinectMsg = new Message(kinectAction, "Kinect");
        SendMessage(kinectMsg);
    }
    public void ButtonsSteering()
    {
        buttons = true;
    }
    public void reloadScene()
    {
        Action reloadSceneAction = new Action(2, "ReloadScene");
        Message reloadSceneMsg = new Message(reloadSceneAction, "ReloadScene");
        SendMessage(reloadSceneMsg);
    }

    //234 i 379 Mateusz
}