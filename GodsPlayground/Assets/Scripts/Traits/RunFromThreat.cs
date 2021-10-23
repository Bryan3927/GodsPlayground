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
            animal.StartMoveToCoord(animal.path[animal.pathIndex]);
            animal.pathIndex++;
        }
    }

    public override void Apply(Animal animal)
    {
        throw new System.NotImplementedException();
    }

    public override void ChooseNextActionApply(Animal animal)
    {
        Animal threat = Environment.SenseThreat(animal, animal.maxViewDistance / 2.0f);
        if (threat != null)
        {
            Coord target = Coord.invalid;
            Coord[] surroundingTiles = EnvironmentUtility.GetSurroundingTiles(animal.coord);
            Vector2 animalPosition = new Vector2(animal.coord.x, animal.coord.y);
            float furthestDistance = 0;
            foreach (Coord coord in surroundingTiles)
            {
                float dist = Vector2.Distance(animalPosition, new Vector2(coord.x, coord.y));
                if (dist > furthestDistance)
                {
                    furthestDistance = dist;
                    target = coord;
                }
            }
            if (target != Coord.invalid)
            {
                animal.CreatePath(target);
                animal.target = Target.RunAway;
                animal.currentAction = CreatureAction.RunningAway;
            } else
            {
                animal.currentAction = CreatureAction.Exploring;
            }
        }
    }

    public override void InteractionApply(Animal animal)
    {
        throw new System.NotImplementedException();
    }

    public override void Mutate()
    {
        throw new System.NotImplementedException();
    }
}
