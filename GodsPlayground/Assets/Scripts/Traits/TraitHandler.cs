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

        SmartBunnies sb = new GameObject().AddComponent<SmartBunnies>();

        RunFromThreat rft = new GameObject().AddComponent<RunFromThreat>();

        // LEVEL TWO TRAITS - FOXES
        SpeedBoost2 sp2 = new GameObject().AddComponent<SpeedBoost2>();

        Lockon lckon = new GameObject().AddComponent<Lockon>();

        Fasting fasting = new GameObject().AddComponent<Fasting>();

        bunnyTraits = new List<Trait>() { sp1, smt, sgp, sb, rft, sp2};

        foxTraits = new List<Trait>() { sp2 , smt, sgp, sp2, lckon, fasting};

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
        List<Trait> removedTraits = new List<Trait>();
        int sIndex;
        for (int i=0; i<n; i++){
            if (animal==Species.Rabbit){
                sIndex=Random.Range(0, bunnyTraits.Count);
                Debug.Log(sIndex);
                roundTraits.Add(bunnyTraits[sIndex]);
                removedTraits.Add(bunnyTraits[sIndex]);
                bunnyTraits.RemoveAt(sIndex); //prevents choosing of the same trait
                
            }
            else{
                sIndex=Random.Range(0, foxTraits.Count);
                roundTraits.Add(foxTraits[sIndex]);
                removedTraits.Add(foxTraits[sIndex]);
                foxTraits.RemoveAt(sIndex);
            }
           
        }
        //add back the selected traits into their respective groups; env.upgrade does the removal of the selected trait

        if (animal == Species.Rabbit)
        {
            foreach(Trait t in removedTraits)
            {
                bunnyTraits.Add(t);
            }
        }
        else
        {
            foreach (Trait t in removedTraits)
            {
                foxTraits.Add(t);
            }
        }


        return roundTraits;
    }
}
