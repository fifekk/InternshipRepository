using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour {

	public float score  = 0.0f;
	public float time = 5.0f;
	public float startTime=1.5f;
	public int difficultyLevel=1; //Asia
	private int maxDifficultyLevel = 10;
	private int scoreToNextLevel = 10;
	private bool isDead = false;
	public Text scoreText;
	public Text levelText;
	public Text startText;
	
	
	void Update () 
	{
		if(isDead)
		return;
		
		if(score >= scoreToNextLevel)
		{
			levelUp();
		}
		startText.fontSize = 500;
		if(time < 1.0f)
		{
			startText.text =("START" );
			startTime-=Time.deltaTime;
			if(startTime < 0.1f)
			{
				startText.text = " ";
				scoreText.text =("A " + (int)countPoints()).ToString();
				levelText.text =("S " + (int)difficultyLevel).ToString();
			}			
		}
		else
		{
			startText.text =("    " + (int)timer()).ToString();
		}
	}
	void levelUp()
	{		
		if(difficultyLevel==maxDifficultyLevel)
		return;
		scoreToNextLevel *= 2;
		difficultyLevel++;

		GetComponent<PlayerMotor>().setSpeed(difficultyLevel);
	}

	public void onDeath()
	{
		isDead = true;
	}
	public float substractPoints(float number)
	{
		score-=number;
		return score;
	}
	public float timer(){
		time-=Time.deltaTime;
		return time;
	}
	public float countPoints(){
		score+=Time.deltaTime;
		return score;
	}
}
