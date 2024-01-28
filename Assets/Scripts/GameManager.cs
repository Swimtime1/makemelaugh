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
    public GameObject p1Tutorial, p2Tutorial;
    public GameObject p1Grenades, p2Grenades;
    public GameObject ground;

    // TextMeshProUGUI Variables
    public TextMeshProUGUI contText;
    public TextMeshProUGUI p1Move, p1Title, p1Shoot, p1NumGrenades;
    public TextMeshProUGUI p2Move, p2Title, p2Shoot, p2NumGrenades;

    // Script Variables
    public PlayerController player1Script, player2Script;

    // AudioSource Variables
    public AudioSource titleAudio, mainAudio;

    // AudioClip Variables
    public AudioClip heehaha;

    // Slider Variables
    public Slider healthBar1, healthBar2;

    // PlayerController Variables
    public PlayerController p1Script, p2Script;
    
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
        ground.SetActive(false);

        titleAudio.Play();

        contText.text = "";
        p1Move.text = "";
        p1Shoot.text = "";
        p1Title.text = "";
        p2Move.text = "";
        p2Shoot.text = "";
        p2Title.text = "";
    }

    // Closes the Start Menu, and gives control of Players
    public void StartGame()
    {
        ground.SetActive(true);
        
        // Activates the necessary number of players
        if(twoPlayer) { p2.SetActive(true); }
        p1.SetActive(true);

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
            startActive = false;
            StartGame();
        }

        healthBar1.value = (player1Script.GetHealth() / 100f);
        healthBar2.value = (player2Script.GetHealth() / 100f);

        // Updates the UI for how many grenades each player has
        if(player1Script.GetGrenades() > 0)
        {
            p1Grenades.SetActive(true);
            p1NumGrenades.text = "x " + player1Script.GetGrenades();
        }
        else { p1Grenades.SetActive(false); }
        if(player2Script.GetGrenades() > 0)
        {
            p2Grenades.SetActive(true);
            p2NumGrenades.text = "x " + player2Script.GetGrenades();
        }
        else { p2Grenades.SetActive(false); }
    }

    // Sets the Game Mode to Single Player
    public void SetSingle()
    {
        twoPlayer = false;
        p1.transform.position = new Vector3(0f, -4.43f, 0f);
        OpenTutorial();
    }

    // Sets the Game Mode to Multi-Player
    public void SetMulti()
    {
        twoPlayer = true;
        p1.transform.position = new Vector3(-1.03f, -4.43f, 0f);
        p2.transform.position = new Vector3(1.03f, -4.43f, 0f);
        OpenTutorial();
    }

    // Opens the Tutorial Page
    public void OpenTutorial()
    {
        startScreen.SetActive(false);
        startActive = true;

        // Determines whether to display information for another player
        if(twoPlayer)
        {
            float xPos1 = p1Tutorial.transform.position.x;
            float yPos1 = p1Tutorial.transform.position.y;
            
            float xPos2 = p2Tutorial.transform.position.x;
            float yPos2 = p2Tutorial.transform.position.y;
            
            p1Tutorial.transform.position = new Vector3(xPos1, yPos1, 0f);
            p2Tutorial.transform.position = new Vector3(xPos2, yPos2, 0f);
        }
        else
        {
            float xPos = (p1Tutorial.transform.position.x + p2Tutorial.transform.position.x) / 2;
            float yPos = p1Tutorial.transform.position.y;
            
            p1Tutorial.transform.position = new Vector3(xPos, yPos, 0f);
            p2Tutorial.SetActive(false);
        }

        tutorial.SetActive(true);
        StartCoroutine(DisplayTutorial());
    }

    // Displays Tutorial Information at Type-Writer Pace
    IEnumerator DisplayTutorial()
    {
        yield return new WaitForSeconds(0.1f);
        
        // Determines whether to display information for another player
        if(twoPlayer) { StartCoroutine(DisplayP2()); }
        StartCoroutine(DisplayP1());
    }

    // Types out information for Player 1
    IEnumerator DisplayP1()
    {
        string text;
        
        // Types out how p1 moves, one letter at a time
        text = "Player 1";
        foreach (char i in text) 
        {
            yield return new WaitForSeconds(0.1f);
            p1Title.text += i;
        }
        
        // Types out Player 1, one letter at a time
        text = "How To Move\nW - Move Up\nS - Move Down\nA - Move Left\nD - Move Right";
        foreach (char i in text) 
        {
            yield return new WaitForSeconds(0.1f);
            p1Move.text += i;
        }

        // Types out how p1 shoots, one letter at a time
        text = "How To Shoot";
        foreach (char i in text) 
        {
            yield return new WaitForSeconds(0.1f);
            p1Shoot.text += i;
        }

        StartCoroutine(HowToStart());
    }

    // Types out information for Player 2
    IEnumerator DisplayP2()
    {
        string text;
        
        // Types out Player 2, one letter at a time
        text = "Player 2";
        foreach (char i in text) 
        {
            yield return new WaitForSeconds(0.1f);
            p2Title.text += i;
        }
    
        // Types out how p2 moves, one letter at a time
        text = "How To Move\nI - Move Up\nK - Move Down\nJ - Move Left\nL - Move Right";
        foreach (char i in text) 
        {
            yield return new WaitForSeconds(0.1f);
            p2Move.text += i;
        }

        // Types out how p2 shoots, one letter at a time
        text = "How To Shoot";
        foreach (char i in text) 
        {
            yield return new WaitForSeconds(0.1f);
            p2Shoot.text += i;
        }
    }

    // Types out how to start playing
    IEnumerator HowToStart()
    {
        string text = "Press any button to start playing!";

        // Types out how to start playing one letter at a time
        foreach (char i in text) 
        {
            yield return new WaitForSeconds(0.1f);
            contText.text += i;
        }
    }
}
