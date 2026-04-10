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
            DontDestroyOnLoad(gameObject);
        }
        // Deletes game object if theres more than one instance
        else
        {
            Destroy(gameObject);
        }
    }

    public void Start()
    {
        // By default selected character is the Owl
        if (selectedCharacter == null)
        {
            selectedCharacter = characterList[0];
        }
    }

    public void setCharacter(Character player)
    {
        selectedCharacter = player;
    }



}