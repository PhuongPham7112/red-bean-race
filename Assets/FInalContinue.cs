using UnityEngine.UI;
using TMPro;
using UnityEngine;

public class FInalContinue : MonoBehaviour
{
    public TextMeshProUGUI FinalContinue;
    public GameObject GameManager;
    bool gameResult;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        gameResult = GameManager.GetComponent<GameManager>().gameResult;
        if (gameResult) {
            FinalContinue.text = "Celebrate";
        } else {
            FinalContinue.text = "Continue";
        }
    }
}
