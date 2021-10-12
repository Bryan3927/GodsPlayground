using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TraitHandler : MonoBehaviour
{

    public List<Trait> levelOneTraits;

    public List<Trait> levelTwoTraits;

    public List<Trait> allAvailableTraits = new List<Trait>();

    // Start is called before the first frame update
    void Start()
    {
        // LEVEL ONE TRAITS
        SpeedBoost1 sp1 = new GameObject().AddComponent<SpeedBoost1>();

        ShortenMateTime smt = new GameObject().AddComponent<ShortenMateTime>();
        
        ShortenGestationPeriod sgp = new GameObject().AddComponent<ShortenGestationPeriod>();

        // LEVEL TWO TRAITS
        SpeedBoost2 sp2 = new GameObject().AddComponent<SpeedBoost2>();

        levelOneTraits = new List<Trait>() { sp1, smt, sgp };

        levelTwoTraits = new List<Trait>() { sp2 };

        allAvailableTraits.AddRange(levelOneTraits);
        allAvailableTraits.AddRange(levelTwoTraits);
    }

    public List<Trait> GetRandomTraits(List<Trait> traits)
    {
        int numTraits = traits.Count;
        if (numTraits < 3)
        {
            Debug.LogError("Not enough traits for UI!");
        }
        return new List<Trait>();
    }
}
