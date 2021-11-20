using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstantsUtility : MonoBehaviour
{

    public readonly static float baseTimeBetweenActionChoices = 1;
    public readonly static float baseMoveSpeed = 1.5f;
    public readonly static float baseTimeToDeathByHunger = 200;
    public readonly static float baseTimeToDeathByThirst = 200;
    public readonly static float baseTimeToDeathByHorny = 200;

    public readonly static float baseDrinkDuration = 6;
    public readonly static float baseEatDuration = 10;

    public readonly static float baseTimeToGrow = 30.0f;
    public readonly static float baseAgeRate = 25.0f;
    public readonly static float baseMateTime = 10.0f;
    public readonly static float baseGestationPeriod = 20.0f;

    public readonly static float baseMateWaitTime = 20.0f;

    public static void SetConstants(Animal animal)
    {
        animal.baseTimeBetweenActionChoices = baseTimeBetweenActionChoices;
        animal.baseMoveSpeed = baseMoveSpeed;
        animal.baseTimeToDeathByHunger = baseTimeToDeathByHunger;
        animal.baseTimeToDeathByThirst = baseTimeToDeathByThirst;
        animal.baseTimeToDeathByHorny = baseTimeToDeathByHorny;

        animal.baseDrinkDuration = baseDrinkDuration;
        animal.baseEatDuration = baseEatDuration;

        animal.baseTimeToGrow = baseTimeToGrow;
        animal.baseAgeRate = baseAgeRate;
        animal.baseMateTime = baseMateTime;
        animal.baseGestationPeriod = baseGestationPeriod;

        animal.baseMateWaitTime = baseMateWaitTime;

    }

}
