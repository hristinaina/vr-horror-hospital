using TMPro;
using UnityEngine;

public class CanvasHandler : MonoBehaviour
{
    [SerializeField] private PlayerManager player;
    [SerializeField] private TextMeshProUGUI result;


    void Start()
    {
        // Subscribe to the OnGameStateChanged event
        player.OnGameStateChanged += HandleGameStateChanged;

        gameObject.SetActive(false);
    }

    private void HandleGameStateChanged(PlayerManager.GameState newGameState)
    {
        Debug.Log("Game State changed");
        Debug.Log(newGameState);

        if (newGameState.Equals(PlayerManager.GameState.Success))
        {
            gameObject.SetActive(true);
            result.text = "SUCCESS";
            result.color = new Color(0.5f, 0.2f, 0.8f);
        }
        else if (newGameState.Equals(PlayerManager.GameState.Failed))
        {
            gameObject.SetActive(true);
            result.text = "YOU FAILED";
            result.color = new Color(0.5f, 0.2f, 0.8f);
        }
    }


    private void OnDestroy()
    {
        player.OnGameStateChanged -= HandleGameStateChanged;

    }
}
