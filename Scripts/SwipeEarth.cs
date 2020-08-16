using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwipeEarth : MonoBehaviour
{
    [SerializeField] private float sensitivity = 3;

    
    void Update()
    {
        float X = transform.localEulerAngles.y + Input.GetAxis("Mouse X") * sensitivity;
        transform.localEulerAngles = new Vector3(0.0f, X, 0);
    }
}
