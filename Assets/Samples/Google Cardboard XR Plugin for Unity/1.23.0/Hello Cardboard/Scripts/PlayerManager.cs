using System.Collections;
using TMPro;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class PlayerManager : MonoBehaviour
{
    public enum GameState
    {
        GameStart,
        Failed,
        Success
    }

    [SerializeField] private AudioClip distortionClip;
    [SerializeField] private int totalMedicalAids = 1;
    [SerializeField] private int collectedMedialAids = 0;
    private GameState gamestate;
    [SerializeField] private Canvas screenCanvas;

    [SerializeField] private Canvas firstAidCanvas;
    private TextMeshProUGUI firstAidText;

    [SerializeField] private Transform targetGameStat; // The object to look at
    private Transform childCamera;

    // Declare the event
    public delegate void GameStateChangedHandler(GameState newGameState);
    public event GameStateChangedHandler OnGameStateChanged;

    void Start()
    {
        gamestate = GameState.GameStart;
        screenCanvas.gameObject.SetActive(false);
        firstAidCanvas.gameObject.SetActive(false);
        Transform textTransform = firstAidCanvas.transform.Find("AidsNum");
        firstAidText = textTransform.GetComponent<TextMeshProUGUI>();
        childCamera = transform.Find("Main Camera");
    }

    public void SetGameFailed(bool isCatchedByZombie = false)
    {
        if (isCatchedByZombie)
        {
            screenCanvas.gameObject.SetActive(true);
            AudioManager.Instance.PlaySound(distortionClip);
            StartCoroutine(WaitAndExecute(3f, () =>
            {
                screenCanvas.gameObject.SetActive(false);
                ChangeGameState(GameState.Failed);
                RotateCameraToTarget();
            }));
        }
        else
        {
            ChangeGameState(GameState.Failed);
            RotateCameraToTarget();
        }
    }

    public void SetGameSucceded()
    {
        ChangeGameState(GameState.Success);
        RotateCameraToTarget() ;
    }

    private void RotateCameraToTarget()
    {
        Vector3 teleportPosition = new Vector3(-33.6f, 2.2f, 8.2f);
        transform.position = teleportPosition;

        childCamera.LookAt(targetGameStat);
    }

    public GameState GetGameState()
    {
        return gamestate;
    }

    public void PickUpMedicalAid()
    {
        collectedMedialAids++;
        firstAidCanvas.gameObject.SetActive(true);
        firstAidText.text = collectedMedialAids.ToString() + "/" + totalMedicalAids;
        Debug.Log("Number of collected medical aids: " + collectedMedialAids);
        StartCoroutine(WaitAndExecute(0.42f, () =>
        {
            firstAidCanvas.gameObject.SetActive(false);
        }));
    }

    public bool IsGameEndSuccess()
    {
        bool isSuccess = collectedMedialAids >= totalMedicalAids;
        Debug.Log(isSuccess);
        Debug.Log(collectedMedialAids);
        Debug.Log(totalMedicalAids);
        return isSuccess;
    }

    private void ChangeGameState(GameState newState)
    {
        if (gamestate != newState)
        {
            gamestate = newState;

            // Trigger the event to notify listeners
            OnGameStateChanged?.Invoke(newState);
        }
    }

    private IEnumerator WaitAndExecute(float waitTime, System.Action action)
    {
        yield return new WaitForSeconds(waitTime);
        action?.Invoke();
    }
}
