using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lockon : Trait
{
    public override string Name => "Lock On";

    public override string Description => "Allows Fox to chase specific bunny until that bunny is dead";

    public override void ActApply(Animal animal)
    {

    }

    public override void Apply(Animal animal)
    {

    }

    public override void ChooseNextActionApply(Animal animal)
    {
        //if food_target!= null
        // createPath(food_target)
        // List<Coord> possibleCoords = ____(animal.coord)
        // var maxIndex = possibleCoords.IndexOf(possibleCoords.Max())
        // Math.max(new List<Coord>() {from coord in possibleCoords select })
    }

    public override void InteractionApply(Animal animal)
    {

    }

    public override void Mutate()
    {

    }
}
