using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class LookToWalk : MonoBehaviour
{
    private bool isWalking = false;
    private Camera mainCamera;
    [SerializeField] private float walkingSpeed = 3.0f;
    [SerializeField] private AudioClip walkingAudioEffect;

    [SerializeField] private float minimumAngleTreshold = 35.0f;
    [SerializeField] private float maximumAngleTreshold = 90.0f;

    private AudioSource walkingAudioSource;
    private bool isTriggered = false;

    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;
        walkingAudioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (mainCamera.transform.eulerAngles.x >= minimumAngleTreshold 
            && mainCamera.transform.eulerAngles.x <= maximumAngleTreshold)
        {
            isWalking = true;
        }
        else
        {
            isWalking = false;
        }
    }

    private void FixedUpdate()
    {
        if (isWalking)
        {
            MovePlayer();
            if (!walkingAudioSource.isPlaying)
            {
                walkingAudioSource.Play();
            }
        }
        else
        {
            walkingAudioSource.Stop();
        }
    }

    private void MovePlayer()
    {
        Vector3 movementVector = new Vector3(mainCamera.transform.forward.x, mainCamera.transform.forward.y, mainCamera.transform.forward.z);

        //if there is no collision
        if (!isTriggered)
        {
            movementVector.y = 0;
            transform.Translate(Time.deltaTime * walkingSpeed * movementVector.normalized);
        }
        //if there is a collision
        else
        {
            // Capsule parameters
            float capsuleRadius = 0.2f;
            float capsuleHeight = 1.0f; // Total height of the capsule, including the rounded ends
            Vector3 capsuleStart = transform.position; // Bottom center of the capsule
            Vector3 capsuleEnd = capsuleStart + Vector3.up * capsuleHeight; // Top center of the capsule

            // Perform the CapsuleCast
            bool canMove = !Physics.CapsuleCast(
                capsuleStart,
                capsuleEnd,
                capsuleRadius,
                movementVector,
                capsuleRadius + 1.8f // Slightly ahead of the capsule's radius
            );

            /*if (!canMove)
            {
                //cant move, try only on x axis
                Vector3 movementVectorX = new Vector3(movementVector.x, 0, 0);
                canMove = !Physics.CapsuleCast(
                capsuleStart,
                capsuleEnd,
                capsuleRadius,
                movementVectorX,
                capsuleRadius + 1.8f); // Slightly ahead of the capsule's radius
                if (canMove)
                {
                    movementVector = movementVectorX;
                }
                else
                {
                    Vector3 movementVectorZ = new Vector3(0, 0, movementVector.z);
                    canMove = !Physics.CapsuleCast(
                    capsuleStart,
                    capsuleEnd,
                    capsuleRadius,
                    movementVectorZ,
                    capsuleRadius + 1.8f
                    );

                    if (canMove)
                    {
                        movementVector = movementVectorZ;
                    }
                }
            }*/

            if (canMove)
            {
                movementVector.y = 0;
                transform.Translate(Time.deltaTime * walkingSpeed * movementVector.normalized);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        isTriggered = true;
    }

    private void OnTriggerExit(Collider other)
    {
        isTriggered = false;
    }


}
