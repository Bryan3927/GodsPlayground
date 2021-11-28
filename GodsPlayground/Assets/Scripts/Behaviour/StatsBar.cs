using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StatsBar : MonoBehaviour
{

    public Slider slider;

    public void SetMaxStat() => slider.maxValue = 1;

    public void SetStat(float stat) => slider.value = (1 - stat);

    private void Update()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        if (currentScene.name != "intro")
        {
            this.transform.LookAt(Camera.main.transform);
            if (Vector3.Distance(this.transform.position, Camera.main.transform.position) < 10.0f)
            {
                this.transform.localScale = Vector3.one;
            }
            else
            {
                this.transform.localScale = Vector3.zero;
            }
        } else
        {
            this.transform.localScale = Vector3.zero;
        }
    }
}
