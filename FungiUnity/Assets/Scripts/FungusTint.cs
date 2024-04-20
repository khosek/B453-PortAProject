using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FungusTint : MonoBehaviour // this is from the "Change Material Funged" section of the Base_BP in Unreal (changes the block to red tint when "funged")
{
    public Color fungedColor = Color.red; // The color to tint the material when funged
    public float tintIntensity = 0.5f; // How strong the tint should be

    private Material material;
    private Color originalColor;

    void Start()
    {
        // Assuming the object has a Renderer component with a material that supports color tinting
        material = GetComponent<Renderer>().material;
        originalColor = material.color; // Store the original color
    }

    void OnMouseDown() // This method is called when the user has pressed the mouse button while over the Collider
    {
        ToggleFungus();
    }

    void ToggleFungus()
    {
        if (material.color == originalColor)
        {
            // Apply the funged tint
            material.color = Color.Lerp(originalColor, fungedColor, tintIntensity);
        }
        else
        {
            // Revert to the original color
            material.color = originalColor;
        }
    }
}
