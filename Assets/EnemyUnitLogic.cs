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
    }

    IEnumerator LateStart(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        calc.GetSpotForEnemy(this.sector);
    }
}
