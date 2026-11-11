using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine;

public class HideAndSeekManager : MonoBehaviour
{
    [SerializeField] private Seeker seeker;
    [SerializeField] private int hidersNeeded = 5;
    [SerializeField] private int totalLives = 5;
    [SerializeField] private string hubSceneName = "Hub";
    [SerializeField] private float returnDelay = 4f;

    private int reachedCount;
    private int livesLeft;
    private bool gameOver;

    private void OnEnable()
    {
        Hider.OnReachedGoal += HandleReachedGoal;
        Hider.OnCaught += HandleCaught;
    }

    private void OnDisable()
    {
        Hider.OnReachedGoal -= HandleReachedGoal;
        Hider.OnCaught -= HandleCaught;
    }

    private void Start()
    {
        livesLeft = totalLives;
        Debug.Log($"Lives: {livesLeft}");
    }

    private void HandleReachedGoal(Hider hider)
    {
        if (gameOver) return;

        reachedCount++;
        Debug.Log($"{hider.name} made it! {reachedCount}/{hidersNeeded}");

        if (reachedCount >= hidersNeeded) EndGame("Hiders win!");
    }

    private void HandleCaught(Hider hider)
    {
        if (gameOver) return;

        livesLeft--;
        Debug.Log($"{hider.name} was caught! Lives left: {livesLeft}");

        if (livesLeft <= 0) EndGame("Seeker wins!");
    }

    private void EndGame(string message)
    {
        gameOver = true;
        seeker.SetActive(false);

        foreach (Hider h in FindObjectsByType<Hider>(FindObjectsSortMode.None))
            h.Lock();

        Debug.Log(message);
        StartCoroutine(ReturnToHub());
    }

    private IEnumerator ReturnToHub()
    {
        yield return new WaitForSeconds(returnDelay);
        SceneManager.LoadScene(hubSceneName);
    }
}