using UnityEngine;

public class HideAndSeekManager : MonoBehaviour
{
    [SerializeField] private Seeker seeker;
    [SerializeField] private int hidersNeeded = 5;

    private int reachedCount;
    private bool gameOver;

    private void OnEnable() => Hider.OnReachedGoal += HandleReachedGoal;
    private void OnDisable() => Hider.OnReachedGoal -= HandleReachedGoal;

    private void HandleReachedGoal(Hider hider)
    {
        if (gameOver) return;

        reachedCount++;
        Debug.Log($"{hider.name} made it! {reachedCount}/{hidersNeeded}");

        if (reachedCount >= hidersNeeded) EndGame();
    }

    private void EndGame()
    {
        gameOver = true;
        seeker.SetActive(false);

        foreach (Hider h in FindObjectsByType<Hider>(FindObjectsSortMode.None))
            h.Freeze();

        Debug.Log("Hiders win!");
    }
}