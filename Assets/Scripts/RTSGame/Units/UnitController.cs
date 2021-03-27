using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class UnitController : MonoBehaviour
{
    public UnitManager.UnitStates State;
    public GameManager.GameTeams Team;
    public GameManager.ClassTypes Type;
    public UnitManager manager;
    public GameObject target;


    #region Stats/Cooldowns Vars
    [SerializeField] protected float maxHealthPoints = 50;
    [SerializeField] protected float healthPoints = 50;
    [SerializeField] protected float attackDamage = 10;
    protected float maxAttackRange = 3f;
    [SerializeField] protected float attackCooldown = 2f;
    [SerializeField] protected float timeSinceLastAttack = 0f;
    #endregion


    // Start is called before the first frame update
    protected virtual void Start()
    {
        timeSinceLastAttack = attackCooldown;
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if (target != null)
        {
            this.GetComponent<NavMeshAgent>().SetDestination(target.transform.position);
        }
        else if (target == null)
        {
            this.State = UnitManager.UnitStates.Searching;
        }

        if (HasLowHealth())
            State = UnitManager.UnitStates.Fleeing;
        CheckForDeath();

        CheckForEnemies();

        switch (this.State)
        {
            case UnitManager.UnitStates.Searching:
                target = manager.rivalBase;
                this.GetComponent<NavMeshAgent>().SetDestination(target.transform.position);
                break;
            case UnitManager.UnitStates.AttackingUnit:
                Attack("unit");
                break;
            case UnitManager.UnitStates.AttackingBase:
                Attack("base");
                break;
            case UnitManager.UnitStates.Defending:
                Attack("unit");
                break;
            case UnitManager.UnitStates.Fleeing:
                //Get Back into the Action if health is at least half full
                if (!HasLowHealth())
                    this.State = UnitManager.UnitStates.Searching;
                else
                {
                    //Keep Fleeing
                    target = manager.baseManager.gameObject;
                }
                break;
            case UnitManager.UnitStates.Taunting:
                Attack("unit");
                break;
            case UnitManager.UnitStates.Dead:
                DestroySelf();
                break;
            default:
                this.State = UnitManager.UnitStates.Searching;
                break;
        }
    }

    /// <summary>
    /// Check for an Enemy Base or Enemy Units near this unit.
    /// </summary>
    protected virtual void CheckForEnemies()
    {
        Collider[] hitColliders = Physics.OverlapSphere(this.transform.position, 20);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.GetComponent<BaseManager>())
            {
                if (hitCollider.GetComponent<BaseManager>().Team != this.Team && CanAttack())
                {
                    timeSinceLastAttack = 0f;
                    hitCollider.GetComponent<BaseManager>().TakeDamage(attackDamage);
                    target = hitCollider.gameObject;
                    State = UnitManager.UnitStates.AttackingBase;
                    return;
                }
            }
            else if (hitCollider.GetComponent<UnitController>())
            {
                if (hitCollider.GetComponent<UnitController>().Team != this.Team && CanAttack())
                {
                    timeSinceLastAttack = 0f;
                    hitCollider.GetComponent<UnitController>().TakeDamage(attackDamage);
                    target = hitCollider.gameObject;
                    State = UnitManager.UnitStates.AttackingUnit;
                    return;
                }
            }
        }
        
        if (Vector3.Distance(this.transform.position, manager.rivalBase.transform.position) <= 2)
        {
            target = manager.rivalBase;
            this.State = UnitManager.UnitStates.AttackingBase;
        }
    }

    /// <summary>
    /// If this unit is in range, attack its current target. If target == null, target the rivalBase.
    /// </summary>
    protected virtual void Attack(string unitOrBase)
    {
        #region Early Break (Null Target/Cooldown Incomplete)
        if (!CanAttack() || target == null)
        {
            this.State = UnitManager.UnitStates.Searching;
            return;
        }
        #endregion

        this.GetComponent<NavMeshAgent>().SetDestination(target.transform.position);

        if (Vector3.Distance(target.transform.position, this.transform.position) <= maxAttackRange)
        {
            this.GetComponent<NavMeshAgent>().isStopped = true;
            timeSinceLastAttack = 0f;

            switch (unitOrBase)
            {
                case "unit":
                    //Attack a Unit
                    this.State = UnitManager.UnitStates.AttackingUnit;
                    if (target.GetComponent<UnitController>())
                        target.GetComponent<UnitController>().TakeDamage(attackDamage);
                    break;
                case "base":
                    //Attack a building in the base
                    this.State = UnitManager.UnitStates.AttackingBase;
                    if (target.GetComponent<BaseManager>())
                        target.GetComponent<BaseManager>().TakeDamage(attackDamage); // Rival Base chooses a random Building to receive this damage
                    break;
            }
        }
    }

    /// <summary>
    /// Check if Attack Cooldown is complete.
    /// </summary>
    /// <returns></returns>
    protected virtual bool CanAttack()
    {
        timeSinceLastAttack += Time.deltaTime;

        if (timeSinceLastAttack > attackCooldown)
        {
            return true;
        }

        return false;
    }

    /// <summary>
    /// Give health to this unit from a Healer Unit
    /// </summary>
    /// <param name="receivedHealth"></param>
    public virtual void GetHealth(float receivedHealth)
    {
        Debug.Log(this + "got HP");
        healthPoints += receivedHealth;
        if (healthPoints > maxHealthPoints)
            healthPoints = maxHealthPoints;
    }

    /// <summary>
    /// Receive Damage to this unit's healthPoints. Change State to Dead if HP <= 0.
    /// </summary>
    /// <param name="receivedDamage"></param>
    public virtual void TakeDamage(float receivedDamage)
    {
        if (healthPoints > 0)
            this.healthPoints -= receivedDamage;
        else
        {
            this.State = UnitManager.UnitStates.Dead;
        }
    }

    /// <summary>
    /// Change this unit's Attack Target to the unit that has successfully Taunted it.
    /// </summary>
    public virtual void BecomeTaunted(GameObject newTarget)
    {
        target = newTarget;
        this.State = UnitManager.UnitStates.AttackingUnit;
        Debug.Log($"{this} is taunted by {newTarget}.");
        this.GetComponent<NavMeshAgent>().SetDestination(target.transform.position);
    }

    /// <summary>
    /// Check if this unit's health is less than half of its MaxHP.
    /// </summary>
    /// <returns></returns>
    protected virtual bool HasLowHealth()
    {
        if (this.healthPoints < (maxHealthPoints / 2))
            return true;
        else
        {
            return false;
        }
    }

    /// <summary>
    /// Change this unit's State to Dead if health is at/below 0.
    /// </summary>
    protected virtual void CheckForDeath()
    {
        if (healthPoints <= 0)
            this.State = UnitManager.UnitStates.Dead;
    }

    /// <summary>
    /// Tell Manager to Remove this unit from its Team's Unit List and Destroy this object.
    /// </summary>
    protected virtual void DestroySelf()
    {        
        manager.RemoveFromUnitList(this.gameObject);
        Destroy(this.gameObject);
    }

}
