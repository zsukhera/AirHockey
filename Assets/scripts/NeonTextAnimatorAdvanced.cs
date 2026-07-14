using UnityEngine;
using TMPro;
using System.Collections;

public class NeonTextAnimatorAdvanced : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textMesh;
    [SerializeField] private CanvasGroup canvasGroup;

    [Header("Typewriter Settings")]
    [SerializeField] private float typewriterDelay = 0.05f;
    [SerializeField] private bool randomColorPerChar = true;

    [Header("Flash Settings")]
    [SerializeField] private float flashDuration = 0.1f;
    [SerializeField] private int flashCount = 3;

    [Header("Color Cycle Settings")]
    [SerializeField] private float colorCycleDuration = 0.5f;
    [SerializeField] private bool useGlowPulse = true;
    [SerializeField] private float glowIntensity = 1.5f;

    [Header("Advanced Effects")]
    [SerializeField] private bool useCharacterScale = false;
    [SerializeField] private bool useCharacterBounce = false;
    [SerializeField] private bool useOutlineGlow = true;

    private Color[] neonColors = new Color[]
    {
        new Color(0.1f, 1f, 1f, 1f),
        new Color(1f, 0.2f, 1f, 1f),
        new Color(1f, 0.5f, 0f, 1f),
        new Color(0f, 1f, 0.5f, 1f),
        new Color(1f, 0f, 0.5f, 1f),
        new Color(1f, 1f, 0f, 1f),
    };

    private Color originalColor;
    private float originalOutlineWidth;
    private bool isAnimating = false;

    private void OnEnable()
    {
        if (textMesh != null)
        {
            originalColor = textMesh.color;
            if (textMesh.outlineWidth > 0)
            {
                originalOutlineWidth = textMesh.outlineWidth;
            }
        }

        if (canvasGroup == null)
        {
            canvasGroup = GetComponent<CanvasGroup>();
        }
    }

    public void PlayNeonAnimation()
    {
        if (isAnimating) return;
        StartCoroutine(NeonAnimationSequence());
    }

    private IEnumerator NeonAnimationSequence()
    {
        isAnimating = true;

        yield return StartCoroutine(TypewriterEffectAdvanced());
        yield return StartCoroutine(FlashTextAdvanced());
        StartCoroutine(ContinuousColorCycleAdvanced());

        isAnimating = false;
    }

    private IEnumerator TypewriterEffectAdvanced()
    {
        textMesh.maxVisibleCharacters = 0;
        int textLength = textMesh.text.Length;
        int colorIndex = 0;

        for (int i = 0; i < textLength; i++)
        {
            textMesh.maxVisibleCharacters = i + 1;

            if (randomColorPerChar)
            {
                textMesh.color = neonColors[colorIndex % neonColors.Length];
                colorIndex++;
            }

            if (useCharacterBounce)
            {
                yield return StartCoroutine(BounceCharacter(i));
            }

            yield return new WaitForSeconds(typewriterDelay);
        }
    }

    private IEnumerator BounceCharacter(int charIndex)
    {
        textMesh.ForceMeshUpdate();
        var textInfo = textMesh.textInfo;

        if (charIndex < textInfo.characterCount)
        {
            var charInfo = textInfo.characterInfo[charIndex];
            if (!charInfo.isVisible) yield break;

            int meshIndex = textInfo.characterInfo[charIndex].materialReferenceIndex;
            int vertexIndex = textInfo.characterInfo[charIndex].vertexIndex;

            var vertices = textInfo.meshInfo[meshIndex].vertices;

            Vector3 offset = Vector3.zero;
            for (float t = 0; t < 0.2f; t += Time.deltaTime)
            {
                offset.y = Mathf.Sin(t * Mathf.PI) * 10f;

                for (int j = 0; j < 4; j++)
                {
                    vertices[vertexIndex + j] += offset;
                }

                textMesh.UpdateVertexData(TMP_VertexDataUpdateFlags.Vertices);
                yield return null;
            }
        }
    }

    private IEnumerator FlashTextAdvanced()
    {
        for (int flash = 0; flash < flashCount; flash++)
        {
            if (canvasGroup != null)
            {
                canvasGroup.alpha = 0f;
            }
            else
            {
                textMesh.alpha = 0f;
            }
            yield return new WaitForSeconds(flashDuration);

            if (canvasGroup != null)
            {
                canvasGroup.alpha = 1f;
            }
            else
            {
                textMesh.alpha = 1f;
            }
            yield return new WaitForSeconds(flashDuration);
        }
    }

    private IEnumerator ContinuousColorCycleAdvanced()
    {
        int colorIndex = 0;

        while (gameObject.activeInHierarchy)
        {
            textMesh.color = neonColors[colorIndex % neonColors.Length];

            if (useOutlineGlow && textMesh.outlineWidth > 0)
            {
                yield return StartCoroutine(PulseOutlineGlow());
            }

            colorIndex++;
            yield return new WaitForSeconds(colorCycleDuration);
        }
    }

    private IEnumerator PulseOutlineGlow()
    {
        float pulseDuration = colorCycleDuration * 0.8f;
        float elapsed = 0f;

        while (elapsed < pulseDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / pulseDuration;
            float outlineWidth = originalOutlineWidth * (1f + (Mathf.Sin(t * Mathf.PI) * 0.5f));
            textMesh.outlineWidth = outlineWidth;
            yield return null;
        }

        textMesh.outlineWidth = originalOutlineWidth;
    }

    public void SetNeonColors(Color[] colors)
    {
        neonColors = colors;
    }

    public void AddNeonColor(Color color)
    {
        System.Array.Resize(ref neonColors, neonColors.Length + 1);
        neonColors[neonColors.Length - 1] = color;
    }

    public void SetTextAndAnimate(string newText)
    {
        StopAllCoroutines();
        textMesh.text = newText;
        textMesh.maxVisibleCharacters = 0;
        isAnimating = false;
        PlayNeonAnimation();
    }

    public Color[] GetNeonColors()
    {
        return neonColors;
    }

    public void StopAnimation()
    {
        StopAllCoroutines();
        isAnimating = false;
        textMesh.maxVisibleCharacters = textMesh.text.Length;
        textMesh.color = originalColor;
        if (canvasGroup != null)
            canvasGroup.alpha = 1f;
        else
            textMesh.alpha = 1f;
    }

    public void SetColorPaletteRetro()
    {
        neonColors = new Color[]
        {
            new Color(1f, 0f, 0.5f, 1f),
            new Color(1f, 1f, 0f, 1f),
            new Color(0f, 1f, 1f, 1f),
        };
    }

    public void SetColorPalettePurple()
    {
        neonColors = new Color[]
        {
            new Color(0.8f, 0f, 1f, 1f),
            new Color(1f, 0.2f, 1f, 1f),
            new Color(0.5f, 0f, 1f, 1f),
        };
    }

    public void SetColorPaletteMiamiVice()
    {
        neonColors = new Color[]
        {
            new Color(1f, 0.2f, 0.8f, 1f),
            new Color(0f, 1f, 1f, 1f),
            new Color(0f, 1f, 0.5f, 1f),
            new Color(1f, 0.5f, 0f, 1f),
        };
    }
}