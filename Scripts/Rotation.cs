using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotation : MonoBehaviour
{
    // Start is called before the first frame update
    public float sensitivity = 3; // чувствительность мышки
	private float X, Y;


	void Update()
	{
		X = transform.localEulerAngles.y + Input.GetAxis("Mouse X") * sensitivity;
		Y += Input.GetAxis("Mouse Y") * sensitivity;
		Y = Mathf.Clamp(Y, -90, 90);
		transform.localEulerAngles = new Vector3(-Y, X, 0);
	}
}
