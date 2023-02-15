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
        calc.GetSpotForEnemy(this.sector);
    }
}
