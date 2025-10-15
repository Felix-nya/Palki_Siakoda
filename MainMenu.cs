using UnityEngine;
using UnityEngine.SceneManagement;


public class MenuController : MonoBehaviour
{
    public void AppExit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.ExitPlaymode();
#endif

        Application.Quit();

        Debug.Log("Exit");
    }

    public void LoadPlayerVSBot()
    {
        SceneManager.LoadScene(1);
    }

    public void LoadPlayerVSPlayer()
    {
        SceneManager.LoadScene(2);                                                  
    }

    public void LoadBotVSBot()
    {
        SceneManager.LoadScene(3);
    }

}
