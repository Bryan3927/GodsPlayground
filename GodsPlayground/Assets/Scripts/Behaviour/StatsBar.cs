using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatsBar : MonoBehaviour
{

    public Slider slider;

    public void SetMaxStat() => slider.maxValue = 1;

    public void SetStat(float stat) => slider.value = stat;
}
