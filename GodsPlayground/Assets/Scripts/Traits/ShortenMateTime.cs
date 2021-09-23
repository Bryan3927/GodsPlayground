using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShortenMateTime : Trait
{
    public override void Mutate()
    {
        throw new System.NotImplementedException();
    }

    public override void Apply(Animal animal)
    {
        animal.baseMateTime = ConstantsUtility.baseMateTime * 0.7f;
    }
}
