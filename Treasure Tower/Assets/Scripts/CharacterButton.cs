using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterButton : MonoBehaviour
{ 
    [SerializeField]
    // Provide character index
    private int characterIndex;
    private int newScene = 2;
    

    public void OnCharacterClick()
    {
        var manager = CharacterManager.Instance;

        if (characterIndex < 0 || characterIndex > manager.characterList.Length)
        {
            Debug.LogError("Character Index invalid");
            return;
        }
        else
        {
            Debug.Log("BUTTON CLICKED");



            // Store the characters info in selected Character (from character manager)
            manager.selectedCharacter = manager.characterList[characterIndex];

            //if (manager.selectedCharacter == null)
            //{
            //    Debug.Log("Chracter is null");
            //    return;
            //}

            Debug.Log("Character Name CHARACTER BUTTON: " + manager.selectedCharacter.GetName());
            //Debug.Log("Instance ID - Button Clicked: " + manager.GetEntityId());

            SceneManager.LoadScene(newScene);
        }
    }
}