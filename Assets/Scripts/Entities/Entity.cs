using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;


public class Entity : MonoBehaviour {
    [SerializeField] internal SliderBar healthBar;

    private float stunned = 0.0f;
    private float knockedUp = 0.0f;
    private float rooted = 0.0f;
    private float silenced = 0.0f;

    [SerializeField] protected float maxHealthPoints = 500.0f;
    [SerializeField] protected float maxManaPoints = 250.0f;
    [SerializeField] protected float healthPerSecond = 5.0f;
    [SerializeField] protected float manaPerSecond = 2.0f;
    [SerializeField] protected float currentHealthPoints;
    [SerializeField] protected float currentManaPoints;

    [SerializeField] protected float armorPoints = 20.0f;
    [SerializeField] protected float attackDamage = 50.0f;
    [SerializeField] protected float attackRange = 8.0f;
    [SerializeField] protected float moveSpeed = 6.0f;

    public float getMoveSpeed => moveSpeed;
    public float getAttackRange => attackRange;

    public float GetAttackDamage() {
        return attackDamage;
    }

    internal bool CanCastAbilities() {
        return stunned <= 0.0f && knockedUp <= 0.0f && silenced <= 0.0f;
    }

    private bool CanMove() {
        return stunned <= 0.0f && knockedUp <= 0.0f && rooted <= 0.0f;
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
        if (currentManaPoints < manaCost) {
            return false;
        }

        currentManaPoints -= manaCost;
        return true;
    }

    public void Stun(float time) {
        stunned = Mathf.Max(stunned, time);
    }

    public bool Move(Vector3 eulerAngles, Vector3 position) {
        if (!CanMove()) return false;

        transform.position = position;
        transform.eulerAngles = eulerAngles;

        return true;
    }

    private void FixedUpdate() {
        float dt = Time.deltaTime;
        
        // Update status effects
        stunned -= dt;
        knockedUp -= dt;
        silenced -= dt;
        rooted -= dt;

        // Health/Mana regen
        currentHealthPoints += healthPerSecond * dt;
        currentManaPoints += manaPerSecond * dt;

        // Limit to max health/mana
        currentHealthPoints = currentHealthPoints >= maxHealthPoints ? maxHealthPoints : currentHealthPoints;
        currentManaPoints = currentManaPoints >= maxManaPoints ? maxManaPoints : currentManaPoints;
        
        OnFixedUpdate(dt);
        
        healthBar.SetValue(currentHealthPoints, maxHealthPoints);
    }

    protected virtual void OnFixedUpdate(float dt) {
        Debug.Log("PlayerAbility::UpdateCast needs to be overwritten");
    }
}
