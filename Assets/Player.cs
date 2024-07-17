using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public static Player Instance;

    public GameObject Exit;

    public Collider Collider;

    public bool HasKey;

    private void Awake()
    {
        Instance = this;
        HasKey = false;
    }

    public void Die()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void DetectedByEnemy(Transform transform)
    {
        Debug.Log("Detect");
    }

    public void ReleasedByEnemy()
    {
        Debug.Log("Release");
    }

    public void ActiveExitObject()
    {
        Exit.SetActive(true);
    }
}
