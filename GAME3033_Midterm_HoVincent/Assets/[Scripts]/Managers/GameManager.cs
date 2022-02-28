using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
    public bool cursorActive = true;
    public bool isPaused = false;

    public int totalEnemies;

    public delegate void UpdateText(int count);
    public event UpdateText UpdateTextCount;

    private static GameManager instance;
    public static GameManager Instance
    {
        get => instance;
    }
    private void Awake()
    {
        if (instance != null && instance != null)
        {
            Destroy(this.gameObject);
            return;
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }


    private void OnEnable()
    {
        AppEvents.MouseCursorEnabled += EnableCursor;
        AppEvents.PauseEnabled += PauseGame;
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        AppEvents.MouseCursorEnabled -= EnableCursor;
        AppEvents.PauseEnabled -= PauseGame;

    }
    
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        ResetCount();
    }

    // Start is called before the first frame update
    void Start()
    {
        totalEnemies = FindObjectsOfType<EnemyAI>().Length;
        Debug.Log(totalEnemies);
    }

    public void ResetCount()
    {
        totalEnemies = FindObjectsOfType<EnemyAI>().Length;

    }
    // Update is called once per frame
    void Update()
    {

    }

    public void UpdateEnemyCount()
    {
        totalEnemies--;
        UpdateTextCount(totalEnemies);
        ScoreHolder.ScoreTrack = totalEnemies;
        if (totalEnemies <= 0)
        {
            GameSceneManager.Instance.LoadEndScene();
        }
    }

    void EnableCursor(bool enable)
    {
        if (enable)
        {
            cursorActive = true;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            cursorActive = false;
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    void PauseGame(bool paused)
    {
        if (paused)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }

        EnableCursor(paused);

    }
}