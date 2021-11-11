using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopulationChart : MonoBehaviour
{
    public int bunnyPop;
    public int foxPop;
    public TextMesh bunnyText;
    public TextMesh foxText;
    private int ogBunnyPop;
    private int ogFoxesPop;

    public void UpdatePopulations(int bunnyPopulation, int foxPopulation)
    {
        if (ogBunnyPop == 0){
            ogBunnyPop = bunnyPopulation;
            ogFoxesPop = foxPopulation;
        }
        bunnyPop = bunnyPopulation;
        foxPop = foxPopulation;
    }

    public void DisplayPopulations()
    {
        bunnyText.text = "" + bunnyPop;
        foxText.text = "" + foxPop;

        if (bunnyPop / foxPop > (ogBunnyPop / ogFoxesPop) * 1.5 || bunnyPop / foxPop < (ogBunnyPop / ogFoxesPop) * 0.5) {
            bunnyText.color = Color.red;
            foxText.color = Color.red;
        } else {
            bunnyText.color = Color.white;
            foxText.color = Color.white;
        }
    }
}
