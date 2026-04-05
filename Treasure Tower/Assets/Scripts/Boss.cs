using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    [SerializeField]
    string bossName;

    [SerializeField]
    string description;

    [SerializeField]
    int attack;

    [SerializeField]
    int defense;

    [SerializeField]
    int accuracy;

    [SerializeField]
    int health;

    [SerializeField]
    Drops drops;

    [SerializeField]
    Status? currentStatus;

    [SerializeField]
    Attack[] attackArray;

    [SerializeField]
    Attack[] tileAttackArray;

    // note: paired with a 2d array
    // example: "raccoon" = {
    // {0, 30}
    // {31, 40}
    // {41, 55}
    // {56, 100} }
    // first value is the minimum, next is the maximum
    [SerializeField]
    Dictionary<string, int[,]> attackDict;

    [SerializeField]
    Dictionary<string, int[,]> tileAttackDict;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() { }

    // Update is called once per frame
    void Update() { }

    // get all info about the boss' stats
    public int[] GetStats()
    {
        // get the boss' current stats
        int attack = this.attack;
        int defense = this.defense;
        int accuracy = this.accuracy;
        int health = this.health;

        int[] statArray = new int[4] { attack, defense, accuracy, health };

        return statArray;
    }

    public bool HasStatus()
    {
        if (this.currentStatus is null)
        {
            return false;
        }
        return true;
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

        // update the boss' health
        this.health = finalHealth;

        // return a value so the game can decide if it's a game over, or update UI
        return finalHealth;
    }

    // basic 'AI' choosing an attack for the boss to play
    // relies on randomly generated values
    public void ChooseAttack(string playerName, bool tileAttack)
    {
        int randomNumber = Random.Range(0, 100);
        Attack chosenAttack;

        // set up the likelihood of the attacks based on player character
        int[,] attackWeights;
        if (tileAttack)
        {
            attackWeights = this.attackDict[playerName];
        }
        else
        {
            attackWeights = this.tileAttackDict[playerName];
        }

        // might be kinda dumb, excuse me. give me a bit to figure this out

        // return chosenAttack;
    }

    public string GetName()
    {
        return this.bossName;
    }

    public string GetDescription()
    {
        return this.description;
    }
}
