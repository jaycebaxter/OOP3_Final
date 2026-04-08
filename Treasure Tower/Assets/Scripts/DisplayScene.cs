using UnityEngine;
using TMPro;

// Trying to make it so that depending on who the user selects,
// load CharacterSelected scene, grab character info
// Replace placeholders with characters information

public class DisplayScene : MonoBehaviour
{
    //public TextMeshProUGUI nameHolder;
    public TextMeshPro nameHolder;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        var character = CharacterManager.instance.selectedCharacter;

        if (CharacterManager.instance == null)
        {
            Debug.LogError("Instance is null");
            return;
        }

        if (character == null)
        {
            Debug.LogError("Character is null");
            return;

        }

        if (nameHolder == null)
        {
            Debug.LogError("Name Holder is null");
            return;
        }

        //if (character != null)
        //{
        //    Debug.Log("Character not null. Character Name: " + character.GetName());

        //    try
        //    {
        //        if (nameHolder.text != null)
        //        {
        //            nameHolder.text = character.GetName();
        //        }
        //        else
        //        {
        //            Debug.Log("Name is null :(");
        //        }
        //    }
        //    catch (System.Exception err)
        //    {
        //        Debug.Log("Error Caught: " + err);
        //    }
            

            
        //}

        //else
        //{
        //    Debug.Log("Character is Null :(");
        //}
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
