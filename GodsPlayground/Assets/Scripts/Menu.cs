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


    void Start()
    {


        //extract main menu and add listeners to buttons
        mainMenu = UI.transform.GetChild(0);
        mainMenu.GetChild(0).GetComponent<Button>().onClick.AddListener(delegate { this.ShowControls(); });
        mainMenu.GetChild(1).GetComponent<Button>().onClick.AddListener(delegate { this.StartGame(); });


        //extract ctrls and add listeners to buttons
        ctrls = UI.transform.GetChild(1);
        ctrls.GetChild(0).GetComponent<Button>().onClick.AddListener(delegate { this.ShowMenu(ctrls); });


        //set other still frames invisible
        ctrls.gameObject.SetActive(false);
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
