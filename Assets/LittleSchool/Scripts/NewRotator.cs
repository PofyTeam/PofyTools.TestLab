using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewRotator: MonoBehaviour  {

    public float rotationSpeed = 4.7f;

    private void Update()
    {
        this.transform.Rotate(Vector3.up, this.rotationSpeed * Time.deltaTime);   
    }

    static void TakeTheLaraDogForAWalk ()
    {
        Dog ourDog = new Dog();
        ourDog.name = "Lara";
        ourDog.color = new Color(0, 0, 0, 1);
        ourDog.gender = Gender.Female;
        ourDog.age = 13;
        ourDog.numberOfLegs = Dog.MaxNumberOfLegs;
    }

    static string larasColor = "black";
    }

public class Dog
{
    public string name;
    public Color color;
    public Gender gender;
    public int age;
    public const int MaxNumberOfLegs = 4;
    public int numberOfLegs;
}

public enum Gender {

    Female, 
    Male,
    Other
}