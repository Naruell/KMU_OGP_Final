using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour
{

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0, 1, 0);
    }

    private void OnTriggerEnter(Collider other)
    {
        Player player = other.transform.GetComponent<Player>();
        if (player)
        {
            Player.Instance.HasKey = true;
            Player.Instance.ActiveExitObject();

            Destroy(gameObject);
        }
    }
}
