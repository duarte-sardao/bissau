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
        InitRenderer();
        lineRenderer.startColor = Color.gray;
    }

    private void CheckMove()
    {
        if (moving || (healthTarget > health && this.sector.friend == null))
            return;
        if(this.health < 0.15f && (this.sector.ControlLevel <= g_fullcontrolp || this.sector.friend != null))
        {
            calc.GetRetreatForEnemy(this.sector);
            healthTarget = Random.Range(0.7f, 1);
            Debug.Log("retreating and healing until " + healthTarget);
        } else if(this.health >= healthTarget && this.sector.ControlLevel >= g_fullcontrolp)
        {
            calc.GetSpotForEnemy(this.sector);
            healthTarget = 0f;
            Debug.Log("attacking");
        }
    }
}
