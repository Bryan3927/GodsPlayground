using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animal : LivingEntity {

    public const int maxViewDistance = 10;

    [EnumFlags]
    public Species diet;

    public CreatureAction currentAction;
    public Genes genes;
    public Color maleColour;
    public Color femaleColour;

    // Virtual settings after being multiplied by Sim Speed
    protected float 
        timeBetweenActionChoices,
        moveSpeed,
        timeToDeathByHunger,
        timeToDeathByThirst,
        timeToDeathByHorny,
        drinkDuration,
        eatDuration,
        timeToGrow,
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
        baseMateTime,
        baseGestationPeriod,
        baseMateWaitTime;

    float scale = 0.6f;
    float mateStartTime;
    float criticalPercent = 0.7f;

    bool pregnant = false;
    float pregnantTime;

    bool acceptedMateRequest = false;

    public bool isMale;

    // Visual settings:
    float moveArcHeight = .2f;

    // State:
    [Header ("State")]
    public float hunger;
    public float thirst;
    public float horny;

    public StatsBar hungerBar;
    public StatsBar thirstBar;
    public StatsBar hornyBar;

    protected LivingEntity foodTarget;
    protected Coord waterTarget;
    protected Animal mateTarget = null;

    // Traits (Fun stuff!)
    protected List<Trait> traits = new List<Trait>();

    // Move data:
    bool animatingMovement;
    Coord moveFromCoord;
    Coord moveTargetCoord;
    Vector3 moveStartPos;
    Vector3 moveTargetPos;
    float moveTime;
    float moveSpeedFactor;
    float moveArcHeightFactor;
    Coord[] path;
    int pathIndex;

    // Other
    float lastActionChooseTime;
    const float sqrtTwo = 1.4142f;
    const float oneOverSqrtTwo = 1 / sqrtTwo;
    float waitingForMate;

    public override void Init (Coord coord) {
        base.Init (coord);
        ConstantsUtility.SetConstants(this);
        moveFromCoord = coord;
        genes = Genes.RandomGenes (1);

        material.color = (genes.isMale) ? maleColour : femaleColour;

        isMale = genes.isMale;

        hungerBar.SetMaxStat();
        thirstBar.SetMaxStat();
        hornyBar.SetMaxStat();

        ChooseNextAction ();
    }



    protected virtual void Update () {

        UpdateSpeeds();

        // Increase hunger and thirst over time
        hunger += Time.deltaTime * 1 / timeToDeathByHunger;
        thirst += Time.deltaTime * 1 / timeToDeathByThirst;
        if (!pregnant) horny += Time.deltaTime * 1 / timeToDeathByHorny;

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

        //traits default override unless conditionally specified within each trait apply method
        foreach(Trait t in traits){
            t.Apply(this);
        }

        if (pregnant && Time.time - pregnantTime > gestationPeriod)
        {
            HandleBirth();
            pregnant = false;
        }

        if (hunger >= 1) {
            Die (CauseOfDeath.Hunger);
        } else if (thirst >= 1) {
            Die (CauseOfDeath.Thirst);
        } else if (horny >= 1)
        {
            horny = 1.0f;
        }

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
        bool currentlyEating = currentAction == CreatureAction.Eating && foodTarget && hunger > 0;
        bool currentlyDrinking = currentAction == CreatureAction.Drinking && thirst > 0;
        bool currentlyMating = currentAction == CreatureAction.Mating && horny > 0;
        bool currentlyWaiting = currentAction == CreatureAction.WaitingForMate;
        if ((currentlyWaiting || acceptedMateRequest) && hunger < criticalPercent && thirst < criticalPercent)
        {
            currentAction = CreatureAction.WaitingForMate;
        } else if (currentlyEating && thirst < criticalPercent)
        {
            FindFood();
        } else if (currentlyDrinking && hunger < criticalPercent)
        {
            FindWater();
        } else if (currentlyMating && hunger < criticalPercent && thirst < criticalPercent) {
            //FindMate();
        } 
        else if ((hunger >= thirst && hunger >= horny && hunger > 0.1) || hunger > criticalPercent)
        {
            FindFood();
        } else if ((thirst >= hunger && thirst >= horny && thirst > 0.1) || thirst > criticalPercent)
        {
            FindWater();
        } else if (!pregnant && (horny > hunger && horny > thirst && horny > 0.1))
        {
            FindMate();
        } else
        {
            currentAction = CreatureAction.Exploring;
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
        Coord waterTile = Environment.SenseWater (coord);
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
        // If no mate target, search for mate
        if (!mateTarget) currentAction = CreatureAction.SearchingForMate;
        // If mate target exists and this is not a waiting female, create path to mate target
        if (mateTarget && currentAction != CreatureAction.WaitingForMate)
        {
        } 
        // else if no mate target and this is male, look for mate target
        if (genes.isMale && !mateTarget)
        {
            foreach (Animal female in potentialMates)
            {
                bool accepted = female.AskToMate(this);
                if (accepted)
                {
                    currentAction = CreatureAction.GoingToMate;
                    mateTarget = female;
                    CreatePath(female.coord);
                }
            }
        }
    }

    public bool AskToMate(Animal male)
    {
        // TODO: Add rejection logic if needed/desired
        //LookAt(male.coord);
        if (acceptedMateRequest)
        {
            return false;
        }
        waitingForMate = Time.time;
        mateTarget = male;
        acceptedMateRequest = true;
        return true;
    }

    public void CancelMate()
    {
        mateTarget = null;
        acceptedMateRequest = false;
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
                    StartMoveToCoord (path[pathIndex]);
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
                } else if (Coord.AreNeighbours(coord, mateTarget.coord))
                {
                    LookAt(mateTarget.coord);
                    currentAction = CreatureAction.Mating;
                    acceptedMateRequest = false;
                    mateStartTime = Time.time;
                } else
                {
                    // do nothing lol
                }
                break;
        }
    }

    protected void CreatePath (Coord target) {
        // Create new path if current is not already going to target
        if (path == null || pathIndex >= path.Length || (path[path.Length - 1] != target || path[Mathf.Clamp(pathIndex - 1, 0, path.Length)] != moveTargetCoord)) {
            path = EnvironmentUtility.GetPath (coord.x, coord.y, target.x, target.y);
            pathIndex = 0;
        }
    }

    protected void StartMoveToCoord (Coord target) {
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
                mateTarget = null;
                if (!genes.isMale)
                {
                    pregnant = true;
                    pregnantTime = Time.time;
                }
            }
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
        mateTime = baseMateTime * simSpeedFraction;
        gestationPeriod = baseGestationPeriod * simSpeedFraction;
        mateWaitTime = baseMateWaitTime * simSpeedFraction;
    }

    void OnDrawGizmosSelected () {
        if (Application.isPlaying) {
            var surroundings = Environment.Sense (coord);
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