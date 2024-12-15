using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneControl : MonoBehaviour
{
    [SerializeField] private float fadeDuration;
    private SceneFader sceneFade;
    [SerializeField] AudioSource SFX;

    [SerializeField] private AudioClip levelEnd1;
    [SerializeField] private AudioClip levelEnd2;
    [SerializeField] private AudioClip levelBegin1;
    [SerializeField] private AudioClip levelBegin2;


    private void Awake()
    {
        sceneFade = GetComponentInChildren<SceneFader>();
    }

    public void loadScene(string sceneName)
    {
        StartCoroutine(sceneLoader(sceneName));
    }

    public void loadScene(int sceneIndex)
    {
        if (sceneIndex != -1)
        {
            StartCoroutine(sceneLoader(sceneIndex));
        }
        else
        {
            Debug.Log("Quit Game");
            Application.Quit();
        }
    }

    // Start is called before the first frame update
    private IEnumerator Start()
    {
        SFX.PlayOneShot(levelBegin1);
        yield return sceneFade.fadeIn(fadeDuration);
        yield return new WaitForSeconds(0.1f);
        SFX.PlayOneShot(levelBegin2);
    }
    private IEnumerator sceneLoader(string sceneName)
    {
        SFX.PlayOneShot(levelEnd1);
        yield return new WaitForSeconds(0.3f);
        SFX.PlayOneShot(levelEnd2);
        yield return new WaitForSeconds(0.5f);
        yield return sceneFade.fadeOut(fadeDuration);
        yield return SceneManager.LoadSceneAsync(sceneName);
    }
    private IEnumerator sceneLoader(int sceneIndex)
    {
        SFX.PlayOneShot(levelEnd1);
        yield return new WaitForSeconds(0.3f);
        SFX.PlayOneShot(levelEnd2);
        yield return new WaitForSeconds(0.5f);
        yield return sceneFade.fadeOut(fadeDuration);
        yield return SceneManager.LoadSceneAsync(sceneIndex);
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
