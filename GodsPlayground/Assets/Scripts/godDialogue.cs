using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class godDialogue : MonoBehaviour
{


    private Text messageText;
    private TextWriter.TextWriterSingle textWriterSingle;
    private string[] dialogue = new string[] {
        "My child I am going away..." ,
        "...on vacation",
        "But I shall bestow you some of my powers",
        "Maintain order over this ecosystem. I'll be back in a few days",
        "Don't fuck it up",
        "Don't fuck it up"
    };
    private int dialogueIndex = 0;


    private void Awake()
    {
        messageText = transform.GetComponentInChildren<Text>();
        transform.GetComponentInChildren<Button>().onClick.AddListener(delegate { this.Click(); });
    }

    private void Start()
    {
       textWriterSingle = TextWriter.AddWriter_static(messageText, dialogue[0], .05f, true, true);
    }


    private void Click()
    {
        if (textWriterSingle != null && textWriterSingle.IsActive())
        {
            textWriterSingle.WriteAllAndDestroy();
        }
        else
        {
            dialogueIndex++;

            if (dialogueIndex >= dialogue.Length)
            {
                //SceneManager.LoadScene(1);
                LevelLoader.LoadLevel_static(1);
            }

            else
            {
                if (dialogueIndex == dialogue.Length - 1)
                {
                    textWriterSingle = TextWriter.AddWriter_static(messageText, dialogue[dialogueIndex], .15f, true, true);
                }
                else
                {
                    textWriterSingle = TextWriter.AddWriter_static(messageText, dialogue[dialogueIndex], .05f, true, true);
                }
            }

            
                
        }
    }



}
 