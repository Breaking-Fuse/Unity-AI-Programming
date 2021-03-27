using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RogueController : UnitController
{
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        Type = GameManager.ClassTypes.Rogue;
        this.maxHealthPoints = 5;
        this.healthPoints = 5;
        this.attackDamage = 5;
        this.maxAttackRange = 2;
        this.attackCooldown = 1.5f;
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }
}
