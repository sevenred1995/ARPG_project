using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
public class EnemyAnimationModel {
    public List<EnemyAnimationProperty> list = new List<EnemyAnimationProperty>();
}
public class EnemyAnimationProperty {
    public string guid { get; set; }
    public bool isIdle { get; set; }
    public bool isAttack { get; set; }
    public bool isWalk { get; set; }
    public bool isDie { get; set; }
    public bool isTakeDamage { get; set; }
}