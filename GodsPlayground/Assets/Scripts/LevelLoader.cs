using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    public Animator transition;
    public float transitionTime = 1f;
    private static LevelLoader instance;
    public GameObject UI;

    private void Awake()
    {
        instance = this;
    }


    private void Update()
    {
       // if (transition.GetCurrentAnimatorStateInfo(0).IsName("idle"))
       // {
       //     UI.transform.Find("crossfade").gameObject.SetActive(false);
       // }
    }

    public static void LoadLevel_static(int scene_index)

    {
        instance.UI.transform.Find("crossfade").gameObject.SetActive(true);
        instance.StartCoroutine(instance.LoadLevel(scene_index));
    }


    IEnumerator LoadLevel(int levelIndex)
    {
        //Play animation
        transition.SetTrigger("start");
        //wait
        yield return new WaitForSeconds(transitionTime);
        //load scene
        SceneManager.LoadScene(levelIndex);
    }
}