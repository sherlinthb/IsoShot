using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class MusicManager : MonoBehaviour {

	public AudioClip mainTheme;
    public AudioClip menuTheme;

    string sceneName;

    void Start()
    {
        AudioManager.instance.PlayMusic(menuTheme, 2);
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnLevelThatWasLoaded;
    }
    //Depreciated
    //void OnLevelWasLoaded(int sceneIndex)
    //{
      //  string newSceneName = SceneManager.GetActiveScene().name;
      //  if(newSceneName != sceneName)
       // {
         //   sceneName = newSceneName;
         //   Invoke("PlayMusic", .2f);
       // }
   // }

    void OnLevelThatWasLoaded(Scene scene, LoadSceneMode mode)
    {
        string newSceneName = SceneManager.GetActiveScene().name;
        if (newSceneName != sceneName)
        {
            sceneName = newSceneName;
            Invoke("PlayMusic", .2f);
        }
    }

    void PlayMusic()
    {
        AudioClip clipToPlay = null;

        if (sceneName == "Menu")
        {
            clipToPlay = menuTheme;
        } else if (sceneName == "Scene1")
        {
            clipToPlay = mainTheme;
        }

        if(clipToPlay != null)
        {
            AudioManager.instance.PlayMusic(clipToPlay, 2);
            Invoke("PlayMusic", clipToPlay.length);
        }
    }
}
