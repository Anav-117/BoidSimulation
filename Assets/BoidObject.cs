// Script applied to each Boid prefab instance
// Stores velocity and handles local rotation

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoidObject : MonoBehaviour
{
    public Vector2 velocity;

    // Update is called once per frame
    void Update()
    {
        transform.up = velocity; // Rotation to esure boid is aligned with the velocity to give a steering effect
    }
}
