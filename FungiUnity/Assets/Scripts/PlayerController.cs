using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FungiCharacter : MonoBehaviour
{
    // Assuming you have a CameraController script to handle camera dragging
    //[SerializeField] private CameraController cameraController;

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
                            UpdateHighlights(lastHover, lastRange, false, lastCorrect);
                        }

                        lastCorrect = hitBaseBox.IsFunged && levelManager.WouldFungeAny(hitBaseBox.GridX, hitBaseBox.GridY);
                        lastHover = hitBaseBox.gameObject;
                        lastRange = levelManager.currentRange;
                        UpdateHighlights(hitBaseBox.gameObject, hitBaseBox.IsFunged ? levelManager.currentRange : 0, true, lastCorrect);
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
                    UpdateHighlights(hitBaseBox.gameObject, levelManager.currentRange, true, false);
                }
            }
        }
    }

    public void UpdateHighlights(GameObject block, int range, bool Highlighted, bool Correct) 
    {
        if (block.GetComponent<Base>().AllowsFunging && block != null)
        {
            //block.DoHighlight(Highlighted, Correct)
        }
        bool[] stop = new bool[4];
        for (int i = 1; i <= range; ++i)
        {
            block = levelManager.GetBlockAt(block.GetComponent<Base>().GridX, block.GetComponent<Base>().GridY - i);
            if (!stop[0] && block.GetComponent<Base>().AllowsFunging)
            {
                //block.DoHighlight(Highlighted, Correct)
            }
            else
            {
                stop[0] = true;
            }
            block = levelManager.GetBlockAt(block.GetComponent<Base>().GridX + i, block.GetComponent<Base>().GridY);
            if (!stop[1] && block.GetComponent<Base>().AllowsFunging)
            {
                //block.DoHighlight(Highlighted, Correct)
            }
            else
            {
                stop[1] = true;
            }
            block = levelManager.GetBlockAt(block.GetComponent<Base>().GridX, block.GetComponent<Base>().GridY + i);
            if (!stop[2] && block.GetComponent<Base>().AllowsFunging)
            {
                //block.DoHighlight(Highlighted, Correct)
            }
            else
            {
                stop[2] = true;
            }
            block = levelManager.GetBlockAt(block.GetComponent<Base>().GridX - i, block.GetComponent<Base>().GridY);
            if (!stop[3] && block.GetComponent<Base>().AllowsFunging)
            {
                //block.DoHighlight(Highlighted, Correct)
            }
            else
            {
                stop[3] = true;
            }
        }
    }
}
