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
        if (animal.foodTarget != null)
        {
            animal.target = Target.Undefined;
            if (EnvironmentUtility.TileIsVisibile(animal.coord.x, animal.coord.y, animal.foodTarget.coord.x, animal.foodTarget.coord.y))
            {
                //even if u r neighbors, path being null will not be called by act because it catches neighbors with conditional
                animal.CreatePath(animal.foodTarget.coord);
            }
            else
            {
                float minDistance = 300;
                Coord[] possibleTiles = Environment.walkableNeighboursMap[animal.coord.x, animal.coord.y];
                Coord bestTile = animal.coord;
                foreach (Coord ptile in possibleTiles)
                {
                    float d = Coord.Distance(ptile, animal.foodTarget.coord);
                    if (d < minDistance)
                    {
                        bestTile = ptile;
                        minDistance = d;
                    }

                }

                animal.path = new Coord[] { bestTile };
            }
        }
    }

    public override void InteractionApply(Animal animal)
    {

    }

    public override void Mutate()
    {

    }
}
