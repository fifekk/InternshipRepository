using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SampleButton : MonoBehaviour {
    
    public Button button;
    public Text NameLabel;
    private string nieruchomosci;
    private ShopScrollList scrollList;
    public Client client;
    
    void Start(){

    }
    public void Setup(string currentNieruchomosci, ShopScrollList currentScrollList)
    {
        nieruchomosci = currentNieruchomosci;
        //NameLabel.text = nieruchomosci.name;
        scrollList = currentScrollList;

    }
    public string HandleClick()
    {
        string text = GetComponentInChildren<Text>().text;
        return text;
    }
}