using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterButton : MonoBehaviour
{ 
    [SerializeField]
    // Provide character index
    private int characterIndex;
    private int newScene = 1;
    

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
            Debug.Log("Button Clicked");
            Debug.Log("Character Index: " + characterIndex);

            // Store the characters info in selected Character (from character manager)
            manager.selectedCharacter = manager.characterList[characterIndex];

            if (manager.selectedCharacter == null)
            {
                Debug.Log("Chracter is null");
                return;
            }

            Debug.Log("Character Name: " + manager.selectedCharacter.GetName());

            SceneManager.LoadScene(newScene);
        }
    }
}