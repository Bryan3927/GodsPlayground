using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hiding : Trait
{
    public override string Name => "Hiding";

    public override string Description => "Allows Bunnies to hide if a threat is nearby";

    public override void ActApply(Animal animal)
    {
        Color animalColor = animal.material.color;
        if (animal.currentAction == CreatureAction.Hiding)
        {
            animal.material.color = new Color(animalColor.r, animalColor.g, animalColor.b, 0.5f);
        } else
        {
            animal.material.color = new Color(animalColor.r, animalColor.g, animalColor.b, 1);
        }
    }

    public override void Apply(Animal animal)
    {
    }

    public override void ChooseNextActionApply(Animal animal)
    {
        Animal threat = Environment.SenseThreat(animal, animal.maxViewDistance / 2.0f);
        if (threat != null)
        {
            animal.target = Target.Undefined;
            animal.currentAction = CreatureAction.Hiding;
        }
    }

    public override void InteractionApply(Animal animal)
    {
    }

    public override void Mutate()
    {
    }
}
