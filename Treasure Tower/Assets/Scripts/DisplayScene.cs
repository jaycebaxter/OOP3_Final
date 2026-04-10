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
        if (CharacterManager.Instance == null)
        {
            Debug.LogError("Instance is null");
        }

        var manager = CharacterManager.Instance;

        if (manager == null || manager.selectedCharacter == null)
        {
            Debug.LogError("No character selected.");
            return;
        }

        var player = manager.selectedCharacter;

        if (player == null)
        {
            Debug.LogError("Player is null :(");
            return;
        }

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
            + "Defense: " + defense
            + "Movement: " + movement;

        // Change image to be character facing forward
        characterImage.sprite = playerSprites[0];
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
