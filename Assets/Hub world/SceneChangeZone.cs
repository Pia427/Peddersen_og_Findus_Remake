using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Collider2D))]
public class SceneChangeZone : MonoBehaviour
{
    [SerializeField] private string sceneName;
    [SerializeField] private float delay = 0f;
    [SerializeField] private bool requireAllPlayers = true;

     private readonly HashSet<PlayerMovement> playersInside = new HashSet<PlayerMovement>();
    private bool triggered;

    private void Reset()
    {
        GetComponent<Collider2D>().isTrigger = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        PlayerMovement player = other.GetComponentInParent<PlayerMovement>();
        if (player == null) return;

        playersInside.Add(player);
        TryStart();
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        PlayerMovement player = other.GetComponentInParent<PlayerMovement>();
        if (player != null) playersInside.Remove(player);
    }

    private void TryStart()
    {
        if (triggered) return;

        int needed = requireAllPlayers
            ? FindObjectsByType<PlayerMovement>(FindObjectsSortMode.None).Length
            : 1;

        if (playersInside.Count < needed) return;

        triggered = true;
        Invoke(nameof(LoadScene), delay);
    }

    private void LoadScene()
    {
        SceneManager.LoadScene(sceneName);
    }
}
