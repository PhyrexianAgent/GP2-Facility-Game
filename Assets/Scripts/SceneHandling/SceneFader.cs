using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SceneFader : MonoBehaviour
{
    private Image sceneFader;

    private void Awake()
    {
        sceneFader = GetComponent<Image>();
        GameManager.CurrentSceneFader = this;
    }

    public IEnumerator fadeIn(float duration)
    {
        Color startColour = new Color(sceneFader.color.r, sceneFader.color.g, sceneFader.color.b, 1);
        Color endColour = new Color(sceneFader.color.r, sceneFader.color.g, sceneFader.color.b, 0);
        yield return fade(startColour, endColour, duration);
        gameObject.SetActive(false);
    }
    public IEnumerator fadeOut(float duration)
    {
        Color startColour = new Color(sceneFader.color.r, sceneFader.color.g, sceneFader.color.b, 0);
        Color endColour = new Color(sceneFader.color.r, sceneFader.color.g, sceneFader.color.b, 1);
        gameObject.SetActive(true);
        yield return fade(startColour, endColour, duration);
    }
    private IEnumerator fade(Color startColour, Color endColour, float duration)
    {
        float durationElapsed = 0;
        float percentElapsed = 0;
        while (percentElapsed < 1)
        {
            percentElapsed = durationElapsed / duration;
            sceneFader.color = Color.Lerp(startColour, endColour, percentElapsed);

            yield return null;
            durationElapsed += Time.deltaTime;
        }
    }
}
