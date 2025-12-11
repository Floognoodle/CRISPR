using UnityEngine;

public class Projectile : MonoBehaviour
{
    public int size = 1;

    // How long the bullet lasts
    public float lifeTime = 4f;

    public float sizeScaleStep = 0.2f;

    Vector3 baseScale;

    void Awake()
    {
        baseScale = transform.localScale;
    }

    public void Initialize(int newSize, float newSizeScaleStep)
    {
        size = Mathf.Max(1, newSize);
        sizeScaleStep = newSizeScaleStep;

        float scaleMultiplier = 1f + sizeScaleStep * (size - 1);
        transform.localScale = baseScale * scaleMultiplier;

        // Destroy this bullet eventually
        Destroy(gameObject, lifeTime);
    }
}