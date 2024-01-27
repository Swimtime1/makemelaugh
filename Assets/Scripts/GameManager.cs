using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    // Bool Variables
    public bool gameOver, gameStarted;
    public static bool twoPlayer;
    private bool startActive;

    // GameObject Variables
    public GameObject gameOverScreen, gameWonScreen;
    public GameObject startScreen, inGameUI, tutorial;
    public GameObject p1, p2;

    // TextMeshProUGUI Variables
    public TextMeshProUGUI eventText;

    // Script Variables
    public PlayerController player1Script, player2Script;

    // AudioSource Variables
    public AudioSource titleAudio, mainAudio, AudioFX;

    // AudioClip Variables
    public AudioClip heehaha;
    
    // Start is called before the first frame update
    void Start()
    {
        gameStarted = false;
        gameOver = false;
        startActive = false;

        startScreen.SetActive(true);
        inGameUI.SetActive(false);
        tutorial.SetActive(false);
        gameOverScreen.SetActive(false);
        gameWonScreen.SetActive(false);
        p1.SetActive(false);
        p2.SetActive(false);

        titleAudio.Play();
    }

    // Closes the Start Menu, and gives control of Players
    public void StartGame()
    {
        gameStarted = true;

        tutorial.SetActive(false);
        inGameUI.SetActive(true);

        titleAudio.Stop();
        mainAudio.Play();
    }

    // Update is called once per frame
    void Update()
    {
        // Closes the Tutorial Menu
        if(startActive && Input.anyKeyDown)
        {
            Debug.Log("Enter pressed");
            startActive = false;
            StartGame();
        }
    }

    // Sets the Game Mode to Single Player
    public void SetSingle()
    {
        twoPlayer = false;
        p1.transform.position = new Vector3(0f, -4.43f, 0f);
        p1.SetActive(true);
        OpenTutorial();
    }

    // Sets the Game Mode to Multi-Player
    public void SetMulti()
    {
        twoPlayer = true;
        p1.transform.position = new Vector3(-1.03f, -4.43f, 0f);
        p2.transform.position = new Vector3(1.03f, -4.43f, 0f);
        p1.SetActive(true);
        p2.SetActive(true);
        OpenTutorial();
    }

    // Opens the Tutorial Page
    public void OpenTutorial()
    {
        startScreen.SetActive(false);
        startActive = true;
        tutorial.SetActive(true);
    }
}
