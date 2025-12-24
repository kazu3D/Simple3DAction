using UnityEngine;

public class GameOverUIController : MonoBehaviour
{
    [SerializeField] AudioSource gameOverSE;
    public static GameOverUIController instance;

    public GameObject gameOverPanel;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void ShowGameOver()
    {
        gameOverPanel.SetActive(true);
        Time.timeScale = 0f; // ÉQÅ[ÉÄí‚é~
        gameOverSE.Play();
    }
}