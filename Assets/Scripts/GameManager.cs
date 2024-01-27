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

    // GameObject Variables
    public GameObject gameOverScreen, gameWonScreen;
    public GameObject startScreen, inGameUI;
    public GameObject p1, p2;

    // TextMeshProUGUI Variables
    public TextMeshProUGUI eventText;

    // Script Variables
    public PlayerController player1Script, player2Script;
    
    // Start is called before the first frame update
    void Start()
    {
        gameStarted = false;
        gameOver = false;

        startScreen.SetActive(true);
        inGameUI.SetActive(false);
        gameOverScreen.SetActive(false);
        gameWonScreen.SetActive(false);
        p1.SetActive(true);
        p2.SetActive(false);
    }

    // Closes the Start Menu, and gives control of Players
    public void StartGame()
    {
        gameStarted = true;

        startScreen.SetActive(false);
        inGameUI.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Sets the Game Mode to Single Player
    public void SetSingle()
    {
        twoPlayer = false;
        p1.transform.position = new Vector3(0f, -4.43f, 0f);
        StartGame();
    }

    // Sets the Game Mode to Multi-Player
    public void SetMulti()
    {
        twoPlayer = true;
        p1.transform.position = new Vector3(-1.03f, -4.43f, 0f);
        p2.transform.position = new Vector3(1.03f, -4.43f, 0f);
        p2.SetActive(true);
        StartGame();
    }
}
