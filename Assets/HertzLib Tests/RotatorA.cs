﻿using UnityEngine;

public class RotatorA : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.up * 90 * Time.deltaTime);
    }
}
