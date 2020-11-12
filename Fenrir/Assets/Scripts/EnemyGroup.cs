using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyGroup_", menuName = "Enemy Group")]
public class EnemyGroup : ScriptableObject
{
    public GameObject[] enemies;
}
