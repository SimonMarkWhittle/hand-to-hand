using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand : MonoBehaviour {

    public const float CHIP_PERCENT = 0.5f;
    public const float STAMINA_STUN_TIME = 1f;

    [Header("Health")]
    public float currentHealth;
    public float regenTarget;
    public float regenRate;
    public float healthMax;

    [Header("Stamina")]
    public float currentStamina;
    public float staminaMax;
    public float staminaRate;

    [Header("Stun")]
    public float stunTimer;
    public bool stunned;

	// Use this for initialization
	void Start () {
        currentHealth = healthMax;
        regenTarget = healthMax;
        currentStamina = staminaMax;
        stunTimer = 0f;
        stunned = false;

        //TODO UI INITIALIZE
	}

	// Update is called once per frame
	void Update () {
        UpdateStamina();
        UpdateStun();
        UpdateHealth();
	}

    private void UpdateHealth() {
        if (currentHealth <= 0) {
            Die();
            return;
        }

        if (currentHealth < regenTarget) {
            currentHealth += Mathf.Min(regenTarget - currentHealth, regenRate * Time.deltaTime);
        }

        if (currentHealth > healthMax) {
            currentHealth = healthMax;
        }
    }

    private void UpdateStamina() {
        if (currentStamina <= 0) {
            Stun(STAMINA_STUN_TIME);
        }

        currentStamina += Mathf.Min(staminaMax - currentStamina, staminaRate * Time.deltaTime);
        //TODO Update UI
    }

    private void UpdateStun() {
        if (!stunned) { 
            return;
        }

        if (stunTimer > 0) {
            stunTimer -= Time.deltaTime;
        }
        else {
            stunned = false;
        }
    }

    public void Stun(float stunTime) {
        stunned = true;

        stunTimer = Mathf.Max(stunTimer, stunTime);
    }

    public void Damage(float amount) {
        currentHealth -= Mathf.Min(currentHealth, amount);
        regenTarget = currentHealth + (amount * CHIP_PERCENT);
    }

    public bool UseStamina(float amount) {
        if (currentStamina < 0) {
            return false;
        }

        currentStamina -= Mathf.Min(currentStamina, amount);
        return true;
    }

    public void Die() {
        //TODO Update UI, send message, whatever
        Debug.Log("DIE, MORON"); //WILL DIE
        stunTimer = 1000000f;
    }

	public void Knock(Vector2 force) {
		//TODO:
		//Knock back by specified force
	}
}
