using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Smell : Trait
{
    public override string Name => "Smell";

    public override string Description => "Allows Foxes to find hidden bunnies via smell";

    public override void ActApply(Animal animal)
    {
    }

    public override void Apply(Animal animal)
    {
    }

    public override void ChooseNextActionApply(Animal animal)
    {
        if (animal.target == Target.Food)
        {

        }
    }

    public override void InteractionApply(Animal animal)
    {
    }

    public override void Mutate()
    {
    }
}
