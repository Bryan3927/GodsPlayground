using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopulationChart : MonoBehaviour
{
    public int bunnyPopulation;
    public int foxPopulation;
    public TextMesh bunnyText;
    public TextMesh foxText;
    private int ogBunnyPop;
    private int ogFoxesPop;

    public void UpdatePopulations(int bunnyPop, int foxPop)
    {
        if (ogBunnyPop == 0){
            ogBunnyPop = bunnyPop;
            ogFoxesPop = foxPop;
        }
        bunnyPopulation = bunnyPop;
        foxPopulation = foxPop;
    }

    public void DisplayPopulations()
    {
        bunnyText.text = "" + bunnyPopulation;
        foxText.text = "" + foxPopulation;
        Debug.Log("bunny: " + ogBunnyPop);
        Debug.Log("fox: " + ogFoxesPop);
        if (bunnyPopulation <= Mathf.Floor(ogBunnyPop / 2) || bunnyPopulation >= ogBunnyPop * 2) {
            bunnyText.color = Color.red;
        } else {
            bunnyText.color = Color.white;
        }
        
        if (foxPopulation <= Mathf.Floor(ogFoxesPop / 2) || foxPopulation >= ogFoxesPop * 2) {
            foxText.color = Color.red;
        } else {
            foxText.color = Color.white;
        }
    }
}
