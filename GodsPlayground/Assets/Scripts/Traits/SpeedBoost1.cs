using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Applies a 20% speed buff
public class SpeedBoost1 : Trait
{
    public SpeedBoost1()
    {

    }

    public override void Mutate()
    {

    }

    public override void Apply(Animal animal)
    {
        animal.baseMoveSpeed = ConstantsUtility.baseMoveSpeed * 1.2f;
    }
}
