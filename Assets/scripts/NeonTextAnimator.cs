using UnityEngine;
using TMPro;
using System.Collections;

public class NeonTextAnimator : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textMesh;
    [SerializeField] private float typewriterDelay = 0.05f;
    [SerializeField] private float flashDuration = 0.1f;
    [SerializeField] private int flashCount = 3;
    [SerializeField] private float colorCycleDuration = 0.5f;
    
    // Neon color palette (80s synthwave vibes)
    private Color[] neonColors = new Color[]
    {
        new Color(0.1f, 1f, 1f, 1f),      // Cyan
        new Color(1f, 0.2f, 1f, 1f),      // Magenta
        new Color(1f, 0.5f, 0f, 1f),      // Orange
        new Color(0f, 1f, 0.5f, 1f),      // Green
        new Color(1f, 0f, 0.5f, 1f),      // Hot Pink
    };

    private void Start()
    {
        // Make sure text is initially invisible for typewriter effect
        if (textMesh != null)
        {
            textMesh.text = "PUCK PROTOCOL";
            textMesh.maxVisibleCharacters = 0;
        }
        SetTextAndAnimate(textMesh.text);
    }

    /// <summary>
    /// Plays the complete animation sequence: typewriter -> flash -> color cycle
    /// </summary>
    public void PlayNeonAnimation()
    {
        StartCoroutine(NeonAnimationSequence());
    }

    private IEnumerator NeonAnimationSequence()
    {
        // Step 1: Typewriter effect with color cycling
        yield return StartCoroutine(TypewriterEffectWithColorCycle());
        
        // Step 2: Flash 3 times
        yield return StartCoroutine(FlashText());
        
        // Step 3: Continuous color cycling (optional, runs indefinitely)
        StartCoroutine(ContinuousColorCycle());
    }

    /// <summary>
    /// Typewriter effect that reveals characters one by one
    /// Text color cycles through neon palette during reveal
    /// </summary>
    private IEnumerator TypewriterEffectWithColorCycle()
    {
        textMesh.maxVisibleCharacters = 0;
        int textLength = textMesh.text.Length;
        int colorIndex = 0;

        for (int i = 0; i < textLength; i++)
        {
            textMesh.maxVisibleCharacters = i + 1;
            textMesh.color = neonColors[colorIndex % neonColors.Length];
            colorIndex++;
            
            yield return new WaitForSeconds(typewriterDelay);
        }
    }

    /// <summary>
    /// Flashes the entire text by toggling visibility
    /// </summary>
    private IEnumerator FlashText()
    {
        for (int flash = 0; flash < flashCount; flash++)
        {
            // Turn off
            textMesh.alpha = 0f;
            yield return new WaitForSeconds(flashDuration);
            
            // Turn on
            textMesh.alpha = 1f;
            yield return new WaitForSeconds(flashDuration);
        }
    }

    /// <summary>
    /// Continuously cycles through neon colors like a sign board
    /// Call this after initial animation or loop it indefinitely
    /// </summary>
    private IEnumerator ContinuousColorCycle()
    {
        int colorIndex = 0;

        while (true)
        {
            textMesh.color = neonColors[colorIndex % neonColors.Length];
            colorIndex++;
            
            yield return new WaitForSeconds(colorCycleDuration);
        }
    }

    /// <summary>
    /// Change the text and restart animation
    /// </summary>
    public void SetTextAndAnimate(string newText)
    {
        StopAllCoroutines();
        textMesh.text = newText;
        PlayNeonAnimation();
    }

    /// <summary>
    /// Customize neon colors (call before animation)
    /// </summary>
    public void SetNeonColors(Color[] colors)
    {
        neonColors = colors;
    }

    /// <summary>
    /// Stop all animations
    /// </summary>
    public void StopAnimation()
    {
        StopAllCoroutines();
    }
}
