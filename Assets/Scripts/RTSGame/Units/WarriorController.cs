using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarriorController : UnitController
{

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        Type = GameManager.ClassTypes.Warrior;
        this.maxHealthPoints = 20;
        this.healthPoints = 20;
        this.attackDamage = 15;
        this.maxAttackRange = 4;
        this.attackCooldown = 2;
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        Taunt(this.transform.position, 30);
    }

    private void Taunt(Vector3 center, float radius)
    {
        Collider[] hitColliders = Physics.OverlapSphere(center, radius);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.GetComponentInParent<UnitController>())
            {
                if (hitCollider.GetComponentInParent<UnitController>().Team != this.Team)
                {
                    this.State = UnitManager.UnitStates.Taunting;
                    hitCollider.transform.parent.SendMessageUpwards("BecomeTaunted", this.gameObject);
                }
            }
        }

        
    }

}
