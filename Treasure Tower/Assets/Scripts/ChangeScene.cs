using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{
    private int prevScene;

    private void Start()
    {
        prevScene = SceneManager.GetActiveScene().buildIndex - 1;
    }

    public void goToScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void goBack()
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
