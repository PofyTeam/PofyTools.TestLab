using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTarget : MonoBehaviour
{

	public Transform target;
	private Transform _selfTransform;
	// Use this for initialization
	void Start ()
	{
		this._selfTransform = this.transform;
	}
	
	// Update is called once per frame
	void Update ()
	{
		this._selfTransform.LookAt (this.target);
	}
}
