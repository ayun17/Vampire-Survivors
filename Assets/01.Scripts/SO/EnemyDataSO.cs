using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/EnemyData")]
public class EnemyDataSO : ScriptableObject
{
    public int maxHP = 0;
    public float maxSpeed = 0;

    public float attackDistance = 0;
}
