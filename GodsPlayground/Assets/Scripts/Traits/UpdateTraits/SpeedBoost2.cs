using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedBoost2 : Trait
{
    public override string Name => "SpeedBoostv2";

    public override string Description => "Increases base speed by 50%";

    public override void ActApply(Animal animal)
    {

    }

    public override void Apply(Animal animal)
    {
        animal.baseMoveSpeed = ConstantsUtility.baseMoveSpeed * 1.5f;
    }

    public override void ChooseNextActionApply(Animal animal)
    {
        
    }

    public override void InteractionApply(Animal animal)
    {
        
    }

    public override void Mutate()
    {
        
    }
}
