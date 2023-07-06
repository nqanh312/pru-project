using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class is responsible for background parallax effect
/// </summary>
public class Background : MonoBehaviour
{
    private float startPosition;
    private float backgroundLength;

    public GameObject cam;
    public float parallaxEffect;

    void Start()
    {
        startPosition = transform.position.x;
        backgroundLength = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    /// <summary>
    /// Parallax effect logic
    /// </summary>
    void Update()
    {
        float temp = (cam.transform.position.x * (1 - parallaxEffect));
        float distance = (cam.transform.position.x * parallaxEffect);

        transform.position = new Vector3(startPosition + distance, transform.position.y, transform.position.z);

        if (temp > startPosition + backgroundLength)
            startPosition += backgroundLength;
        else if (temp < startPosition - backgroundLength)
            startPosition -= backgroundLength;
    }
}
