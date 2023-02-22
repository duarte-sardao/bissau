using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyUnitLogic : UnitLogic
{
    private PathCalculator calc;
    private float healthTarget;
    private void Start()
    {
        sector.enemy = this;
        guinean = false;
        healthTarget = 0f;
        calc = FindObjectOfType<PathCalculator>();
        InvokeRepeating(nameof(CheckMove), 1f, 1f);
    }

    private void CheckMove()
    {
        if(this.health < 0.15f && (this.sector.ControlLevel < 100 || this.sector.friend != null))
        {
            calc.GetRetreatForEnemy(this.sector);
            healthTarget = Random.Range(0.7f, 1);
        } else if(this.health >= healthTarget)
        {
            calc.GetSpotForEnemy(this.sector);
            healthTarget = 100;
        }
    }
}
