using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotater : MonoBehaviour
{
    void FixedUpdate()
    {
        transform.Rotate(0, 30, 0);
    }
}
