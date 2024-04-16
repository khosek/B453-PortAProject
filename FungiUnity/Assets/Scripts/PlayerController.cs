using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FungiCharacter : MonoBehaviour
{
    // Assuming you have a CameraController script to handle camera dragging
    [SerializeField] private CameraController cameraController;

    private GameObject lastHover;
    private bool lastCorrect;
    private int lastRange;

    // Assuming LevelManager is a singleton for ease of access
    private LevelManager levelManager;

    private void Start()
    {
        // Unity uses Start for initialization
        Time.timeScale = 1; // Unpausing game if paused
    }

    private void Update()
    {
        HandleInteraction();
    }

    public void SetCameraDragging(bool dragging)
    {
        // Assuming cameraController exists and manages camera dragging
        cameraController.IsDragging = dragging;
    }

    private void HandleInteraction()
    {
        if (Input.GetButtonDown("Fire1")) // Assuming Fire1 is set up in Input Manager for interaction
        {
            RaycastHit hit;
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit))
            {
                Base hitBaseBox = hit.collider.GetComponent<Base>();
                if (hitBaseBox != null && levelManager != null)
                {
                    if (hitBaseBox != lastHover)
                    {
                        if (lastHover != null)
                        {
                            levelManager.UpdateHighlights(lastHover, lastRange, false, lastCorrect);
                        }

                        lastCorrect = hitBaseBox.IsFunged && levelManager.WouldFungeAny(hitBaseBox.GridX, hitBaseBox.GridY);
                        lastHover = hitBaseBox.gameObject;
                        lastRange = levelManager.currentRange;
                        levelManager.UpdateHighlights(hitBaseBox.gameObject, hitBaseBox.IsFunged ? levelManager.currentRange : 0, true, lastCorrect);
                    }
                }
                else
                {
                    lastHover = null;
                }
            }
        }
    }

    public void Interact()
    {
        // Simplified interaction logic based on Unreal's Interact method
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit))
        {
            Base hitBaseBox = hit.collider.GetComponent<Base>();
            if (hitBaseBox != null && levelManager != null)
            {
                if (levelManager.ExpandFunge(hitBaseBox.GridX, hitBaseBox.GridY))
                {
                    levelManager.UpdateHighlights(hitBaseBox.gameObject, levelManager.currentRange, true, false);
                }
            }
        }
    }

    // Additional methods like Pause and ContinueGame can be implemented as needed
}
