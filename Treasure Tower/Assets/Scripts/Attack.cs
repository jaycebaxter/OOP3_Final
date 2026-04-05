using UnityEngine;

public class Attack : MonoBehaviour
{
    // still a wip, and we need a subclass for tileattack
    [SerializeField]
    string attackName;

    [SerializeField]
    string description;

    [SerializeField]
    int baseDamage;

    [SerializeField]
    Status? statusEffect;

    public string GetName()
    {
        return this.attackName;
    }

    public string GetDescription()
    {
        return this.description;
    }

    public int GetDamage()
    {
        return this.baseDamage;
    }

    public bool HasStatus()
    {
        if (this.statusEffect is null)
        {
            return false;
        }
        return true;
    }

    public Status GetStatusEffect()
    {
        return this.statusEffect;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() { }

    // Update is called once per frame
    void Update() { }
}
