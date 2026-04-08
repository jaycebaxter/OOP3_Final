using UnityEngine;

// This script is keeping track of which character was selected
// To work each scene needs a game object with this attached script
public class CharacterManager : MonoBehaviour
{
    public static CharacterManager instance { get; private set; }

    // Private field that can only be accessed with getter
    private Character selectedCharacterPrivate;
    public Character selectedCharacter
    {
        get => selectedCharacterPrivate;
        set => selectedCharacterPrivate = value;
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
