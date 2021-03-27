using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingController : MonoBehaviour
{
    public enum BuildingStates { Intact, Destroyed }
    public BuildingStates State = BuildingStates.Intact;
    public GameManager.ClassTypes type;
    public GameManager.GameTeams Team;
    public BaseManager manager;
    protected float healthPoints = 50;

    private void Update()
    {
        UpdateIntegrity();

        switch (State)
        {
            case BuildingStates.Destroyed:
                DestroySelf();
                break;
            case BuildingStates.Intact:
            default:
                break;
        }
    }

    /// <summary>
    /// Called by this building's base Manager. Receive Damage based on a unit's attackDamage.
    /// </summary>
    /// <param name="receivedDamage"></param>
    public virtual void TakeDamage(float receivedDamage)
    {
        Debug.Log("TAKE DAMAGE CALLED ON " + this);
        if (healthPoints > 0)
            healthPoints -= receivedDamage;
        else
        {
            DestroySelf();
        }
    }

    /// <summary>
    /// Check the healthPoints of this building and update
    /// </summary>
    protected virtual void UpdateIntegrity()
    {
        if (healthPoints <= 0)
        {
            this.State = BuildingStates.Destroyed;
        }
        else
        {
            this.State = BuildingStates.Intact;
        }
    }

    /// <summary>
    /// Tell Manager to Remove this building from its Team's Building List and Destroy this object.
    /// </summary>
    protected virtual void DestroySelf()
    {
        manager.RemoveBuildingFromBase(this.gameObject);
    }

}
