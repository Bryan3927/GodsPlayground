using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShortenGestationPeriod : Trait
{
    public override string Name => "Shorten Gestation Period";

    public override string Description => "Decreases the gestation period of the animal by 20%";

    public override void Mutate()
    {
        throw new System.NotImplementedException();
    }

    public override void Apply(Animal animal)
    {
        animal.baseGestationPeriod = ConstantsUtility.baseGestationPeriod * 0.8f;
    }

    public override void InteractionApply(Animal animal)
    {
        
    }

    public override void ChooseNextActionApply(Animal animal)
    {
        
    }

    public override void ActApply(Animal animal)
    {
        
    }
}
