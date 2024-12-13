using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneControl : MonoBehaviour
{
    [SerializeField] private float fadeDuration;
    private SceneFader sceneFade;
    [SerializeField] AudioSource SFX;

    [SerializeField] private AudioClip levelEnd;
    [SerializeField] private AudioClip levelBegin;


    private void Awake()
    {
        sceneFade = GetComponentInChildren<SceneFader>();
    }

    public void loadScene(string sceneName)
    {
        StartCoroutine(sceneLoader(sceneName));
    }

    // Start is called before the first frame update
    private IEnumerator Start()
    {
        SFX.PlayOneShot(levelBegin);
        yield return sceneFade.fadeIn(fadeDuration);
    }
    private IEnumerator sceneLoader(string sceneName)
    {
        SFX.PlayOneShot(levelEnd);
        yield return sceneFade.fadeOut(fadeDuration);
        yield return SceneManager.LoadSceneAsync(sceneName);
    }
    //purely for test purposes
    private void Update()
    {
        if (Input.GetKey("1"))
        {
            loadScene("Level 1");
        }
        else if (Input.GetKey("2"))
        {
            loadScene("Level 2");
        }
        else if (Input.GetKey("3"))
        {
            loadScene("Level 3");
        }
        else if (Input.GetKey("m"))
        {
            loadScene("MainMenu");
        }
    }


}
