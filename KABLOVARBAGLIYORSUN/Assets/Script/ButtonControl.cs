using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonControl : MonoBehaviour
{
    public GameObject[] AllObjects;
    public void StartButton()
    {
        SceneManager.LoadScene(1);
    }
    public void ExitButton()
    {
        Application.Quit();
    }
    public void OpenPanel(GameObject Obj)
    {
        for (int i = 0; i < AllObjects.Length; i++)
        {
            AllObjects[i].SetActive(false);
        }
        Obj.SetActive(true);
    }
}