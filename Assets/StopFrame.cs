using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopFrame : MonoBehaviour
{
    // Start is called before the first frame update
    public void Stop()
    {
        Time.timeScale = 0f;

    }
}
