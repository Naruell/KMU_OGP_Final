using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destination : MonoBehaviour
{
    public GameObject DestroyTarget;

    private void OnTriggerEnter(Collider other)
    {
        Player player = other.transform.GetComponent<Player>();
        if (player)
        {

            Destroy(DestroyTarget);

            EnemyManager.Instance.SetEnemyIdle();
        }
    }
}
