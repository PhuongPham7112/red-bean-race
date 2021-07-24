using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Announce : MonoBehaviour
{
    public GameObject gameManager;
    string announceResult;
    public TextMeshProUGUI AnnounceText;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    void Update(){
        if (gameManager.GetComponent<GameManager>().gameResult == true){
            announceResult = "win";
        } else {
            announceResult = "lose";
        }
        AnnounceText.text = "You " + announceResult;
    }
}
