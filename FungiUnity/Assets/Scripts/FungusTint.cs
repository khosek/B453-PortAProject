using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FungusTint : MonoBehaviour // this is from the "Change Material Funged" section of the Base_BP in Unreal (changes the block to red tint when "funged")
{
    public Color fungedColor = Color.red; // The color to tint the material when funged
    public float tintIntensity = 0.5f; // How strong the tint should be (0 = no change, 1 = full tint color)

    private Renderer[] renderers;
    private Material[] originalMaterials;

    void Start()
    {
        // Find all Renderer components in this object and its children
        renderers = GetComponentsInChildren<Renderer>();
        originalMaterials = new Material[renderers.Length];

        // Store the original materials of all renderers
        for (int i = 0; i < renderers.Length; i++)
        {
            // Instantiate a new material based on the original material
            originalMaterials[i] = renderers[i].material;
        }
    }

    void OnMouseDown() // This method is called when the user has pressed the mouse button while over the Collider
    {
        ToggleFungus();
    }

    void ToggleFungus()
    {
        // Apply the funged tint to all parts of the dirt block
        for (int i = 0; i < renderers.Length; i++)
        {
            Renderer rend = renderers[i];
            // Create a new material instance if not already created
            if (rend.material == originalMaterials[i])
            {
                rend.material = new Material(originalMaterials[i]);
            }
            // Lerp the color towards the fungedColor based on tintIntensity
            rend.material.color = Color.Lerp(originalMaterials[i].color, fungedColor, tintIntensity);
        }
    }
}
