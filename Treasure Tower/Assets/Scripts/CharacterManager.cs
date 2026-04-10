using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

// This script is keeping track of which character was selected
// To work each scene needs a game object with this attached script
public class CharacterManager : MonoBehaviour
{
    public GameObject[] characterList;
    public GameObject selectedCharacter;

    public TextMeshPro characterName;
    public TextMeshPro characterHealth;
    public TextMeshPro characterDefense;
    public TextMeshPro characterAttack;
    public TextMeshPro characterMovement;

    public static CharacterManager instance { get; private set; }

    public void LoadCharacterInfo()
    {
        
    }




    //// Private field that can only be accessed with getter
    //private Character selectedCharacterPrivate;
    //public Character selectedCharacter
    //{
    //    get => selectedCharacterPrivate;
    //    set => selectedCharacterPrivate = value;
    //}


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
