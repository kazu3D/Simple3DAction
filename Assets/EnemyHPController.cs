using UnityEngine;
using UnityEngine.UI;

public class EnemyHPController : MonoBehaviour
{
    public int maxHP = 3;
    private int currentHP;

    public Image hpFill;

    void Start()
    {
        currentHP = maxHP;
        UpdateHPBar();
    }

    public void TakeDamage(int damage)
    {
        currentHP -= damage;
        UpdateHPBar();

        if (currentHP <= 0)
        {
            Die();
        }
    }

    void UpdateHPBar()
    {
        float fillAmount = (float)currentHP / maxHP;
        hpFill.fillAmount = fillAmount;

        // F•Ï‰»i—Î ¨ ‰© ¨ Ôj
        hpFill.color = Color.Lerp(Color.red, Color.green, fillAmount);
    }

    void Die()
    {
        Destroy(gameObject);
    }
}