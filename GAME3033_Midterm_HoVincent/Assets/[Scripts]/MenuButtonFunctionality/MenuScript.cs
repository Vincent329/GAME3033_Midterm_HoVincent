using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuScript : MonoBehaviour
{

    private Button button;
    // Start is called before the first frame update
    void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(GameSceneManager.Instance.LoadMenuScreen);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
