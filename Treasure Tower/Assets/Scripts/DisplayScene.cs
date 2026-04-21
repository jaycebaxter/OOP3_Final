using UnityEngine;
using TMPro;
using UnityEngine.UI;

// Trying to make it so that depending on who the user selects,
// load CharacterSelected scene, grab character info
// Replace placeholders with characters information

public class DisplayScene : MonoBehaviour
{
    public TextMeshProUGUI characterName;
    public TextMeshProUGUI characterInfo;
    public Image characterImage;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Debug.Log("DISPLAY SCENE");
        Debug.Log("List null? " + (CharacterManager.Instance.characterList == null));
        Debug.Log("List length: " + CharacterManager.Instance.characterList?.Length);

        var manager = CharacterManager.Instance;
        var player = manager.characterList[manager.selectedIndex];

        //if (manager == null || player == null)
        //{
            
        //    Debug.Log("No character selected.");
        //    return;
        //}




        //Debug.Log("DISPLAY SCENE");
        ////Debug.Log("Manager Exist!");
        ////Debug.Log("Instance ID - Character Manager: " + manager.GetEntityId());
        //Debug.Log("Character List Length: " + manager.characterList.Length);
        //Debug.Log("Selected character: " + player.GetName());



        //foreach (var character in manager.characterList)
        //{
        //    Debug.Log("Character Names: " + character.GetName());
        //}

        string name = player.GetName();
        string health = player.GetHealth().ToString();
        string defense = player.GetDefense().ToString();
        string movement = player.GetMovement().ToString();
        string attack = player.GetAttack().ToString();

        Sprite[] playerSprites = player.GetSprites();

        // Change Text
        characterName.text = name;
        characterInfo.text =
            "Health: " + health
            + "\nAttack: " + attack
            + "\nDefense: " + defense
            + "\nMovement: " + movement;

        // Change image to be selected character facing forward
        characterImage.sprite = playerSprites[0];
    }

}
