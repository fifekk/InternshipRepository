using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class PlayerMotor : MonoBehaviour
{

    int connectionID;
    int maxConnections = 10;
    int reliableChannelID;
    int hostID;
    int socketPort = 8888;
    byte error;
    HashSet<Connection> connectionSet = new HashSet<Connection>();
    List<User> userList = new List<User>();
    List<Player> playerList = new List<Player>();
    private CharacterController controller;
    public static Vector3 moveVector;
    private float speed = 5.0f;
    private float gravity = 140.0f;
    public bool isDead = false;
    private float positionX;
    private float jumpSpeed = 150.0f;
    private float directionVector = 0.955f;
    private bool directionChange = false;
    private float timer = 0.0f;
    private Light lt;
    private bool inTheClub = false;
    public float timer2 = 1.0f;
    public Text mainText;
    public Text mainText2;
    private int life = 3;
    public Text lifeText;
    public static Server server;
    public static string moveDecision;
    public float turnSpeed=3.0f; //Asia - najgorzej @Filip
    public  bool moveRight;
    public  bool moveLeft;
    public bool jump;

    void Start()
    {
        // Server.playerMotor = this;
        positionX = 1.5f;
        controller = GetComponent<CharacterController>();
        lt = GetComponent<Light>();
    }
    void Update()
    {
        lifeText.text = ("O " + (int)life).ToString();
        //move();
        //change direction
        changeDirection();
        // change Light
        changeLight();
    }
    public void changeLight()
    {
        if (inTheClub)
        {
            if (timer2 % 0.1 > 0.05)
            {
                lt.range = 15.0F;
            }
            else
            {
                lt.range = 0.0f;
            }
            time2();

            if (timer2 < 0.0f)
            {

                inTheClub = false;
                timer2 = 1.0f;
            }

        }
        else if (!inTheClub)
        {
            lt.range = 30.0f;
        }

    }
    public void changeDirection()
    {
        if (directionChange)
        {
            mainText.text = "Nie pij tyle!!!";
            time();
            //Debug.Log("Timer1: " + timer);
            if (timer > 5.0f)
            {
                directionChange = false;
            }
        }
        else if (!directionChange)
        {
            mainText.text = "";
            timer = 0.0f;
        }
    }
    public void setSpeed(float modifier)
    {
        speed = 5.0f + modifier;
    }
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.tag == "die")
        {
            life -= 1;
            if (life > 0)
            {
                Destroy(hit.gameObject);
            }
            else if (life == 0)
            {
                mainText2.text = "Koniec gry!";
                death();
            }
        }

        if (hit.gameObject.tag == "book")
        {
            if (GetComponent<Score>().countPoints() < 10)
            {

                Destroy(hit.gameObject);
                GetComponent<Score>().substractPoints(GetComponent<Score>().countPoints());
            }
            else
            {
                Destroy(hit.gameObject);
                GetComponent<Score>().substractPoints(10.0f);
            }
        }
        if (hit.gameObject.tag == "club")
        {
            inTheClub = true;
            Destroy(hit.gameObject);
        }
        if (hit.gameObject.tag == "vodka")
        {
            directionChange = true;
            Destroy(hit.gameObject);
        }
    }

    private void death()
    {
        isDead = true;
        server.DieMsg();
        GetComponent<Score>().onDeath();
        

    }
    private float time()
    {
        timer += Time.deltaTime;
        return timer;
    }
    private float time2()
    {
        timer2 -= Time.deltaTime;
        return timer2;
    }
    public void Jump()
    {
        // Jump
        // if (controller.isGrounded && (Input.GetKeyDown(KeyCode.W) || Server.GetMoveDecision() == "Jump" ))
        // {
            if(controller.isGrounded && moveDecision == "Jump"){
                moveVector.y = jumpSpeed;
            }
        //}
        
        
    }
    public void LeftMove(){
        if((directionChange && moveDecision == "Left") || (directionChange && moveLeft)){
            // if (Input.GetKeyDown(KeyCode.A))
            // {
                if (positionX < 2)
                {
                    positionX += directionVector;
                }
            //}
        }
        else if((!directionChange && moveDecision == "Left") || (!directionChange && moveLeft)){
             // if (Input.GetKeyDown(KeyCode.A))
            // {
                if (positionX > 1)
                {
                    positionX -= directionVector;
                }
            //}
        }
        
    }

    public static void PauseGame()
    {
        if(Time.timeScale ==1){
            Time.timeScale = 0;
        }else{
            Time.timeScale = 1;
        }
    }

    public void ZeroPosition()
    {
        if(moveDecision == "ZeroPosition"){
            Debug.Log("ELO ELO JESTEM ZERO");
            positionX = 1.5f;
            
        }
    }
    public void RightMove(){
        if((directionChange && moveDecision == "Right") || (directionChange && moveRight)){
            // if (Input.GetKeyDown(KeyCode.D))
            // {
                if (positionX > 1)
                {
                    positionX -= directionVector;
                }
            //}
        }
        else if((!directionChange && moveDecision == "Right") || (!directionChange && moveRight)){
           
            // if (Input.GetKeyDown(KeyCode.D))
            // {
                if (positionX < 2)
                {
                    positionX += directionVector;
                }
            //}
        }
    }
    public void move()
    {
        if (isDead)
        {
            return;
        }
            
        moveVector = Vector3.zero;
        // Forward 
        moveVector.z = speed;
        //Jump
        Jump();
        moveVector.y -= gravity * Time.deltaTime;
        controller.Move(moveVector * Time.deltaTime);
        //LeftAndRightMove();
        LeftMove();
        RightMove();
        ZeroPosition();
        Debug.Log(directionChange);
        moveDecision = "";
        float step = setTurnSpeed()*Time.deltaTime;
        Vector3 newPosition = transform.position;
        newPosition.x = positionX;
        //transform.position = newPosition;
        transform.position=Vector3.MoveTowards(transform.position,newPosition,step);
    }
    public float setTurnSpeed(){
        turnSpeed *=GetComponent<Score>().difficultyLevel;
        return turnSpeed;
    }
}
