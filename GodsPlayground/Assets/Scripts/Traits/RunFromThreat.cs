using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunFromThreat : Trait
{
    public override string Name => "Run From Threat";

    public override string Description => "Allows animal to run from threats";

    public override void ActApply(Animal animal)
    {
        if (animal.currentAction == CreatureAction.RunningAway)
        {
            // Do nothing. All logic already handled
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
            Coord target = Coord.invalid;
            Coord[] surroundingTiles = Environment.walkableNeighboursMap[animal.coord.x, animal.coord.y];
            Coord threatCoord = threat.coord;
            float furthestDistance = 0;
            foreach (Coord coord in surroundingTiles)
            {
                float dist = Coord.SqrDistance(threatCoord, coord);
                if (dist > furthestDistance)
                {
                    furthestDistance = dist;
                    target = coord;
                }
            }
            if (target != Coord.invalid)
            {
                // animal.CreatePath(target); CreatePath won't work if target is a neighboring tile
                animal.path = new Coord[] { target };
                animal.target = Target.RunAway;
                animal.currentAction = CreatureAction.RunningAway;
                animal.StartMoveToCoord(target);
            } else
            {
                animal.currentAction = CreatureAction.Exploring;
            }
        } else
        {
            // animal.currentAction = CreatureAction.Exploring;
        }
    }

    public override void InteractionApply(Animal animal)
    {
        
    }

    public override void Mutate()
    {
        
    }
}
