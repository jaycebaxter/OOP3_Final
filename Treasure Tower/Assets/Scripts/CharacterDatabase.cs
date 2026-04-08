using UnityEngine;

// Ability to create a database that contains all characters info
[CreateAssetMenu]
public class CharacterDatabase : ScriptableObject
{
    public Character[] charactersList;

    public int CharacterCount
    {
        get { return charactersList.Length;}
        
    }

    public Character GetCharacter(int characterIndex)
    {
        return charactersList[characterIndex];
    }
    

}
