using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Applies a 20% speed buff
public class SpeedBoost1 : Trait
{

    public override string Name => "Speed Boost v1";

    public override string Description => "Increases the speed of the animal by 20%";

    public override void Mutate()
    {

    }

    public override void Apply(Animal animal)
    {
        animal.baseMoveSpeed = ConstantsUtility.baseMoveSpeed * 1.2f;
    }
}
