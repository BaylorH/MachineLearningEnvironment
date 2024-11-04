using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class MainMenuScript : MonoBehaviour
{
    private UIDocument _document;
    private Button _button;
    private VisualElement _backgroundImageContainer; // Reference to the container

    private void Awake()
    {
        _document = GetComponent<UIDocument>();

        // Get the Start button
        _button = _document.rootVisualElement.Q("StartButton") as Button;

        // Get the BackgroundImageContainer by name
        _backgroundImageContainer = _document.rootVisualElement.Q("BackgroundImageContainer");

        // Register the button click event
        _button.RegisterCallback<ClickEvent>(OnPlayClick);
    }

    private void OnDisable()
    {
        // Unregister the callback to avoid memory leaks
        _button.UnregisterCallback<ClickEvent>(OnPlayClick);
    }

    private void OnPlayClick(ClickEvent evt)
    {
        // Log to the console for debugging
        Debug.Log("Play button pressed");

        // Hide the BackgroundImageContainer by setting its display style to None
        _backgroundImageContainer.style.display = DisplayStyle.None;
    }
}
