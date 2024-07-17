using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Experimental.GlobalIllumination;

public class Enemy : MonoBehaviour
{
    [Header("Patrol")]
    public List<PatrolPoint> PatrolPoints = new List<PatrolPoint>();

    [Header("Member Variable")]
    public Rigidbody rb;
    public NavMeshAgent agent;
    public Light slight;

    public EEnemyState currentState;

    private float nowviewAngle = 0f;
    private float nowviewRadius = 1f;
    private float nowWarningTime = 1f;
    private List<Collider> targetList = new List<Collider>();

    private int currentPatrolIndex;
    private int patrolDirection;

    private void Start()
    {
        currentState = EEnemyState.Idle;

        nowviewAngle = EnemyManager.Instance.DefaultViewAngle;
        nowviewRadius = EnemyManager.Instance.DefaultViewRadius;
        nowWarningTime = EnemyManager.Instance.WarningTime;

        currentPatrolIndex = 0;
        patrolDirection = 1;

        EnemyManager.Instance.RegisterEnemy(this);
    }

    private void Update()
    {
        Vector3 pos = transform.position + Vector3.up * 0.5f;

        float lookingAngle = transform.eulerAngles.y;
        Vector3 rightDir = AngleToDirection(transform.eulerAngles.y + nowviewAngle * 0.5f);
        Vector3 leftDir = AngleToDirection(transform.eulerAngles.y - nowviewAngle * 0.5f);
        Vector3 lookDir = AngleToDirection(lookingAngle);

        if (EnemyManager.Instance.DrawEnemySight)
        {
            Debug.DrawRay(pos, rightDir * nowviewRadius, Color.blue);
            Debug.DrawRay(pos, leftDir * nowviewRadius, Color.blue);
            Debug.DrawRay(pos, lookDir * nowviewRadius, Color.cyan);
        }

        targetList.Clear();
        Collider[] targets = Physics.OverlapSphere(pos, nowviewRadius, EnemyManager.Instance.PlayerMask);
        if (targets.Length != 0)
        {
            foreach (Collider col in targets)
            {
                Vector3 targetPos = col.transform.position;
                Vector3 targetDir = (targetPos - pos).normalized;
                float targetAngle = Mathf.Acos(Vector3.Dot(lookDir, targetDir)) * Mathf.Rad2Deg;
                if (targetAngle <= nowviewAngle * 0.5f && !Physics.Raycast(pos, targetDir, nowviewRadius, EnemyManager.Instance.ObstacleMask))
                {
                    targetList.Add(col);
                    if (EnemyManager.Instance.DrawEnemySight)
                    {
                        Debug.DrawLine(pos, targetPos, Color.red);
                    }
                }
            }
        }

        if (EnemyManager.Instance.DrawPatrolPath && PatrolPoints.Count != 0)
        {
            for (int i = 0; i < PatrolPoints.Count - 1; i++)
            {
                Debug.DrawLine(PatrolPoints[i].transform.position, PatrolPoints[i + 1].transform.position, Color.blue);
            }
        }

        switch (currentState)
        {
            case EEnemyState.Idle:
                {
                    slight.color = Color.white;
                    slight.type = UnityEngine.LightType.Spot;
                    if (targetList.Contains(Player.Instance.Collider))
                    {
                        currentState = EEnemyState.Found;
                    }

                    if (agent.enabled && agent.isOnNavMesh && PatrolPoints.Count > 0)
                    {
                        if (Vector3.Distance(transform.position, PatrolPoints[currentPatrolIndex].transform.position) <= EnemyManager.Instance.PatrolReachDistance)
                        {
                            if (patrolDirection == 1 && currentPatrolIndex == PatrolPoints.Count - 1)
                            {
                                patrolDirection = -1;
                            }
                            else if (patrolDirection == -1 && currentPatrolIndex == 0)
                            {
                                patrolDirection = 1;
                            }
                            currentPatrolIndex += patrolDirection;
                        }
                        agent.SetDestination(PatrolPoints[currentPatrolIndex].transform.position);
                    }

                    break;
                }
            case EEnemyState.Found:
                {
                    slight.color = Color.red;
                    slight.type = UnityEngine.LightType.Point;
                    if (agent.enabled && agent.isOnNavMesh)
                    {
                        if (targetList.Contains(Player.Instance.Collider))
                        {
                            Player.Instance.DetectedByEnemy(transform);
                            agent.SetDestination(Player.Instance.transform.position);
                        }
                        else
                        {
                            currentState = EEnemyState.Warning;
                        }
                    }
                    break;
                }
            case EEnemyState.RemoteFound:
                {
                    slight.color = Color.blue;
                    slight.type = UnityEngine.LightType.Spot;
                    if (agent.enabled && agent.isOnNavMesh)
                    {
                        agent.SetDestination(Player.Instance.transform.position);
                    }

                    if (Vector3.Distance(transform.position, Player.Instance.transform.position) > EnemyManager.Instance.MaxDistance)
                    {
                        currentState = EEnemyState.Warning;
                    }
                    break;
                }
            case EEnemyState.Warning:
                {
                    slight.color = Color.yellow;
                    slight.type = UnityEngine.LightType.Point;
                    nowviewAngle = EnemyManager.Instance.WarningViewAngle;
                    nowviewRadius = EnemyManager.Instance.WarningViewRadius;

                    agent.enabled = true;

                    rb.constraints = RigidbodyConstraints.FreezeAll;

                    rb.velocity = Vector3.zero;

                    if (targetList.Contains(Player.Instance.Collider))
                    {
                        currentState = EEnemyState.Found;
                    }

                    if (nowWarningTime > 0)
                    {
                        nowWarningTime -= Time.deltaTime;
                    }
                    else
                    {
                        Player.Instance.ReleasedByEnemy();

                        currentState = EEnemyState.Idle;

                        nowviewAngle = EnemyManager.Instance.DefaultViewAngle;
                        nowviewRadius = EnemyManager.Instance.DefaultViewRadius;
                        nowWarningTime = EnemyManager.Instance.WarningTime;
                    }
                    break;
                }
        }

        slight.innerSpotAngle = nowviewAngle - 10;
        slight.spotAngle = nowviewAngle - 10;
        slight.range = nowviewRadius;
    }

    void OnCollisionEnter(Collision other)
    {
        Player player = other.transform.GetComponent<Player>();
        if (player && currentState != EEnemyState.Sleeping)
        {
            player.Die();
        }
    }

    void OnDrawGizmos()
    {
        if (EnemyManager.Instance.DrawEnemySight)
        {
            Vector3 pos = transform.position + Vector3.up * 0.5f;
            Gizmos.DrawWireSphere(pos, nowviewRadius);
        }
    }

    private Vector3 AngleToDirection(float ang)
    {
        float radianAngle = ang * Mathf.Deg2Rad;
        return new Vector3(Mathf.Sin(radianAngle), 0, Mathf.Cos(radianAngle));
    }

    public Vector3 ReturnDirectionByQuaternion(Vector3 dir, int rot)
    {
        Quaternion rotation = Quaternion.Euler(0, rot, 0);

        return dir + rotation.eulerAngles;
    }
}
