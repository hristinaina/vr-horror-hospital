using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro; // Required for TextMeshPro
using System;

public class ExitDoor : MonoBehaviour
{
    [Header("Effects")]
    [SerializeField] private AudioClip enterSound;
    [SerializeField] private AudioClip failSound;
    [SerializeField] private AudioClip successSound;

    [Header("Settings")]
    [SerializeField] private float maxGazeDetectionTime = 2f;
    private float elapsedGazeDetectionTime = 0f;

    [SerializeField] private PlayerManager player;

    [Header("UI Elements")]
    [SerializeField] private TextMeshProUGUI gazeText; // Reference to the TextMeshProUGUI

    private bool isGazing = false;


    private void Start()
    {
        // Ensure the text is hidden at the start
        if (gazeText != null)
        {
            gazeText.gameObject.SetActive(false);
        }
    }


    private void Update()
    {
        Debug.Log(isGazing);
        if (isGazing)
        {
            elapsedGazeDetectionTime += Time.deltaTime;

            if (elapsedGazeDetectionTime >= maxGazeDetectionTime)
            {
                if (gameObject.CompareTag("ExitDoor"))
                {
                    LeaveGame();
                }
                else if (gameObject.CompareTag("EnterDoor"))
                {
                    EnterGame();
                }
            }
        }
    }

    private void LeaveGame()
    {
        //todo da se igrac okrene ka zidu
        bool success = player.IsGameEndSuccess();
        if (success)
        {
            player.SetGameSucceded();
            AudioManager.Instance.PlaySound(successSound);
        }
        else
        {
            player.SetGameFailed();
            AudioManager.Instance.PlaySound(failSound);
        }
        
        OnPointerExit();
    }

    private void EnterGame()
    {
        Vector3 teleportPosition = new Vector3(-27.35f, 2.2f, -3.72f); 
        player.transform.position = teleportPosition;

        AudioManager.Instance.PlaySound(enterSound);
        OnPointerExit();
    }


    public void OnPointerEnter()
    {
        if (gameObject.CompareTag("EnterDoor") && !player.IsGameStarted()) return;

        Debug.Log(player.IsGameStarted());

        isGazing = true;

        if (gazeText != null)
        {
            gazeText.gameObject.SetActive(true);
        }
    }


    public void OnPointerExit()
    {
        isGazing = false;
        elapsedGazeDetectionTime = 0f;

        if (gazeText != null)
        {
            gazeText.gameObject.SetActive(false);
        }
    }
}
