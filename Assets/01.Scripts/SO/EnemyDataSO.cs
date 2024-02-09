using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/EnemyData")]
public class EnemyDataSO : ScriptableObject
{
    public float damage = 0;
    public float enemyHp = 0;
    public float enemySpeed = 0;
}
