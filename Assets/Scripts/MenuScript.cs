using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour
{
    private GameObject content;
    private static MenuScript prevInstance = null;

    void Start()
    {
        if (prevInstance == null)
        {
            prevInstance = this;
            content = transform.Find("Content").gameObject;
        }
        else
        {
            Destroy(this.gameObject);
        }
        Time.timeScale = gameObject.activeInHierarchy ? 0f : 1f;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Time.timeScale = 1 - Time.timeScale;
            content.SetActive(!content.activeInHierarchy);
        }
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            // Load scene 1
            SceneManager.LoadScene(1);
        }
    }
}