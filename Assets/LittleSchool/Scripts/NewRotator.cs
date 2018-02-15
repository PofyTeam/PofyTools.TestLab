using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewRotator: MonoBehaviour  {

    public float rotationSpeed = 4.7f;

    private void Update()
    {
        this.transform.Rotate(Vector3.up, this.rotationSpeed * Time.deltaTime);   
    }
    
}
