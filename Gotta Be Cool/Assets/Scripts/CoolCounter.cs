using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;

public class CoolCounter : MonoBehaviour
{
    public static CoolCounter instance;//singleton instance to make sure only one instance of the script exists

    public int score; //public variable to store the current score
    public int CoolScore 
    {
        get { return score; }
        private set
        {
            score = value;
            Debug.Log("score changed!");
            // Check if the current score is greater than the high score
            if (score > HighScore)
            {
                HighScore = score;
            }
        }
    }
    //private variable to store the high score 
    private int highScore = 0;

    private const string KEY_HIGH_SCORE = "High Score";

    public int HighScore //property to get and set the high score, reading and writing to a file 
    {
        get
        {
            if (File.Exists(DATA_FULL_HS_FILE_PATH))//check if the file with high score data exists
            {
                //read the content of the file and parse it ot an integer
                string fileContents = File.ReadAllText(DATA_FULL_HS_FILE_PATH);
                highScore = Int32.Parse(fileContents);
            }

            return highScore;
        }
        set
        {
            highScore = value; //set high score and save it to the file 
            Debug.Log("New High Score");
            string fileContent = "" + highScore;
    
            if (!Directory.Exists(Application.dataPath + DATA_DIR))//create data directory if it doesn't exist
            {
                Directory.CreateDirectory(Application.dataPath + DATA_DIR);
            }

            File.WriteAllText(DATA_FULL_HS_FILE_PATH, fileContent); //write the high score to the file 
        }
    }

    public int DestroyScore { get; private set; } //public varaible to store the destroy score 

    //constant varaible for the data directory and high score file name 
    private const string DATA_DIR = "/Data/";
    private const string DATA_HS_FILE = "hs.txt";

    //full path to high score file
    private string DATA_FULL_HS_FILE_PATH;

    //reference to textmeshpro for displaying the final score 
    public TextMeshProUGUI CoolScoreText;

    void Awake() //the script instance is being loaded
    {
        if (instance == null)//ensures there is only one instance of the script
        {
            instance = this;
            DontDestroyOnLoad(gameObject);

            // Initialize DATA_FULL_HS_FILE_PATH here
            DATA_FULL_HS_FILE_PATH = Application.dataPath + DATA_DIR + DATA_HS_FILE;
        }
        else
        {
            Destroy(gameObject);//destroy duplicate instance 
        }
    }

    void Start()
    {
        CoolScore = 0;//initalize scores and print debug messages 
        Debug.Log("CoolCounter initialized. CoolScore: " + CoolScore);
        DestroyScore = 0;
        Debug.Log("DestroyCounter initialized. DestroyScore:" + DestroyScore);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Cool")) //if this object collides with an object tagged Cool the cool 
            //score wil go up and that object will be destroyed and the destroy score will go up
        {
            CoolScore++;
            Debug.Log("Collision with Cool object detected. CoolScore: " + CoolScore);

            Destroy(collision.gameObject);
            DestroyScore++;
            Debug.Log("Collision with Cool object detected. DestroyScore: " + DestroyScore);
        }

        if (collision.gameObject.CompareTag("NotCool")) //if this object collides with an object tagged Not Cool the
            //object will be destroyed and the destroy score will go up 
        {
            Destroy(collision.gameObject);
            DestroyScore++;
            Debug.Log("Collision with Cool object detected. DestroyScore: " + DestroyScore);
        }
    }


    // Update is called once per frame
    void Update()
    {
        
            if (DestroyScore == 4) //If Destroy Score is 4 jump to ResultScreen Scene
            {
                SceneManager.LoadScene("ResultScreen");
            }

            if (DestroyScore == 4 && CoolScore == 2) //If destroy score is 4 and cool score is 2 display NOT COOL text
            {
                CoolScoreText.text = "NOT COOL";
            }

            if (DestroyScore == 4 && CoolScore == 3) //If destroy score is 4 and cool score is 3 display COOL text
            {
                CoolScoreText.text = "COOL";
            }

            if (DestroyScore == 4 && CoolScore == 4) //If destroy score is 4 and cool score is 4 display SUPER COOL text
            {
                CoolScoreText.text = "SUPER COOL";
            }
            
    }

}
