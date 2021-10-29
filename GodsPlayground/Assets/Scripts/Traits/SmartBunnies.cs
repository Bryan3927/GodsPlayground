using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmartBunnies : Trait
{
    public override string Name => "Smart Eating Bunnies";

    public override string Description => "Bunnies learn to leave the root of plants in order to allow them to grow back faster";

    public override void ActApply(Animal animal)
    {
        if (animal.currentAction == CreatureAction.Eating)
        {
            animal.currentAction = CreatureAction.SmartEating;
        }
    }

    public override void Apply(Animal animal)
    {
    }

    public override void ChooseNextActionApply(Animal animal)
    {
    }

    public override void InteractionApply(Animal animal)
    {
        if (animal.currentAction == CreatureAction.SmartEating)
        {
            if (animal.foodTarget && animal.hunger > 0)
            {
                float amountRemaining = ((Plant)animal.foodTarget).amountRemaining;
                float eatAmount = Mathf.Min(animal.hunger, Time.deltaTime * 1 / animal.eatDuration);
                if (eatAmount > amountRemaining)
                {
                    eatAmount = amountRemaining / 2.0f;
                }
                eatAmount = animal.foodTarget.Consume(eatAmount);
                animal.hunger -= eatAmount;
                animal.hunger = Mathf.Clamp01(animal.hunger);
            }
        }
    }

    public override void Mutate()
    {
        throw new System.NotImplementedException();
    }
}
