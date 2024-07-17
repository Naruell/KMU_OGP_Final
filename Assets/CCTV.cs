using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CCTV : MonoBehaviour
{
    [Header("Member Variable")]
    public GameObject RotateObject;

    private float minAngle;
    private float maxAngle;
    private int rotateDirection;
    private float currentAngle;

    private void Start()
    {
        minAngle = -EnemyManager.Instance.RotateAngleRange / 2;
        maxAngle = EnemyManager.Instance.RotateAngleRange / 2;
        rotateDirection = 1;

        currentAngle = 0;
    }

    void OnDrawGizmos()
    {
        if (EnemyManager.Instance.DrawCCTVAlertRange)
        {
            Gizmos.DrawWireSphere(transform.position, EnemyManager.Instance.AlertRange);
        }
    }

    private void Update()
    {

        if (rotateDirection == 1 && currentAngle >= maxAngle)
        {
            currentAngle = maxAngle;
            rotateDirection = -1;
        }
        else if (rotateDirection == -1 && currentAngle <= minAngle)
        {
            currentAngle = minAngle;
            rotateDirection = 1;
        }

        currentAngle += rotateDirection * EnemyManager.Instance.RotateSpeed * Time.deltaTime;
        RotateObject.transform.localRotation = Quaternion.Euler(50, currentAngle, 0);
    }

    void OnTriggerEnter(Collider other)
    {
        Player player = other.transform.GetComponent<Player>();
        if (player)
        {
            EnemyManager.Instance.AlertEnemy(this);
            Debug.Log("CCTV Alert!");
        }
    }
}
