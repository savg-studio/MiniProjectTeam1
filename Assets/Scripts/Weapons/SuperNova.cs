using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuperNova : Explosion
{
    // Public params
    public float explosionDelay;
    private float explosionTimeLeft;
    public Color targetColor;

    // Cache
    private GameObject circleSprite;
    private SpriteRenderer outlineSpriteRenderer;
    private SpriteRenderer circleSpriteRenderer;
    private Vector3 baseScale;
    private Color baseColor;

    private void Start()
    {
        base.Start();

        // Time
        explosionTimeLeft = explosionDelay;

        // Cache
        circleSprite = transform.Find("Sprite").gameObject;
        baseScale = circleSprite.transform.localScale;
        circleSpriteRenderer = circleSprite.GetComponent<SpriteRenderer>();
        baseColor = circleSpriteRenderer.color;

        GameObject outline = transform.Find("Outline").gameObject;
        outlineSpriteRenderer = outline.GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        DecreaseTime();

        UpdateSprite();

        if (IsTimeOver())
        {
            Explode();
            OnExplode();
        }
    }

    private void OnExplode()
    {
        GameObject.Destroy(this.gameObject);
    }

    private void DecreaseTime()
    {
        explosionTimeLeft -= Time.deltaTime;
    }

    private bool IsTimeOver()
    {
        return explosionTimeLeft <= 0;
    }

    private void UpdateSprite()
    {
        // Update circle scale
        var percent = 1 - (explosionTimeLeft / explosionDelay);
        Vector3 scale = baseScale * percent;
        circleSprite.transform.localScale = scale;

        // Update color
        var diffColor = targetColor - baseColor;
        var color = baseColor + diffColor * percent;
        color.a = 1;
        circleSpriteRenderer.color = color;
        outlineSpriteRenderer.color = color;
    }

    protected override void BeforeImpactObject(GameObject go)
    {
        var spaceship = go.GetComponent<Spaceship>();
        if (spaceship)
            spaceship.Stun();
    }
}
