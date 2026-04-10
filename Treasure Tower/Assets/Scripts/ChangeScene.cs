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
            prevScene = 0;
        }
        else
        {
            SceneManager.LoadScene(prevScene);
        }

    }



}
