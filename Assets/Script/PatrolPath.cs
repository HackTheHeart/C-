using System.Collections.Generic;
using UnityEngine;

public class PatrolPath : MonoBehaviour
{
    public List<Transform> patrolPoints;
    public Vector2 GetPatrolPoint(int index)
    {
        return patrolPoints[index].position;
    }
    public int GetPatrolCount()
    {
        return patrolPoints.Count;
    }
}
