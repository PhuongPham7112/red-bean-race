using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CoinCount : MonoBehaviour
{

    public GameObject player;
    Movement movement_script;
    int coin;
    public TextMeshProUGUI coinText;

    void Start(){
        movement_script = player.GetComponent<Movement>();
    }

    // Update is called once per frame
    void Update()
    {
        coin = movement_script.coinCount;
        coinText.text = "Coins: " + coin.ToString();
    }
}
