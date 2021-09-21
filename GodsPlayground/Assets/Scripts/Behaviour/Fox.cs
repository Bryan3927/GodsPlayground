using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fox : Animal
{

    public override void Init(Coord coord)
    {
        timeBetweenActionChoices = 1;
        moveSpeed = 1.5f;
        timeToDeathByHunger = 200;
        timeToDeathByThirst = 200;

        drinkDuration = 6;
        eatDuration = 10;

        base.Init(coord);
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }
}
