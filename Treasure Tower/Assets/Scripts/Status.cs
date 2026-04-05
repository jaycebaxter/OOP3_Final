using UnityEngine;

public class Status
{
    [SerializeField]
    string statusName;

    [SerializeField]
    string description;

    [SerializeField]
    int lifetime;

    [SerializeField]
    int baseDamage;

    // return how many turns this status will last
    public int TurnsLeft()
    {
        return this.lifetime;
    }

    // reduce lifetime and return if the status should stay active
    public bool StatusContinues()
    {
        this.lifetime -= 1;

        if (this.lifetime == 0)
        {
            return false;
        }

        return true;
    }

    public int GetDamage()
    {
        return this.baseDamage;
    }

    public string GetName()
    {
        return this.statusName;
    }

    public string GetDescription()
    {
        return this.description;
    }
}
