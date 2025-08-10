using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartButton : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [Header("Effects")]
    [SerializeField] private AudioClip soundEffect;

    [Header("Settings")]
    [SerializeField] private float maxGazeDetectionTime = 2f;
    private float elapsedGazeDetectionTime = 0f;

    [SerializeField] private PlayerManager player;

    private bool isGazing = false;


    private void Start()
    {
        
    }


    private void Update()
    {
        if (isGazing)
        {
            elapsedGazeDetectionTime += Time.deltaTime;

            if (elapsedGazeDetectionTime >= maxGazeDetectionTime)
            {
                AudioManager.Instance.PlaySound(soundEffect);
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
        }
    }


    public void OnPointerEnter()
    {
        isGazing = true;
    }


    public void OnPointerExit()
    {
        isGazing = false;
        elapsedGazeDetectionTime = 0f;
    }
}
