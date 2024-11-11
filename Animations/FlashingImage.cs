using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FlashingImage : MonoBehaviour
{
    public float flashSpeed = 1.0f;
    public bool startOnAwake;

    private Image _image;

    

    private void Awake()
    {
        // Get the SpriteRenderer component
        _image = GetComponent<Image>();
        if(startOnAwake )
            StartFlashing();
    }

    public void StartFlashing()
    {
        // Start the flashing coroutine
        StartCoroutine(FlashRoutine());
    }

    private IEnumerator FlashRoutine()
    {
        float timeElapsed = 0f;

        // Store the original color of the sprite
        Color originalColor = _image.color;

        // Set the initial alpha to 0 (fully transparent)
        Color newColor = originalColor;
        newColor.a = 0f;
        _image.color = newColor;

        while (timeElapsed < flashSpeed)
        {
            // Increment the elapsed time
            timeElapsed += Time.deltaTime;

            // Calculate the new alpha value
            float alpha = timeElapsed / flashSpeed; // This will go from 0 to 1 over time

            // Update the sprite's color with the new alpha
            newColor.a = alpha;
            _image.color = newColor;

            // Wait for the next frame
            yield return null;
        }

        // Ensure the alpha is set to 1 at the end
        newColor.a = 1f;
        _image.color = newColor;
    }

    private IEnumerator ContinuousFlash()
    {
        while (true)
        {
            float alpha = Mathf.PingPong(Time.time * (1 / flashSpeed), 1);
            Color newColor = _image.color;
            newColor.a = alpha;
            _image.color = newColor;
            yield return null;
        }
    }
}
