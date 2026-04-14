using UnityEngine;

public class Status : MonoBehaviour
{
    [SerializeField]
    string statusName;

    [SerializeField]
    string description;

    [SerializeField]
    int lifetime;

    [SerializeField]
    int baseDamage;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // return how many turns this status will last
    public int GetLifetime()
    {
        return this.lifetime;
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
