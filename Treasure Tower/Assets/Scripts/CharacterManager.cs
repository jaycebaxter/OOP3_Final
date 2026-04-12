using TMPro;
using UnityEngine;

// Used for data persistence 
// 
public class CharacterManager : MonoBehaviour
{
    public Character[] characterList;
    public Character selectedCharacter;

    public static CharacterManager Instance { get; private set; }

    public void Awake()
    {
        // Creates instance if there isn't one
        if (Instance == null)
        {
            Instance = this;
            // Prevents gameObjects from being deleted when a new scene is loaded
            DontDestroyOnLoad(gameObject);

            Debug.Log("CHARACTER MANAGER");
            Debug.Log("Instance ID - Character Manager: " + Instance.GetEntityId());
            Debug.Log("Character count: " + characterList.Length);
        }
        // Deletes instance if theres more than one instance
        else if (Instance != this)
        {
            // If the character list has characters in the list is saved
            // Prevents character list from disappearing when reloading a prev scene
            if (characterList != null)
            {
                Instance.characterList = characterList;
            }

            // Game object destoryed
            Destroy(gameObject);
        }

    }

    public void Start()
    {
        // By default selected character is the Owl
        if (selectedCharacter == null)
        {
            selectedCharacter = characterList[0];
            Debug.Log("Character Selected - Manager: " + selectedCharacter.GetName());
        }
    }

    public void setCharacter(Character player)
    {
        selectedCharacter = player;
    }



}