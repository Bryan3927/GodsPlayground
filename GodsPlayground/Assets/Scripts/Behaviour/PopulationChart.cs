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

    public void UpdatePopulations(int bunnyPop, int foxPop)
    {
        bunnyPopulation = bunnyPop;
        foxPopulation = foxPop;
    }

    public void DisplayPopulations()
    {
        bunnyText.text = "" + bunnyPopulation;
        foxText.text = "" + foxPopulation;
    }
}
