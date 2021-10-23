using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TraitHandler : MonoBehaviour
{

    public List<Trait> bunnyTraits;

    public List<Trait> foxTraits;

    public List<Trait> allAvailableTraits = new List<Trait>();

    // Start is called before the first frame update
    void Start()
    {
        // LEVEL ONE TRAITS - BUNNIES?
        SpeedBoost1 sp1 = new GameObject().AddComponent<SpeedBoost1>();

        ShortenMateTime smt = new GameObject().AddComponent<ShortenMateTime>();
        
        ShortenGestationPeriod sgp = new GameObject().AddComponent<ShortenGestationPeriod>();

        // LEVEL TWO TRAITS - FOXES
        SpeedBoost2 sp2 = new GameObject().AddComponent<SpeedBoost2>();

        bunnyTraits = new List<Trait>() { sp1, smt, sgp };

        foxTraits = new List<Trait>() { sp2 }; 

        allAvailableTraits.AddRange(bunnyTraits);
        allAvailableTraits.AddRange(foxTraits);
    }

    public List<Trait> GetRandomTraits(Species animal, int n)
    {
        // int numTraits = traits.Count;
        // if (numTraits < 3)
        // {
        //     Debug.LogError("Not enough traits for UI!");
        // }
        
        List<Trait> roundTraits = new List<Trait>();
        for (int i=0; i<n; i++){
            if (animal==Species.Rabbit){
                roundTraits.Add(bunnyTraits[Random.Range(0, bunnyTraits.Count)]);
            }
            else{
                roundTraits.Add(foxTraits[Random.Range(0, foxTraits.Count)]);
            }
        }

        return roundTraits;
    }
}
