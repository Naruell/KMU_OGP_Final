using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager Instance;

    [Header("Sight")]
    public float DefaultViewAngle;
    public float DefaultViewRadius;
    public float WarningViewAngle;
    public float WarningViewRadius;

    public LayerMask PlayerMask;
    public LayerMask ObstacleMask;

    [Header("AI")]
    public float WarningTime;
    public float MaxDistance;
    public float PatrolReachDistance;

    [Header("CCTV")]
    public float RotateSpeed;
    public float RotateAngleRange;
    public float AlertRange;
    public float AlertDuration;

    [Header("Debug")]
    public bool DrawEnemySight;
    public bool DrawPatrolPath;
    public bool DrawCCTVAlertRange;


    private List<Enemy> enemies = new List<Enemy>();

    public void Awake()
    {
        Instance = this;
    }

    public void RegisterEnemy(Enemy enemy)
    {
        enemies.Add(enemy);
    }

    public void AlertEnemy(CCTV cctv)
    {
        var pos = cctv.transform.position;
        foreach(Enemy enemy in enemies)
        {
            if (Vector3.Distance(enemy.transform.position, pos) <= AlertRange)
            {
                enemy.currentState = EEnemyState.RemoteFound;
            }
            else
            {
                enemy.currentState = EEnemyState.Warning;
            }
        }
    }

    public void SetEnemyIdle()
    {
        foreach (Enemy enemy in enemies)
        {
            enemy.currentState = EEnemyState.Idle;
        }
    }
}
