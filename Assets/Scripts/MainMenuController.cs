using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{

    public CanvasGroup canvasGroup;

    public void PlayGame()
    {
        StartCoroutine(FadeLoadingScreen(2));
    }

    public void Options()
    {

    }

    public void QuitGame()
    {
        Application.Quit();
    }

    IEnumerator FadeLoadingScreen(float duration)
    {
        float startValue = canvasGroup.alpha;
        float time = 0;

        while(time < duration)
        {
            canvasGroup.alpha = Mathf.Lerp(startValue, 1, time / duration);
            time += Time.deltaTime;
        }

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("level_snow");
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }
}
