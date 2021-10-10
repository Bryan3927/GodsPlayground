using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TraitHandler : MonoBehaviour
{
    
    public List<Trait> levelOneTraits = new List<Trait>() { new SpeedBoost1(), new ShortenGestationPeriod(), new ShortenMateTime()};

    public List<Trait> allAvailableTraits = new List<Trait>();

    // Start is called before the first frame update
    void Start()
    {

    }
}
