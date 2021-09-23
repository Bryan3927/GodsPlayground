using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShortenGestationPeriod : Trait
{
    public override void Mutate()
    {
        throw new System.NotImplementedException();
    }

    public override void Apply(Animal animal)
    {
        animal.baseGestationPeriod = ConstantsUtility.baseGestationPeriod * 0.8f;
    }
}
