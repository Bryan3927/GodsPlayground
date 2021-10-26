using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plant : LivingEntity {
    public float amountRemaining = 1;
    const float consumeSpeed = 8;

    private float timeOfDeath;
    private float timeSinceLastRegen;

    public float rebirthTime;
    public float regenTime;

    public float baseRebirthTime = 8.0f;
    public float baseRegenTime = 2.5f;

    public override float Consume (float amount) {
        float amountConsumed = Mathf.Max (0, Mathf.Min (amountRemaining, amount));
        amountRemaining -= amount * consumeSpeed;

        if (amountRemaining <= 0) {
            Die (CauseOfDeath.Eaten);
        }

        return amountConsumed;
    }

    private void Update()
    {
        UpdateSpeeds();
        if (dead && Time.time - timeOfDeath > rebirthTime)
        {
            dead = false;
            amountRemaining = 0.1f;
            Environment.RegisterBirth(this);
        } else if (!dead)
        {
            HandleRegen();
        }
        if (amountRemaining >= 0)
        {
            transform.localScale = Vector3.one * amountRemaining;
        }
    }

    protected override void Die(CauseOfDeath cause)
    {
        if (!dead)
        {
            dead = true;
            timeOfDeath = Time.time;
            Environment.RegisterDeath(this);
        }
    }

    private void HandleRegen()
    {
        if (Time.time - timeSinceLastRegen > regenTime && amountRemaining < 1)
        {
            amountRemaining = Mathf.Clamp01(amountRemaining + 0.1f);
            timeSinceLastRegen = Time.time;
        }
    }

    public float AmountRemaining {
        get {
            return amountRemaining;
        }
    }

    void UpdateSpeeds()
    {
        float simSpeed = Environment.GetSimSpeed();
        float simSpeedFraction = 1.0f / simSpeed;

        rebirthTime = baseRebirthTime * simSpeedFraction;
        regenTime = baseRegenTime * simSpeedFraction;
    }
}