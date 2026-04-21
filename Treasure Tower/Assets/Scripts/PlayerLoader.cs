using Unity.InferenceEngine;
using UnityEngine;

// attach to the character objects
// allows the characters to move, interacting with the game grid
public class PlayerLoader : MonoBehaviour
{
    // Storing characters game objects in list
    //public GameObject[] characterModels;

    // This will store character info
    //public SpriteRenderer renderSprites;


    // Get child object of player
    Transform modelHolder;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        modelHolder = transform.GetChild(0);
        LoadCharacter(CharacterManager.Instance.selectedIndex);
    }

    public void LoadCharacter(int index)
    {
        var manager = CharacterManager.Instance;

        //if (manager == null || manager.selectedCharacter == null)
        //{
        //    Debug.Log("PLAYER LOADER");
        //    Debug.Log("No character selected.");
        //    return;
        //}

        // Get the selectedCharacter
        var player = manager.characterList[index];

        Debug.Log("PlayerLoader Player Name: " + player.GetName());
    }

    //void Start()
    //{
    //    var manager = CharacterManager.Instance;
    //    modelHolder = transform.GetChild(0);

    //    Debug.Log("Model Holder: " + modelHolder.name);

    //    if (manager != null )
    //    {
    //        Character selected = manager.selectedCharacter;

    //        if (selected != null)
    //        {
    //            Debug.Log("Player Loader Name: " + selected.GetName());
    //        }
    //        else
    //        {
    //            Debug.LogError("Selected character is null");
    //        }


    //    }
    //    else
    //    {
    //        Debug.LogError("Manager is null");
    //    }


    //    Debug.Log("PlayerLoader Instance ID: " + manager.GetEntityId());
    //    Debug.Log("List Null? " + (manager.characterList == null));
    //    Debug.Log("Selected Character Name: " + manager.selectedCharacter.GetName());
    //    Debug.Log("List Length: " + manager.characterList.Length);
    //    Debug.Log("Selected Character: " + manager.selectedCharacter);


    //string characterName = selected.GetName();



    //Sprite forward = selected.GetSprites()[0];

    //GetComponent<SpriteRenderer>().sprite = forward;



    //// First turn off all models
    //foreach (GameObject model in characterModels) {
    //    model.SetActive(false);

    //}

    //// Only active the model that is selected
    //foreach (GameObject model in characterModels)
    //{
    //    if (model.name == characterName)
    //    {
    //        // Activate model and exit loop
    //        model.SetActive(true);
    //        break;
    //    }
    //}

}


