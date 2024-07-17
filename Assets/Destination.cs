using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destination : MonoBehaviour
{
    public GameObject DestroyTarget;
    public GameObject EnableTarget;

    private void OnTriggerEnter(Collider other)
    {
        Player player = other.transform.GetComponent<Player>();
        if (player)
        {

            Destroy(DestroyTarget);
            EnableTarget.SetActive(true);

            EnemyManager.Instance.SetEnemyIdle();
        }
    }
}
