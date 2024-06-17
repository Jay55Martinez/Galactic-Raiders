using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyPos Template", menuName = "ScriptableObjects/EnemyPos")]
public class EnemyPos : ScriptableObject
{
    public bool smartEnemy;
    public Vector3 spawnPoint;
    public Vector3[] patrolPoints;
}