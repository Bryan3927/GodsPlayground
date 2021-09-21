using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rabbit : Animal {
    public static readonly string[] GeneNames = { "A", "B" };

    public override void Init(Coord coord)
    {
        timeBetweenActionChoices = 1;
        moveSpeed = 3.0f;
        timeToDeathByHunger = 200;
        timeToDeathByThirst = 200;

        drinkDuration = 6;
        eatDuration = 10;

        base.Init(coord);
    }

    protected override void Update()
    {
        base.Update();
    }

    public override float Consume(float amount)
    {
        Die(CauseOfDeath.Eaten);

        // affects how much fox hunger is satiated by eating a rabbit
        return 0.75f;
    }
}