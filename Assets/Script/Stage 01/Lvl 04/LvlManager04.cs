using UnityEngine;
using UnityEngine.SceneManagement;

public class LvlManager04 : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void next()
    {
        SceneManager.LoadScene("Stage1_Lvl 5");
    }

    public void Exit()
    {
        SceneManager.LoadScene("Stage Map");
    }
}
