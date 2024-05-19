using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelExit : MonoBehaviour
{
    //** For use if trying to move between levels that aren't 1 index above current level
    //[SerializeField] int nextLevelIndex = -1;  **  

    [SerializeField] float loadDelay = 1f;

    //private void Start()
    //{
    //    if (nextLevelIndex < 0) **
    //    {
    //        Debug.LogError("Next level not set.");
    //    }
    //}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
            StartCoroutine(LoadNextLevel());
    }

    IEnumerator LoadNextLevel()
    {
        yield return new WaitForSeconds(loadDelay);

        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;

        FindObjectOfType<ScenePersist>().ResetScenePersist();
        
        if (nextSceneIndex == SceneManager.sceneCountInBuildSettings)
        {
            nextSceneIndex = 0;
        }

        //SceneManager.LoadScene(nextLevelIndex); **
        SceneManager.LoadScene(nextSceneIndex);
    }
}
