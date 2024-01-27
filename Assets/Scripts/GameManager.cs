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
        StartGame();
    }

    // Sets the Game Mode to Multi-Player
    public void SetMulti()
    {
        twoPlayer = true;
        p2.SetActive(true);
        StartGame();
    }
}
