using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TempLevelSwapScript : MonoBehaviour
{
    [Tooltip("Pick the build index of the level you wish to load")]
    [SerializeField] public int nextLevelIndex = -1;
    public SceneControl sceneControl;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            int sceneToLoad = nextLevelIndex;

            if (nextLevelIndex == -1)
            {
                Debug.Log("Exiting the application.");
                Application.Quit();

                #if UNITY_EDITOR
                    UnityEditor.EditorApplication.isPlaying = false;
                #endif

                return;
            }

            if (sceneToLoad < SceneManager.sceneCountInBuildSettings)
            {
                StartCoroutine(sceneControl.sceneLoader(sceneToLoad));
                //SceneManager.LoadScene(sceneToLoad);
            }
            else
            {
                Debug.LogWarning("Specified scene index is out of range or no more scenes in the build settings to load.");
            }
        }
    }
}
