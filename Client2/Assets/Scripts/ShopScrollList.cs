using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Nieruchomosci
 {
	public string name;
	// public Nieruchomosci(string name){
	// 	this.name=name;
	// }
	
}

public class ShopScrollList : MonoBehaviour {
	
	
	private static ArrayList[] nieruchomosciList = new ArrayList[Player.propertyList.Count];
	
	public static Transform contentPanel;
	public static SimpleObjectPool buttonObjectPool;
	public static GameObject newButton;
	
	

	// Use this for initialization
	void Start ()
	{ 
		//RefreshDisplay();
		//Client.scrollList = this;
	}
	// /// <summary>
	// /// Update is called every frame, if the MonoBehaviour is enabled.
	// /// </summary>
	// void Update()
	// {
	// 	foreach(string n in Player.propertyList)
	// 	{
	// 		Debug.Log(n);
	// 	}
	// }
	public static void RefreshDisplay(){
		
		//AddButtons();
		
	}
	// public void AddButtons()
	// {
	// 	for (int i = 0; i <Player.propertyList.Count;i++)
	// 	{

	// 		string nieruchomosci = Player.propertyList[i];
	// 		newButton = buttonObjectPool.GetObject();
	// 		newButton.GetComponentInChildren<Text>().text = Player.propertyList[i];
	// 		newButton.transform.SetParent(contentPanel,false);

	// 		SampleButton sampleButton = newButton.GetComponent<SampleButton>();
			
	// 		sampleButton.Setup(nieruchomosci,ShopScrollList.scroll); 

	// 	}
	// }
/* 
	 void AddItem(string nieruchomosc, ShopScrollList shopList)
    {
		Nieruchomosci itemToAdd = new Nieruchomosci(nieruchomosc);

        shopList.nieruchomosciList.Add(itemToAdd);
    }
*/
	public /// <summary>
	/// OnGUI is called for rendering and handling GUI events.
	/// This function can be called multiple times per frame (one call per event).
	/// </summary>
	void OnGUI()
	{
		GUI.Label(new Rect(0,0,200,200),Player.propertyList.Count.ToString());
	}
	

}
