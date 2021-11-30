using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChange : MonoBehaviour
{
    public void SceneNoAnim()
    {
        SceneManager.LoadScene("NoAnimation");
    }
    public void SceneAnim()
    {
        SceneManager.LoadScene("AnimationOnly");
    }
    public void SceneFull()
    {
        SceneManager.LoadScene("FullyInteractive");
    }
    public void SceneTest()
    {
        SceneManager.LoadScene("Testing");
    }
}
