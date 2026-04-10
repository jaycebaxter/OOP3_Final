using UnityEngine;

//[CreateAssetMenu(fileName = "New Character")]
public class Character : MonoBehaviour
{
    [SerializeField]
    private string characterName;

    [SerializeField]
    private Sprite[] characterSprites;

    [SerializeField]
    private string description;
    
    [SerializeField]
    private int health;

    [SerializeField]
    private int attack;

    [SerializeField]
    private int defense;

    [SerializeField]
    private int movement;

    [SerializeField]
    private int accuracy;

    [SerializeField]
    private Item? heldItem;

    [SerializeField]
    private Status? currentStatus;

    [SerializeField]
    private Attack[] attackArray;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() { }

    // Update is called once per frame
    void Update() { }

    // gets all of the stats for display purposes
    public int[] GetStats()
    {
        // get the player's current stats
        int attack = this.attack;
        int defense = this.defense;
        int accuracy = this.accuracy;
        int health = this.health;

        // apply extra stats if the player has a held item
        if (HasItem())
        {
            int[] statBoosts = this.heldItem.GetStatBoosts();
            // add all stat boosts - remember, these can be 0
            attack += statBoosts[0];
            defense += statBoosts[1];
            accuracy += statBoosts[2];
            health += statBoosts[3];
        }

        int[] statArray = new int[4] { attack, defense, accuracy, health };

        return statArray;
    }

    // returns whether or not the character has a status currently
    public bool HasStatus()
    {
        if (this.currentStatus is null)
        {
            return false;
        }
        return true;
    }

    // returns the current status for game purposes
    public Status GetStatus()
    {
        return currentStatus;
    }

    // returns if there is an item held by the player
    public bool HasItem()
    {
        if (this.heldItem is null)
        {
            return false;
        }
        return true;
    }

    // returns the held item
    public Item GetItem()
    {
        return heldItem;
    }

    // how far the character can move
    // dedicated method due to the board needing access
    public int GetMovementAmt()
    {
        int movement = this.movement;

        if (HasItem())
        {
            movement += this.heldItem.GetMovementBoost();
        }

        return movement;
    }

    // sets a new health value, and returns the current health value for game update purposes
    public int ChangeHealth(int damage)
    {
        int finalHealth;
        // the stats, also affected by the held item (if applicable)
        int[] currentStats = GetStats();
        // filtering for relevant stats here
        health = currentStats[3];
        defense = currentStats[1];

        // modifies how much damage is dealt
        // some examples: 60 (damage) / 1.6 (defense stat is 60) = 37.5
        // 60 (damage) / 1,1 (defense stat is 10) = 54.5
        damage = System.Convert.ToInt32(damage / ((defense + 100) / 100));

        finalHealth = health - damage;

        // change to 0 if health goes negative
        if (finalHealth < 0)
        {
            finalHealth = 0;
        }

        // update the character's health
        this.health = finalHealth;

        // return a value so the game can decide if it's a game over, or update UI
        return finalHealth;
    }

    public Attack[] GetAttacks()
    {
        return this.attackArray;
    }

    public string GetName()
    {
        return this.characterName;
    }

    public string GetDescription()
    {
        return this.description;
    }
}
