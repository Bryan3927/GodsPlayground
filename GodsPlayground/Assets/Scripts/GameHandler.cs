using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameHandler : MonoBehaviour
{

    public GameObject UI;

    float gameStartTime;
    float waitTime = 70.0f;
    float lastSimSpeed;

    // Start is called before the first frame update
    void Start()
    {
        UI.SetActive(false);
        gameStartTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time - gameStartTime > waitTime && !UI.activeInHierarchy)
        {
            lastSimSpeed = Environment.GetSimSpeed();
            Environment.SetSimSpeed(0);
            UI.SetActive(true);
        }
    }

    public void StartNextRound()
    {
        Debug.Log("Starting next round. Previous sim speed: " + lastSimSpeed);
        UI.SetActive(false);
        Environment.SetSimSpeed(lastSimSpeed);
        gameStartTime = Time.time;
    }
}
