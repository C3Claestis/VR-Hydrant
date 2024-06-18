using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Microsoft.MixedReality.Toolkit.Experimental.UI;

public class PlayerNameManager : MonoBehaviour
{
    #region Private Variables

    private TMP_InputField inputField; // Reference to the TextMeshPro InputField

    #endregion

    #region Unity Methods

    // Start is called before the first frame update
    void Start()
    {
        // Get the TMP_InputField component
        inputField = GetComponent<TMP_InputField>();

        // Add listener to open keyboard when the input field is selected
        inputField.onSelect.AddListener(x => OpenKeyboard());

        // Set the input field text to the saved player name
        inputField.text = PlayerPrefs.GetString("NamePlayer");

        // Add listener to update player name when the input field value changes
        inputField.onValueChanged.AddListener(UpdatePlayerName);
    }

    #endregion

    #region Custom Methods

    /// <summary>
    /// Updates the player name in PlayerPrefs when the input field value changes.
    /// </summary>
    /// <param name="newName">The new player name.</param>
    void UpdatePlayerName(string newName)
    {
        PlayerPrefs.SetString("NamePlayer", newName);
        PlayerPrefs.Save();
    }

    /// <summary>
    /// Opens the non-native keyboard with the current input field text.
    /// </summary>
    void OpenKeyboard()
    {
        NonNativeKeyboard.Instance.InputField = inputField;
        NonNativeKeyboard.Instance.PresentKeyboard(inputField.text);
    }

    #endregion

    #region TODO

    // TODO: Add error handling for keyboard and input field interactions.
    // TODO: Implement functionality to close the keyboard when input is complete or canceled.
    // TODO: Add visual feedback or notifications when the player name is updated.

    #endregion
}
