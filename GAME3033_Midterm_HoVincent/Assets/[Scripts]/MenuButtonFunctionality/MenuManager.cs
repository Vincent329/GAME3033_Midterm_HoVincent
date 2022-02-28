using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public GameObject Instructions;
    public GameObject Credits;
    public Button InstructionsButton, CreditsButton;

    // Start is called before the first frame update
    void Start()
    {
        InstructionsButton.onClick.AddListener(ActivateInstructions);
        CreditsButton.onClick.AddListener(ActivateCredits);

        Instructions.SetActive(false);
        Credits.SetActive(false);
    }

    void ActivateInstructions()
    {
        Instructions.SetActive(true);
        Credits.SetActive(false);

    }
    void ActivateCredits()
    {
        Instructions.SetActive(false);
        Credits.SetActive(true);

    }
}
