using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScoreKeeper : MonoBehaviour {

    public static ScoreKeeper scoreKeep;
    public static int score { get; private set;}
    float lastEnemyKillTime;
    int streakCount;
    int kills;

    public static string scoreText;
    public int tableID = 263848;
    public string extraData = " ";

    Text text;
    public GameObject killStreakText0;
    public GameObject killStreakText1;
    public GameObject killStreakText2;
    public GameObject killStreakText3;

    // Use this for initialization
    void Start () {
        if(scoreKeep == null)
        {
            scoreKeep = this;
            text = GetComponent<Text>();

        }else if(scoreKeep != this)
        {
            Destroy(gameObject);
        }
        killStreakText0.SetActive(false);
        killStreakText1.SetActive(false);
        killStreakText2.SetActive(false);
        killStreakText3.SetActive(false);
        score = 0;
        Enemy.OnDeathStatic += OnEnemyKilled;
        Player.OnHurtStatic += EndStreak;
        Player.OnDeathStatic += SetScore;
        Player.OnDeathStatic += showLeaderBoards;
	}

    void OnEnemyKilled()
    {
        if(streakCount >= 5 && streakCount <=9)
        {
            score += 10;
        }else if (streakCount >= 10 && streakCount <= 25)
        {
            score += 20;
        }else if(streakCount >= 25 && streakCount <= 35)
        {
            score += 30;
        }else if(streakCount >= 35)
        {
            score += 40;
        }else
        {
            score += 5;
        }
        streakCount++;
        kills++;
    }

    void EndStreak()
    {
        streakCount = 0;
    }

    public void SetScore()
    {
        print("score set");

        scoreText = score.ToString();
        GameJolt.API.Scores.Add(score, scoreText);

    }

    void EndText()
    {
        //killStreakText.SetActive(false);
    }

    public static void showLeaderBoards()
    {
        print("This actually ran");
        GameJolt.UI.Manager.Instance.ShowLeaderboards();
    }

    // Update is called once per frame
    void Update() {
        text.text = "Score: " + score + "\n" + "Kills: " + kills + "\n" + "kill Streak: "+ streakCount;
        if (streakCount >= 5 && streakCount <= 9)
        {
            killStreakText0.SetActive(true);
        }
        else if (streakCount >= 10 && streakCount <= 25)
        {
            killStreakText1.SetActive(true);
            killStreakText0.SetActive(false);
        }
        else if (streakCount >= 25 && streakCount <= 35)
        {
            killStreakText2.SetActive(true);
            killStreakText1.SetActive(false);
        }
        else if (streakCount >= 35)
        {
            killStreakText3.SetActive(true);
            killStreakText2.SetActive(false);
        }
        else
        {
            killStreakText0.SetActive(false);
            killStreakText1.SetActive(false);
            killStreakText2.SetActive(false);
            killStreakText3.SetActive(false);
        }
    }
}
