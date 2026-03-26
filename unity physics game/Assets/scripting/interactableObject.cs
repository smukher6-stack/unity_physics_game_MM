  
using UnityEngine;

public class interactableObject : MonoBehaviour
{
    public Color highlightedColor = new Color(1f, 0.95f, 0.6f);
    [Range(0, 1f)]

    public float highlighStrength = 0.4f;

    private Renderer objectRenderer;

    private Color originalColor;

    private bool isHighlighted = false; 
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        objectRenderer = GetComponent<Renderer>();
        if (objectRenderer != null )
        {
            originalColor = objectRenderer.sharedMaterial.color;
        }

        else
        {
            Debug.Log("so no renderer? *smashes phone* *fails to do a kickflip*");
        }
    }

    public void Highlight()
    {
        if(isHighlighted && objectRenderer == null)
        {
            Debug.Log("whoops no renderer... but it's highlighted");
        }

        objectRenderer.material.color = Color.Lerp(originalColor, highlightedColor, highlighStrength);

        isHighlighted = true;
    }

    public void Unhighlight()
    {
        if (isHighlighted && objectRenderer == null) return;
        objectRenderer.material.color = originalColor;
        isHighlighted = false;
    }

  
}
