using UnityEngine;

public class Item : MonoBehaviour
{
    [SerializeField]
    string itemName;

    [SerializeField]
    string description;

    [SerializeField]
    string type;

    [SerializeField]
    int healing;

    [SerializeField]
    int healthBoost;

    [SerializeField]
    int attackBoost;

    [SerializeField]
    int defenseBoost;

    [SerializeField]
    int movementBoost;

    [SerializeField]
    int accuracyBoost;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() { }

    // Update is called once per frame
    void Update() { }

    // checks if the item is capable of healing health
    public bool HasHealing()
    {
        if (this.healing > 0)
        {
            return true;
        }
        return false;
    }

    // returns the healing amount
    public int HealAmount()
    {
        return this.healing;
    }

    public int GetMovementBoost()
    {
        return this.movementBoost;
    }

    // gets the stat boosts from the item, usually for adding to the player's stats
    public int[] GetStatBoosts()
    {
        int[] statBoosts = new int[]
        {
            this.attackBoost,
            this.defenseBoost,
            this.accuracyBoost,
            this.healthBoost,
        };

        return statBoosts;
    }
}
