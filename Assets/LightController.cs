using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LightController : MonoBehaviour
{
    public MemoryGameController gameController;
    
    public Image image;
    public Sprite onSprite;
    public Sprite offSprite;
    public Sprite crackedSprite;

    private void Start()
    {
        image = GetComponent<Image>();
        ToggleLight(false);
    }

    
    public void OnClick()
    {
        if (gameController.CanInteract)
        {
        
            gameController.OnLightClicked(gameObject);
        }
    }

    public void ToggleLight(bool isOn)
    {
        if (image != null)
        {
            // Change the sprite based on the state
            image.sprite = isOn ? onSprite : offSprite; 
        }
    }

    internal void CrackLight()
    {
        if (image != null)
        {
            
            image.sprite = crackedSprite;
        }
    }
}