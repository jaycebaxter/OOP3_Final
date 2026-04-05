using UnityEngine;

public class Drops
{
    [SerializeField]
    Item[] commonDrops;

    [SerializeField]
    Item[] uncommonDrops;

    [SerializeField]
    Item[] rareDrops;

    // simple random selection of an item in a drop list
    public Item GetItemDrop()
    {
        Item chosenItem;
        int randomChance = Random.Range(0, 10);
        int randomIndex;

        if (randomChance < 6)
        {
            randomIndex = Random.Range(0, this.commonDrops.Length);
            chosenItem = commonDrops[randomIndex];
        }
        else if (randomChance <= 9)
        {
            randomIndex = Random.Range(0, this.uncommonDrops.Length);
            chosenItem = uncommonDrops[randomIndex];
        }
        else
        {
            randomIndex = Random.Range(0, this.rareDrops.Length);
            chosenItem = rareDrops[randomIndex];
        }

        return chosenItem;
    }
}
