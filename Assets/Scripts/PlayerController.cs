using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(BoxCollider))]

public class PlayerController : MonoBehaviour
{
    public Transform[] CheckPoints;

    private PlayerInput CustomInput;
    private Rigidbody Player;
    private int CurrentPointIndex = 0;

    private void Awake()
    {
        CustomInput = new PlayerInput();
        CustomInput.Enable();
    }

    private void Start()
    {
        Player = GetComponent<Rigidbody>();
        transform.position = CheckPoints[0].position;
        transform.forward = CheckPoints[1].position - CheckPoints[0].position;
    }

    private void Update()
    {
        if (CustomInput.Player.Accelerate.triggered)
        {
            Player.AddForce((CheckPoints[CurrentPointIndex + 1].position - transform.position).normalized * 5f, ForceMode.VelocityChange);
            transform.forward = Player.velocity;
        }

        Player.AddForce(-Player.velocity * 0.1f);
    }

    private void OnTriggerEnter(Collider other)
    {
        CurrentPointIndex++;
        if (CurrentPointIndex >= CheckPoints.Length - 1)
        {
            CurrentPointIndex = 0;
        }
        Player.velocity = (CheckPoints[CurrentPointIndex + 1].position -
                transform.position).normalized * Player.velocity.magnitude;
    }
}
