using UnityEngine;
using UnityEngine.SceneManagement;

// Changes the scene
public class ChangeScene : MonoBehaviour
{
    public int prevScene;

    public void Start()
    {
        prevScene = SceneManager.GetActiveScene().buildIndex - 1;
    }

    public void GoToScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void GoBack()
    {
        if (prevScene < 0)
        {
            // Takes player back to character selection scene
            prevScene = 1;
        }
        else
        {
            foreach (var character in CharacterManager.Instance.characterList)
            {
                Debug.Log("Character Name - Button: " + character.GetName());
            }

            //for (int i = 0; i < CharacterManager.Instance.characterList.Length; i++)
            //{
            //    Debug.Log($"Index BUTTON {i}: {CharacterManager.Instance.characterList[i].GetName()}");
            //}


            SceneManager.LoadScene(prevScene);
        }

    }



}
