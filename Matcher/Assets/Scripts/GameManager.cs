using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject board;
    [SerializeField] private GameObject introTxt;
    [SerializeField] private GameObject resetTxt;
    [SerializeField] private GameObject UiManager;


    private bool hasOpened = false;
    void Update()
    {
        if (Input.GetKeyDown("f") && hasOpened == false)
        {
            board.SetActive(true);
            introTxt.SetActive(false);
            hasOpened = true;
            UiManager.SetActive(true);
        }

        if (Input.GetKeyDown("x") && hasOpened == true)
        {
            SceneManager.LoadScene("SampleScene");
        }
    }
}
