using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ButtonManager : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
     public Color hoverColor = Color.yellow;
     public Color clickColor = Color.grey;

     [SerializeField] GameObject button;
     [SerializeField] Image ButtonBackground;

     public void OnPointerEnter(PointerEventData eventData)
     {
        ButtonBackground.color = hoverColor;
     }

     public void OnPointerExit(PointerEventData eventData)
     {
        ButtonBackground.color = Color.white;
     }

     public void OnPointerClick()
     {
        ButtonBackground.color = clickColor;
        // Load your specific scene here
        SceneManager.LoadScene("YourSceneName");
     }
}
