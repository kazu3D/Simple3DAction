using UnityEngine;

public class CameraSwing : MonoBehaviour
{
    [SerializeField] float swingAngle = 15f;   // Å‘å‰ñ“]Šp
    [SerializeField] float swingSpeed = 0.2f;   // ñU‚è‚Ì‘¬‚³

    Quaternion startRotation;

    void Start()
    {
        startRotation = transform.rotation;
    }

    void Update()
    {
        float angle = Mathf.Sin(Time.time * swingSpeed) * swingAngle;
        transform.rotation = startRotation * Quaternion.Euler(0f, angle, 0f);
    }
}