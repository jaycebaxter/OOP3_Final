using System;
using TMPro;
using Unity.VectorGraphics;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// When a character is clicked they will be taken to the SelectecdCharacter scene
// Selected charcter is saved
public class CharacterButton : MonoBehaviour
{
    public Character characterData;
    public CharacterDatabase characterDB;
    private int newScene = 2;

    public void onCharacterClick()
    {
        
        if (characterData != null) 
        { 
            CharacterManager.instance.selectedCharacter = characterData;
            // Loads CharacterSelected scene
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
