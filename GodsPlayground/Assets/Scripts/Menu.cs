using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{

    public GameObject UI;
    Transform mainMenu;
    Transform ctrls;
    Transform credits;


    void Start()
    {


        //extract main menu and add listeners to buttons
        mainMenu = UI.transform.GetChild(0);
        mainMenu.GetChild(0).GetComponent<Button>().onClick.AddListener(delegate { this.ShowControls(); });
        mainMenu.GetChild(1).GetComponent<Button>().onClick.AddListener(delegate { this.StartGame(); });
        mainMenu.GetChild(2).GetComponent<Button>().onClick.AddListener(delegate { this.ShowCredits(); });

        //extract ctrls and add listeners to buttons
        ctrls = UI.transform.GetChild(1);
        ctrls.GetComponentInChildren<Button>().onClick.AddListener(delegate { this.ShowMenu(ctrls); });

        //extract credits and add listeners to buttons
        credits = UI.transform.GetChild(2);
        credits.GetComponentInChildren<Button>().onClick.AddListener(delegate { this.ShowMenu(credits); });

        //set other still frames invisible
        ctrls.gameObject.SetActive(false);
        credits.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void ShowControls()
    {
       mainMenu.gameObject.SetActive(false);
       ctrls.gameObject.SetActive(true);
    }

    public void ShowCredits()
    {
        mainMenu.gameObject.SetActive(false);
        credits.gameObject.SetActive(true);
    }

    public void ShowMenu(Transform currentFrame)
    {
        currentFrame.gameObject.SetActive(false);
        mainMenu.gameObject.SetActive(true);
    }

    public void StartGame()
    {
        SceneManager.LoadScene(2);
    }

}
