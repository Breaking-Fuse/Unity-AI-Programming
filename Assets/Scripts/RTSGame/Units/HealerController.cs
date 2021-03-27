using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealerController : UnitController
{
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        Type = GameManager.ClassTypes.Healer;
        this.maxHealthPoints = 10;
        this.healthPoints = 10;
        this.attackDamage = 5; //HEALS instead
        this.maxAttackRange = 30;
        this.attackCooldown = 3;
        this.State = UnitManager.UnitStates.Defending;
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        if (CanAttack())
            HealTeam(this.transform.position, 30);
    }

    private void HealTeam(Vector3 center, float radius)
    {
        Collider[] hitColliders = Physics.OverlapSphere(center, radius);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.GetComponentInParent<UnitController>())
            {
                if (hitCollider.GetComponentInParent<UnitController>().Team == this.Team
                    && hitCollider.GetComponentInParent<UnitController>().Type != GameManager.ClassTypes.Healer)
                {
                    hitCollider.transform.parent.SendMessageUpwards("GetHealth", attackDamage);
                    target = hitCollider.gameObject;
                    State = UnitManager.UnitStates.Defending;
                }
            }
        }
    }
}
