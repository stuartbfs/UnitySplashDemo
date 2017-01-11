using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

using System.Collections;

public class LogoController : MonoBehaviour 
{
    public float fadeDuration = 5.0f;   // Length of time (in seconds) to fade in the logo

    public float pauseDuration = 1.0f;  // Length of time (in seconds) to transition to the next scene

    public string nextLevel;            // Name of the scene to transition to after fading in the logo 
                                        // (make sure the scene has been added in Build Settings!)

    public int startAlphaRange = 0;     // Inital Alpha value
    public int endAlphaRange = 255;     // Final Alpha value

    private float currentTime;          // Keeps track of how much time has accumulated

    private Image image;                // The image we are manipulating

    private readonly AnimationCurve smoothCurve = new AnimationCurve 
        (
            new Keyframe [] { new Keyframe (0, 0), new Keyframe (1, 1) }
        );

    // Use this for initialization
    void Start () 
    {
        image = this.GetComponent<Image> ();
        if (image == null) 
        {
            Debug.LogError ("Image component not found");
        } 
        else 
        {
            UpdateAlpha (startAlphaRange);  // Set the alpha range to the initial value
            StartCoroutine (FadeIn ());     // Kick off the thread that does the fade out
        }
    }

    // Loop for each 100 ms and fade in the 
    private IEnumerator FadeIn ()
    {
        currentTime = 0.0f;
        while (currentTime <= fadeDuration) 
        {
            // Keep track of how long has passed
            currentTime += Time.deltaTime;

            // Update the alpha value based on how much time has expired from 0s -> fadeDuration
            UpdateAlpha (Mathf.Lerp (startAlphaRange, (endAlphaRange / 255.0f), smoothCurve.Evaluate (currentTime / fadeDuration)));

            // Sleep for 100ms
            yield return new WaitForSeconds (0.01f);
        }

        // Wait for a period of time with the final alpha value
        yield return new WaitForSeconds (pauseDuration);

        // Load the next scene (obvs.)
        SceneManager.LoadScene (nextLevel, LoadSceneMode.Single);
    }

    // Updates our Alpha value
    // Note: color cannot be updated so we need to copy the current color values along
    // with the updated alpha value
    private void UpdateAlpha (float alpha)
    {
        image.color = new Color (image.color.r, image.color.g, image.color.b, alpha);
    }
}