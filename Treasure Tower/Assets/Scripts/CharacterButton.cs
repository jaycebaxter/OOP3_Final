using System;
using TMPro;
using Unity.VectorGraphics;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// When a character is clicked they will be taken to the SelectecdCharacter scene
// Script will be attached to each character
// Selected charcter is saved
public class CharacterButton : MonoBehaviour
{
    public GameObject characterData;

    [SerializeField]
    // Provide character index
    private int characterIndex;

    private int listCount = CharacterManager.instance.characterList.Length;

    private int newScene = 2;

    public void OnCharacterClick()
    {
        if (characterIndex < 0 || characterIndex > listCount)
        {
            Debug.LogError("Character Index cannot be less than 0 or greater than Character list");
        }
        else
        {
            // Store the characters info in selected Character (from character manager)
            CharacterManager.instance.selectedCharacter =
                CharacterManager.instance.characterList[characterIndex];

            SceneManager.LoadScene(newScene);
        }

    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
