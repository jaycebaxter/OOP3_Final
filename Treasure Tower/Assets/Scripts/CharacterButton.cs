using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterButton : MonoBehaviour
{ 
    [SerializeField]
    // Provide character index
    private int characterIndex;
    public int newScene;
    

    public void OnCharacterClick()
    {
        Debug.Log("Manager BEFORE load: " + CharacterManager.Instance.GetEntityId());
        var manager = CharacterManager.Instance;

        if (characterIndex < 0 || characterIndex >= manager.characterList.Length)
        {
            Debug.LogError("Character Index invalid");
            return;
        }



        Debug.Log("BUTTON CLICKED");

        // Store the characters info in selected Character (from character manager)
        manager.SetCharacter(characterIndex);

        //Debug.Log("CharacterButton Instance ID: " + manager.GetEntityId());
        //Debug.Log("Character Name: " + manager.selectedCharacter.GetName());
        //Debug.Log("Clicked index: " + characterIndex);

        //Debug.Log("Character Name CHARACTER BUTTON: " + manager.selectedCharacter.GetName());
        SceneManager.LoadScene(newScene);

    }
}