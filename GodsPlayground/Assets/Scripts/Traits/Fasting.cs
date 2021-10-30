using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fasting : Trait
{
    public override string Name => "Fasting";

    public override string Description => "Allows Fox to periodically fast for a period";

    private bool active = true;

    public override void ActApply(Animal animal)
    {

    }

    public override void Apply(Animal animal)
    {
        if (animal.hunger>=.8f && active)
        {
            animal.hunger = .8f;
        }
    }

    public override void ChooseNextActionApply(Animal animal)
    {
        if (animal.target == Target.Food && active)
        {
            if ((animal.thirst >= animal.horny && animal.thirst > 0.1) || animal.thirst > animal.criticalPercent)
            {
                animal.target = Target.Water;
            }
            else if (!animal.pregnant && (animal.horny > animal.thirst && animal.horny > 0.1))
            {
                animal.target = Target.Mate;
            }
            else
            {
                animal.currentAction = CreatureAction.Exploring;
            }
        }
    }

    public override void InteractionApply(Animal animal)
    {

    }

    public override void Mutate()
    {
        active = !active;
    }
}
