using UnityEngine;
using UnityEngine.SceneManagement;

public class RetryButton : MonoBehaviour
{
    public void Retry()
    {
        // ”O‚Ì‚½‚ßŠÔ‚ğ–ß‚·
        Time.timeScale = 1f;

        // ¡‚ÌƒV[ƒ“‚ğÄ“Ç‚İ‚İ
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}