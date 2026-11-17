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

    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip gameOverClip;   // seeker wins (lives ran out)
    [SerializeField] private AudioClip winClip;        // hiders win (optional)
    [Range(0f, 1f)]
    [SerializeField] private float endVolume = 0.9f;
    [SerializeField] private float endSoundDelay = 0.4f; // lets the "spotted" sound be heard first

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

        if (reachedCount >= hidersNeeded) EndGame("Hiders win!", true);
    }

    private void HandleCaught(Hider hider)
    {
        if (gameOver) return;

        if (reachedCount >= hidersNeeded) EndGame("Hiders win!", true);

        livesLeft--;
        Debug.Log($"{hider.name} was caught! Lives left: {livesLeft}");

       if (livesLeft <= 0) EndGame("Seeker wins!", false); 
    }

    private void EndGame(string message, bool hidersWon)
    {
        gameOver = true;
        seeker.SetActive(false);

        foreach (Hider h in FindObjectsByType<Hider>(FindObjectsSortMode.None))
            h.Lock();

        Debug.Log(message);
        StartCoroutine(PlayEndSound(hidersWon ? winClip : gameOverClip));
        StartCoroutine(ReturnToHub());
    }

    private IEnumerator ReturnToHub()
    {
        yield return new WaitForSeconds(returnDelay);
        SceneManager.LoadScene(hubSceneName);
    }

    private IEnumerator PlayEndSound(AudioClip clip)
{
    yield return new WaitForSeconds(endSoundDelay);

    if (audioSource != null && clip != null)
        audioSource.PlayOneShot(clip, endVolume);
}
}