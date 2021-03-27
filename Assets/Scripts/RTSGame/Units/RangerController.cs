using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangerController : UnitController
{ 
    // Start is called before the first frame update
    protected override void Start()
    {        
        base.Start();
        Type = GameManager.ClassTypes.Ranger;
        this.maxHealthPoints = 10;
        this.healthPoints = 10;
        this.attackDamage = 2;
        this.maxAttackRange = 30;
        this.attackCooldown = 1;
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }
}
