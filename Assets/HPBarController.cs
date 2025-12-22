using UnityEngine;
using UnityEngine.UI;

public class HPBarController : MonoBehaviour
{
    public Image hpFill;

    private float currentFillAmount;
    private float targetFillAmount;
    public float smoothSpeed = 5f;

    public Color fullHPColor = Color.green;
    public Color lowHPColor = Color.red;

    public static HPBarController instance;

    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        currentFillAmount = 1f;
        targetFillAmount = 1f;
    }

    void Update()
    {
        currentFillAmount = Mathf.Lerp(
            currentFillAmount,
            targetFillAmount,
            smoothSpeed * Time.deltaTime
        );

        hpFill.fillAmount = currentFillAmount;

        UpdateHPColor();
    }

    public void UpdateHP(float currentHP, float maxHP)
    {
        targetFillAmount = currentHP / maxHP;
    }

    void UpdateHPColor()
    {
        hpFill.color = Color.Lerp(
            lowHPColor,
            fullHPColor,
            currentFillAmount
        );
    }
}