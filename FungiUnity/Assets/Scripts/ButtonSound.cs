using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonSound : MonoBehaviour
{
    private AudioSource audioSource;
    public AudioClip[] buttonSounds; // Assign this array in the Inspector with your 26 sounds

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            // Add an AudioSource component if the button doesn't already have one
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    // Method to be called on button click
    public void PlayRandomButtonSound()
    {
        if (buttonSounds.Length > 0)
        {
            int randomIndex = Random.Range(0, buttonSounds.Length); // This is correct and will include the last element
            audioSource.clip = buttonSounds[randomIndex];
            audioSource.Play();
        }
        else
        {
            Debug.LogWarning("Button sounds array is empty!");
        }
    }
}
