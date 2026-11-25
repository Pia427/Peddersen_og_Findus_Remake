using TMPro;
using UnityEngine;

public class HideAndSeekUI : MonoBehaviour
{
    [SerializeField] private TMP_Text livesText;
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private TMP_Text resultText;

    private void Awake()
    {
        if (resultText != null) resultText.gameObject.SetActive(false);
    }

    public void SetLives(int left, int total)
    {
        if (livesText != null) livesText.text = $"Lives: {left}/{total}";
    }

    public void SetScore(int reached, int needed)
    {
        if (scoreText != null) scoreText.text = $"Escaped: {reached}/{needed}";
    }

    public void ShowResult(string message)
    {
        if (resultText == null) return;
        resultText.text = message;
        resultText.gameObject.SetActive(true);
    }
}