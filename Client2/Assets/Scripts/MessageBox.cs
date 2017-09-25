using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class MessageBox : MonoBehaviour {
public GameObject window;


public Text messageField;

public void Show(string message){
	messageField.text = message;
	window.SetActive(true);
	
}

public void Show(){
	window.SetActive(true);
}


public void Hide(){
	window.SetActive(false);
		
}


}
