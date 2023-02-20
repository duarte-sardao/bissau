using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyUnitLogic : UnitLogic
{
    private PathCalculator calc;
    private void Start()
    {
        sector.enemy = this;
        guinean = false;
        calc = FindObjectOfType<PathCalculator>();
        StartCoroutine(LateStart(0.33f));
        InvokeRepeating(nameof(CheckRetreat), 1f, 1f);
    }

    IEnumerator LateStart(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        calc.GetSpotForEnemy(this.sector);
    }

    private void CheckRetreat()
    {
        if(this.health < 0.15f && (this.sector.ControlLevel < 100 || this.sector.friend != null))
        {
            calc.GetRetreatForEnemy(this.sector);
        }
    }
}
