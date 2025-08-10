using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MedicalKit : MonoBehaviour
{
    [Header("Effects")]
    [SerializeField] private AudioClip pickUpSoundEffect;

    [Header("Settings")]
    [SerializeField] private float maxGazeDetectionTime = 1f;
    private float elapsedGazeDetectionTime = 0f;

    [SerializeField] private PlayerManager player;
    private bool isGazing = false;


    // Update is called once per frame
    private void Update()
    {
        if (isGazing)
        {
            elapsedGazeDetectionTime += Time.deltaTime;

            if (elapsedGazeDetectionTime >= maxGazeDetectionTime)
            {
                AudioManager.Instance.PlaySound(pickUpSoundEffect);
                PickUp();
            }
        }
    }


    private void PickUp()
    {
        Debug.Log("Medical kit picked up!");

        player.PickUpMedicalAid();

        OnPointerExit();
        gameObject.SetActive(false);
    }

    // This method is called by the Main Camera when it starts gazing at this GameObject.
    public void OnPointerEnter()
    {
        isGazing = true;
    }

    // This method is called by the Main Camera when it stops gazing at this GameObject.
    public void OnPointerExit()
    {
       isGazing= false;
       elapsedGazeDetectionTime = 0f;
    }

}
