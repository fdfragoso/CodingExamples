using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static UnityEditorInternal.ReorderableList;

namespace TollanWorlds.Utility
{
    public static class NavMeshHelper 
    {
        public static Vector2 GetRandomPointInCircle(Vector2 center, float radius)
        {
            if(radius==0) return center;

            Vector2 randomPos = center + UnityEngine.Random.insideUnitCircle * radius;
            NavMeshHit hit;
            NavMesh.SamplePosition(randomPos, out hit, radius, NavMesh.AllAreas);
            while(hit.position.x == Mathf.Infinity)
            { 
                randomPos = center + UnityEngine.Random.insideUnitCircle * radius;
                NavMesh.SamplePosition(randomPos, out hit, radius, NavMesh.AllAreas);
            }
            return hit.position;
        }
    }
}