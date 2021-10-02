using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShortenMateTime : Trait
{

    public override string Name => "Shorten Mate Time";

    public override string Description => "Decreases the mate time of animal by 30%";

    public override void Mutate()
    {
        throw new System.NotImplementedException();
    }

    public override void Apply(Animal animal)
    {
        animal.baseMateTime = ConstantsUtility.baseMateTime * 0.7f;
    }
}
