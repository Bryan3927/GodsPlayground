using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animal : LivingEntity {

    public int maxViewDistance = 10;

    [EnumFlags]
    public Species diet;

    public CreatureAction currentAction;
    public Genes genes;
    public Color maleColour;
    public Color femaleColour;

    public ParticleSystem birthParticles;

    // Virtual settings after being multiplied by Sim Speed
    [HideInInspector]
    public float 
        timeBetweenActionChoices,
        moveSpeed,
        timeToDeathByHunger,
        timeToDeathByThirst,
        timeToDeathByHorny,
        drinkDuration,
        eatDuration,
        timeToGrow,
        ageRate,
        mateTime,
        gestationPeriod,
        mateWaitTime;

    // Base settings, won't be affected by Sim Speed
    [HideInInspector]
    public float 
        baseTimeBetweenActionChoices, 
        baseMoveSpeed, 
        baseTimeToDeathByHunger, 
        baseTimeToDeathByThirst, 
        baseTimeToDeathByHorny, 
        baseDrinkDuration, 
        baseEatDuration,
        baseTimeToGrow,
        baseAgeRate,
        baseMateTime,
        baseGestationPeriod,
        baseMateWaitTime;

    float scale = 0.6f;
    float mateStartTime;
    public float criticalPercent = 0.7f;

    public bool pregnant = false;
    float pregnantTime;

    protected bool acceptedMateRequest = false;

    public bool isMale;

    // Visual settings:
    float moveArcHeight = .2f;

    // State:
    [Header ("State")]
    public float hunger;
    public float thirst;
    public float horny;
    public int age;
    private float ageProgress = 0;

    public StatsBar hungerBar;
    public StatsBar thirstBar;
    public StatsBar hornyBar;

    public LivingEntity foodTarget;
    protected Coord waterTarget;
    public Animal mateTarget = null;

    // Traits (Fun stuff!)
    public List<Trait> traits = new List<Trait>();

    // Move data:
    bool animatingMovement;
    Coord moveFromCoord;
    Coord moveTargetCoord;
    Vector3 moveStartPos;
    Vector3 moveTargetPos;
    float moveTime;
    float moveSpeedFactor;
    float moveArcHeightFactor;
    public Coord[] path;
    public int pathIndex;

    // Other
    protected float lastActionChooseTime;
    const float sqrtTwo = 1.4142f;
    const float oneOverSqrtTwo = 1 / sqrtTwo;
    float waitingForMate;

    List<Animal> potentialMatches = new List<Animal>();

    public Target target;

    public override void Init (Coord coord) {
        base.Init (coord);
        ConstantsUtility.SetConstants(this);
        moveFromCoord = coord;
        genes = Genes.RandomGenes (1);
        age = 0;

        material.color = (genes.isMale) ? maleColour : femaleColour;

        isMale = genes.isMale;

        birthParticles.Play();

        hungerBar.SetMaxStat();
        thirstBar.SetMaxStat();
        hornyBar.SetMaxStat();

        ChooseNextAction ();
    }



    protected virtual void Update () {

        UpdateSpeeds();
        // Apply Update Traits
        foreach (Trait trait in traits)
        {
            trait.Apply(this);
        }

        // Increase hunger and thirst over time
        hunger += Time.deltaTime * 1 / timeToDeathByHunger;
        thirst += Time.deltaTime * 1 / timeToDeathByThirst;
        if (!pregnant) horny += Time.deltaTime * 1 / timeToDeathByHorny;
        ageProgress += Time.deltaTime * 1 / ageRate;
        if (ageProgress >= 1)
        {
            age += 1;
            ageProgress = 0;
            if (age > 10)
            {
                // Starting at age 11, animals have a +20% chance to die at each new age
                // The highest age an animal can reach is 15
                float chance = UnityEngine.Random.Range(0, 1.0f);
                if ((float)(age - 10) * 0.2f > chance)
                {
                    Die(CauseOfDeath.Age);
                }
            }
        }
        if (age == 0 && ageProgress >= 0.5 && birthParticles.isEmitting)
        {
            birthParticles.Stop();
        }

        hungerBar.SetStat(hunger);
        thirstBar.SetStat(thirst);
        hornyBar.SetStat(horny);

        // Animate movement. After moving a single tile, the animal will be able to choose its next action
        if (animatingMovement) {
            animatingMovement = AnimateMove();
        } else {
            // Handle interactions with external things, like food, water, mates
            HandleInteractions ();
            float timeSinceLastActionChoice = Time.time - lastActionChooseTime;
            if (timeSinceLastActionChoice > timeBetweenActionChoices) {
                ChooseNextAction ();
            }
        }

        // Handles Births
        if (pregnant && Time.time - pregnantTime > gestationPeriod)
        {
            HandleBirth();
            pregnant = false;
        }

        // Handles Deaths
        if (hunger >= 1) {
            Die (CauseOfDeath.Hunger);
        } else if (thirst >= 1) {
            Die (CauseOfDeath.Thirst);
        } else if (horny >= 1)
        {
            horny = 1.0f;
        } 

        // Resizes baby animals
        if (transform.localScale != Vector3.one)
        {
            scale = Mathf.Clamp01((scale + (0.4f / timeToGrow) * Time.deltaTime));
            transform.localScale = Vector3.one * scale;
        }

    }

    // Animals choose their next action after each movement step (1 tile),
    // or, when not moving (e.g interacting with food etc), at a fixed time interval
    protected virtual void ChooseNextAction () {
        lastActionChooseTime = Time.time;
        // Get info about surroundings

        // Decide next action:
        // Eat if (more hungry than thirsty) or (currently eating and not critically thirsty)
        bool currentlyEating = currentAction == CreatureAction.Eating && foodTarget && hunger > 0.01;
        bool currentlyDrinking = currentAction == CreatureAction.Drinking && thirst > 0.01;
        bool currentlyMating = currentAction == CreatureAction.Mating && horny > 0.01;
        bool currentlyWaiting = currentAction == CreatureAction.WaitingForMate;

        target = Target.Undefined;

        if ((currentlyWaiting || acceptedMateRequest) && hunger < criticalPercent && thirst < criticalPercent)
        {
            currentAction = CreatureAction.WaitingForMate;
        } else if (currentlyEating && thirst < criticalPercent)
        {
            target = Target.Food;
        } else if (currentlyDrinking && hunger < criticalPercent)
        {
            target = Target.Water;
        } else if (currentlyMating && hunger < criticalPercent && thirst < criticalPercent) {
            // Should not need to find another mate
        } 
        else if ((hunger >= thirst && hunger >= horny && hunger > 0.1) || hunger > criticalPercent)
        {
            target = Target.Food;
        } else if ((thirst >= hunger && thirst >= horny && thirst > 0.1) || thirst > criticalPercent)
        {
            target = Target.Water;
        } else if (!pregnant && (horny > hunger && horny > thirst && horny > 0.1))
        {
            target = Target.Mate;
        } else
        {
            currentAction = CreatureAction.Exploring;
        }

        // Apply Choose Next Action Traits
        foreach (Trait trait in traits)
        {
            trait.ChooseNextActionApply(this);
        }

        // If target is part of base AI, run base code. Otherwise code is offloaded to ChooseNextActionApply
        switch(target)
        {
            case (Target.Food):
                FindFood();
                break;
            case (Target.Water):
                FindWater();
                break;
            case (Target.Mate):
                FindMate();
                break;
            default:
                // Do nothing
                break;
        }
            

        Act ();

    }

    protected virtual void FindFood () {
        LivingEntity foodSource = Environment.SenseFood (coord, this, FoodPreferencePenalty);
        if (foodSource) {
            currentAction = CreatureAction.GoingToFood;
            foodTarget = foodSource;
            CreatePath (foodTarget.coord);

        } else {
            currentAction = CreatureAction.Exploring;
        }
    }

    protected virtual void FindWater () {
        Coord waterTile = Environment.SenseWater (coord, this);
        if (waterTile != Coord.invalid) {
            currentAction = CreatureAction.GoingToWater;
            waterTarget = waterTile;
            CreatePath (waterTarget);

        } else {
            currentAction = CreatureAction.Exploring;
        }
    }

    protected virtual void FindMate()
    {
        List<Animal> potentialMates = Environment.SensePotentialMates(coord, this);

        // If mate target exists and this is not a waiting female, create path to mate target
        if (mateTarget && currentAction != CreatureAction.WaitingForMate)
        {
            // CreatePath(mateTarget.coord);
        } 
        // else if no mate target and this is a male, look for mate target
        else if (genes.isMale && potentialMatches.Count > 0)
        {
            Animal[] copy = new Animal[potentialMatches.Count];
            potentialMatches.CopyTo(copy);
            foreach (Animal female in copy)
            {
                if (currentAction == CreatureAction.GoingToMate)
                {
                    female.CancelMate();
                    continue;
                }
                try
                {
                    CreatePath(female.coord);
                    if (path == null)
                    {
                        female.CancelMate();
                        potentialMatches.Remove(female);
                        continue;
                    }
                    mateTarget = female;
                    currentAction = CreatureAction.GoingToMate;
                    potentialMatches.Clear();
                }
                catch (Exception)
                {
                    continue;
                }
            }
        } else if (genes.isMale)
        {
            currentAction = CreatureAction.SearchingForMate;
            foreach (Animal female in potentialMates)
            {
                female.AskToMate(this);
            }
        } else if (!genes.isMale && potentialMatches.Count > 0)
        {
            Animal[] copy = new Animal[potentialMatches.Count];
            potentialMatches.CopyTo(copy);
            foreach (Animal male in copy)
            {
                try
                {
                    // Accept/Reject logic
                    CreatePath(male.coord);
                    if (path == null)
                    {
                        potentialMatches.Remove(male);
                        continue;
                    }
                    male.AskToMate(this);
                    mateTarget = male;
                    currentAction = CreatureAction.WaitingForMate;
                    potentialMatches.Clear();
                    waitingForMate = Time.time;
                    break;
                } catch (Exception)
                {
                    continue;
                }
            }
        } 
        else
        {
            currentAction = CreatureAction.SearchingForMate;
        }
    }

    public void AskToMate(Animal male)
    {
        // TODO: Add rejection logic if needed/desired
        //LookAt(male.coord);

        potentialMatches.Add(male);
    }

    public void CancelMate()
    {
        mateTarget = null;
        acceptedMateRequest = false;
        currentAction = CreatureAction.Exploring;
    }

    public bool CheckExists()
    {
        return true;
    }

    // When choosing from multiple food sources, the one with the lowest penalty will be selected
    protected virtual int FoodPreferencePenalty (LivingEntity self, LivingEntity food) {
        return Coord.SqrDistance (self.coord, food.coord);
    }

    protected void Act () {
        switch (currentAction) {
            case CreatureAction.Exploring:
                StartMoveToCoord (Environment.GetNextTileWeighted (coord, moveFromCoord));
                break;
            case CreatureAction.SearchingForMate:
                StartMoveToCoord(Environment.GetNextTileWeighted(coord, moveFromCoord));
                break;
            case CreatureAction.GoingToFood:
                if (Coord.AreNeighbours (coord, foodTarget.coord)) {
                    LookAt (foodTarget.coord);
                    currentAction = CreatureAction.Eating;
                } else {
                    try
                    {
                        StartMoveToCoord(path[pathIndex]);
                    } catch (Exception)
                    {
                        Debug.LogError("Failed to move towards food. Food target is: " + foodTarget + " and path should be is: " + EnvironmentUtility.GetPath(coord.x, coord.y, foodTarget.coord.x, foodTarget.coord.y));
                    } 
                    
                    pathIndex++;
                }
                break;
            case CreatureAction.GoingToWater:
                if (Coord.AreNeighbours (coord, waterTarget)) {
                    LookAt (waterTarget);
                    currentAction = CreatureAction.Drinking;
                } else {
                    StartMoveToCoord (path[pathIndex]);
                    pathIndex++;
                }
                break;
            case CreatureAction.GoingToMate:
                if (Coord.AreNeighbours(coord, mateTarget.coord))
                {
                    LookAt(mateTarget.coord);
                    currentAction = CreatureAction.Mating;
                    mateStartTime = Time.time;
                } else
                {
                    StartMoveToCoord(path[pathIndex]);
                    pathIndex++;
                    if (pathIndex >= path.Length)
                    {
                        currentAction = CreatureAction.SearchingForMate;
                    } 
                }
                break;
            case CreatureAction.WaitingForMate:
                if (Time.time - waitingForMate > mateWaitTime)
                {
                    currentAction = CreatureAction.Exploring;
                    this.CancelMate();
                } else if (Coord.AreNeighbours(coord, mateTarget.coord))
                {
                    LookAt(mateTarget.coord);
                    currentAction = CreatureAction.Mating;
                    mateStartTime = Time.time;
                } else
                {
                    // do nothing lol
                }
                break;
        }

        // Apply Act Traits
        foreach (Trait trait in traits)
        {
            trait.ActApply(this);
        }
    }

    public void CreatePath (Coord target) {
        // Create new path if current is not already going to target
        if (path == null || pathIndex >= path.Length || (path[path.Length - 1] != target || path[Mathf.Clamp(pathIndex - 1, 0, path.Length)] != moveTargetCoord)) {
            path = EnvironmentUtility.GetPath (coord.x, coord.y, target.x, target.y);
            pathIndex = 0;
        }
    }

    public void StartMoveToCoord (Coord target) {
        moveFromCoord = coord;
        moveTargetCoord = target;
        moveStartPos = transform.position;
        moveTargetPos = Environment.tileCentres[moveTargetCoord.x, moveTargetCoord.y];
        animatingMovement = true;

        bool diagonalMove = Coord.SqrDistance (moveFromCoord, moveTargetCoord) > 1;
        moveArcHeightFactor = (diagonalMove) ? sqrtTwo : 1;
        moveSpeedFactor = (diagonalMove) ? oneOverSqrtTwo : 1;

        LookAt (moveTargetCoord);
    }

    protected void LookAt (Coord target) {
        if (target != coord) {
            Coord offset = target - coord;
            transform.eulerAngles = Vector3.up * Mathf.Atan2 (offset.x, offset.y) * Mathf.Rad2Deg;
        }
    }

    void HandleInteractions () {
        if (currentAction == CreatureAction.Eating) {
            if (foodTarget && hunger > 0) {
                float eatAmount = Mathf.Min (hunger, Time.deltaTime * 1 / eatDuration);
                eatAmount = foodTarget.Consume (eatAmount);
                hunger -= eatAmount;
                hunger = Mathf.Clamp01(hunger);
            }
        } else if (currentAction == CreatureAction.Drinking) {
            if (thirst > 0) {
                thirst -= Time.deltaTime * 1 / drinkDuration;
                thirst = Mathf.Clamp01 (thirst);
            }
        } else if (currentAction == CreatureAction.Mating)
        {
            StartMoveToCoord(coord);
            if (Time.time - mateStartTime > mateTime)
            {
                horny = 0;
                this.CancelMate();
                if (!genes.isMale)
                {
                    pregnant = true;
                    pregnantTime = Time.time;
                }
            }
        }

        // Apply Interaction Traits
        foreach (Trait trait in traits)
        {
            trait.InteractionApply(this);
        }
    }

    public virtual bool AnimateMove () {
        moveTime = Mathf.Min(1, moveTime + Time.deltaTime * moveSpeed * moveSpeedFactor);
        float height = (1 - 4 * (moveTime - .5f) * (moveTime - .5f)) * moveArcHeight * moveArcHeightFactor;
        transform.position = Vector3.Lerp(moveStartPos, moveTargetPos, moveTime) + Vector3.up * height;

        // Finished moving
        if (moveTime >= 1)
        {
            Environment.RegisterMove(this, moveFromCoord, moveTargetCoord);
            coord = moveTargetCoord;

            moveTime = 0;
            return false;
        }
        return true;
    }

    public void GiveTrait(Trait trait)
    {
        traits.Add(trait);
        // trait.Apply(this);
    }

    void HandleBirth()
    {
        Environment environment = FindObjectOfType<Environment>();
        environment.SpawnBaby(this);
    }

    void UpdateSpeeds()
    {
        float simSpeed = Environment.GetSimSpeed();
        float simSpeedFraction = 1.0f / simSpeed;
        timeBetweenActionChoices = baseTimeBetweenActionChoices * simSpeedFraction;
        moveSpeed = baseMoveSpeed * simSpeed;
        timeToDeathByHunger = baseTimeToDeathByHunger * simSpeedFraction;
        timeToDeathByThirst = baseTimeToDeathByThirst * simSpeedFraction;
        timeToDeathByHorny = baseTimeToDeathByHorny * simSpeedFraction;

        drinkDuration = baseDrinkDuration * simSpeedFraction;
        eatDuration = baseEatDuration * simSpeedFraction;

        timeToGrow = baseTimeToGrow * simSpeedFraction;
        ageRate = baseAgeRate * simSpeedFraction;
        mateTime = baseMateTime * simSpeedFraction;
        gestationPeriod = baseGestationPeriod * simSpeedFraction;
        mateWaitTime = baseMateWaitTime * simSpeedFraction;
    }

    void OnDrawGizmosSelected () {
        if (Application.isPlaying) {
            var surroundings = Environment.Sense (coord, Species.Plant, this);
            Gizmos.color = Color.white;
            if (surroundings.nearestFoodSource != null) {
                Gizmos.DrawLine (transform.position, surroundings.nearestFoodSource.transform.position);
            }
            if (surroundings.nearestWaterTile != Coord.invalid) {
                Gizmos.DrawLine (transform.position, Environment.tileCentres[surroundings.nearestWaterTile.x, surroundings.nearestWaterTile.y]);
            }

            if (currentAction == CreatureAction.GoingToFood) {
                var path = EnvironmentUtility.GetPath (coord.x, coord.y, foodTarget.coord.x, foodTarget.coord.y);
                Gizmos.color = Color.black;
                for (int i = 0; i < path.Length; i++) {
                    Gizmos.DrawSphere (Environment.tileCentres[path[i].x, path[i].y], .2f);
                }
            }
        }
    }

}