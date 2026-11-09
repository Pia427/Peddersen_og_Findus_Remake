using UnityEngine;
using UnityEngine.InputSystem;

public class LocalCoopSpawner : MonoBehaviour
{
    [SerializeField] GameObject playerPrefab;
    [SerializeField] Transform spawn1, spawn2;
    [SerializeField] Sprite sprite1, sprite2;

    void Start()
    {
        SpawnPlayer(0, "WASD", spawn1, sprite1);
        SpawnPlayer(1, "Arrows", spawn2, sprite2);
    }

    void SpawnPlayer(int index, string scheme, Transform spawn, Sprite sprite)
    {
        var player = PlayerInput.Instantiate(playerPrefab, playerIndex: index,
            controlScheme: scheme, pairWithDevice: Keyboard.current);

        player.transform.position = spawn.position;
        player.GetComponentInChildren<SpriteRenderer>().sprite = sprite;
    }
    
}