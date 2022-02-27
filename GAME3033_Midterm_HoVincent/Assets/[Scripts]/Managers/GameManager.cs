using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
    public bool cursorActive = true;
    public bool isPaused = false;

    public int totalEnemies;

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
    }

    private void OnDisable()
    {
        AppEvents.MouseCursorEnabled -= EnableCursor;
        AppEvents.PauseEnabled -= PauseGame;

    }

    // Start is called before the first frame update
    void Start()
    {
        totalEnemies = FindObjectsOfType<EnemyAI>().Length;
        Debug.Log(totalEnemies);
    }
    // Update is called once per frame
    void Update()
    {

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