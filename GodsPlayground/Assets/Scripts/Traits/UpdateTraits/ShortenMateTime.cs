using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShortenMateTime : Trait
{

    public override string Name => "Shorten Mate Time";

    public override string Description => "Decreases the mate time of animal by 30%";

    public override void Mutate()
    {
        
    }

    public override void Apply(Animal animal)
    {
        animal.baseMateTime = ConstantsUtility.baseMateTime * 0.7f;
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
