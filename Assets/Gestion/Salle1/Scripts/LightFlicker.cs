using UnityEngine;

public class LightFlicker : MonoBehaviour
{
    public Light spotLight;

    [Header("Flicker Settings")]
    public float minIntensity = 0.5f;
    public float maxIntensity = 1.5f;
    public float flickerSpeed = 0.1f;

    private float timer;

    void Start()
    {
        if (spotLight == null)
            spotLight = GetComponent<Light>();
    }

    void Update()
    {
        timer -= Time.deltaTime;

        if (timer <= 0f)
        {
            // Change intensity de façon aléatoire pour un effet “endommagé”
            spotLight.intensity = Random.Range(minIntensity, maxIntensity);

            // Temps entre deux “pannes”
            timer = flickerSpeed;
        }
    }
}
