using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;


public class Entity : MonoBehaviour {
    [SerializeField] internal SliderBar manaBar;
    [SerializeField] internal SliderBar healthBar;

    private float stunned = 0.0f;
    private float knockedUp = 0.0f;
    private float rooted = 0.0f;
    private float silenced = 0.0f;
    private float blinded = 0.0f;
    private float slowed = 0.0f;
    private float slowPercentage = 0.0f;

    [SerializeField] protected float maxHealthPoints;
    [SerializeField] protected float healthPerSecond;
    [SerializeField] protected float currentHealthPoints;

    [SerializeField] protected float maxManaPoints;
    [SerializeField] protected float manaPerSecond;
    [SerializeField] protected float currentManaPoints;

    [SerializeField] protected float armor;
    [SerializeField] protected float magicResist;
    [SerializeField] protected float moveSpeed;

    public float GetMoveSpeed => moveSpeed;

    public bool CanCastAbilities() {
        return stunned <= 0.0f && knockedUp <= 0.0f && silenced <= 0.0f;
    }

    public bool CanMove() {
        return stunned <= 0.0f && knockedUp <= 0.0f && rooted <= 0.0f;
    }

    public bool CanAttack() {
        return stunned <= 0.0f && knockedUp <= 0.0f && blinded <= 0.0f;

    }

    protected virtual void Initialize() {
        Debug.Log("Entity::Initialize needs to be overwritten");
    }

    private void Start() {
        Initialize();

        currentHealthPoints = maxHealthPoints;
        currentManaPoints = maxManaPoints;
        
        healthBar.SetValue(currentHealthPoints, maxHealthPoints);
    }

    public void TakeDamage(float value) {
        currentHealthPoints -= value;

        if (currentHealthPoints <= 0.0f) {
            Destroy(gameObject);
        }

        healthBar.SetValue(currentHealthPoints, maxHealthPoints);
    }

    public bool RemoveMana(float manaCost) {
        bool hasMana = HasMana(manaCost);
        if (hasMana) {
            currentManaPoints -= manaCost;
        }

        return hasMana;
    }

    public bool HasMana(float manaCost) {
        return currentManaPoints >= manaCost;
    }

    public void Stun(float time) {
        stunned = Mathf.Max(stunned, time);
    }
    
    public void Blind(float time) {
        blinded = Mathf.Max(blinded, time);
    }
    
    public void KnockUp(float time) {
        knockedUp = Mathf.Max(knockedUp, time);
    }
    
    public void Root(float time) {
        rooted = Mathf.Max(rooted, time);
    }
    
    public void Silence(float time) {
        silenced = Mathf.Max(silenced, time);
    }

    public void Slow(float slowPercent_, float slowDuration_) {
        slowPercentage = slowPercent_;
        slowed = slowDuration_;
    }
    
    public bool Move(Vector3 eulerAngles, Vector3 position) {
        if (!CanMove()) return false;

        //TODO: Limit movement here! (add dt, check if not exceeding movespeed)
        
        if (slowed > 0.0f) {
            transform.position = 0.01f * slowPercentage * transform.position +
                                 0.01f * (100.0f - slowPercentage) * position;
            
            transform.eulerAngles = eulerAngles;
        }
        else {
            transform.position = position;
            transform.eulerAngles = eulerAngles;
        }

        return true;
    }

    private void FixedUpdate() {
        float dt = Time.deltaTime;
        
        // Update status effects
        stunned -= dt;
        knockedUp -= dt;
        silenced -= dt;
        rooted -= dt;
        blinded -= dt;
        slowed -= dt;
        
        // Health/Mana regen
        currentHealthPoints += healthPerSecond * dt;
        currentManaPoints += manaPerSecond * dt;

        // Limit to max health/mana
        currentHealthPoints = currentHealthPoints >= maxHealthPoints ? maxHealthPoints : currentHealthPoints;
        currentManaPoints = currentManaPoints >= maxManaPoints ? maxManaPoints : currentManaPoints;
        
        OnFixedUpdate(dt);
        
        healthBar.SetValue(currentHealthPoints, maxHealthPoints);

        if (maxManaPoints > 0.0f) {
            manaBar.SetValue(currentManaPoints, maxManaPoints);
        }
    }

    protected virtual void OnFixedUpdate(float dt) {
        Debug.Log("PlayerAbility::UpdateCast needs to be overwritten");
    }


}
