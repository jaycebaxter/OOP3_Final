using TMPro;
using UnityEngine;

// Used for data persistence 
// 
public class CharacterManager : MonoBehaviour
{
    public Character[] characterList;
    public Character selectedCharacter;
    public int selectedIndex;

    public static CharacterManager Instance { get; private set; }

    public void Awake()
    {
        // Creates instance if there isn't one
        if (Instance == null)
        {
            Instance = this;
            // Prevents gameObjects from being deleted when a new scene is loaded
            DontDestroyOnLoad(gameObject);
        }
        // Deletes instance if theres more than one instance
        else if (Instance != this)
        {
            // If the character list has characters then list is saved
            // Prevents character list from disappearing when reloading a prev scene
            if (characterList != null)
            {
                Instance.characterList = characterList;
            }

            // Game object destoryed
            Destroy(gameObject);
        }


    }

    //public void Start()
    //{
    //    // By default selected character is the Owl
    //    if (selectedCharacter == null)
    //    {
    //        selectedCharacter = characterList[0];

    //    }
    //}

    public void SetCharacter(int playerIndex)
    {
        //selectedCharacter = characterList[playerIndex];  
        //Debug.Log("SET CHARACTER FUNCTION");
        //Debug.Log("Player Name: " + selectedCharacter.GetName());

        //Debug.Log("List: " + characterList.Length);
        //Debug.Log("Index used: " + playerIndex);

        selectedIndex = playerIndex;

    }



}