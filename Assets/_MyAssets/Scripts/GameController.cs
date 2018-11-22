using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    private static bool playing;
    private const string SCENEID = "sceneId";
    private const string XPOS = "xPos";
    private const string YPOS = "yPos";
    private const string SCORE = "savedScore";

    public static void StoreAll(Vector2 position, int score)
	{
        StoreScene();
        StorePosition(position);
        StoreScore(score);
    }

    public static void StoreScene()
	{
        PlayerPrefs.SetInt(
            SCENEID, SceneManager.GetActiveScene().buildIndex);

        PlayerPrefs.Save();
    }

    public static void StorePosition(Vector2 position)
	{
        PlayerPrefs.SetFloat(XPOS, position.x);
        PlayerPrefs.SetFloat(YPOS, position.y);

        PlayerPrefs.Save();
    }

    public static void StoreScore(int score)
	{
        PlayerPrefs.SetInt(SCORE, score);

        PlayerPrefs.Save();
    }

    public static void ClearData()
    {
        PlayerPrefs.DeleteAll();
    }

    public static Vector2 GetPosition()
	{
        Vector2 position = new Vector2(
            GetSavedX(), GetSavedY());
        
        return position;
    }

    public static void LoadSavedScene()
    {
        int savedScene = GetSavedScene();

        if (savedScene != -1)
        {
            SceneManager.LoadScene(savedScene);
        }
        else print("No saved scenes available");
    }

    public static void LoadNextScene()
    {
        SceneManager.LoadScene(
            SceneManager.GetActiveScene().buildIndex + 1);
    }

    public static bool IsPlaying()
    {
        return playing;
    }

    public static void SetPlay(bool isPlaying)
    {
        playing = isPlaying;
    }

    public static int GetSavedScene()
    {
        int savedScene = -1;

        if (PlayerPrefs.HasKey(SCENEID))
        {
            savedScene = PlayerPrefs.GetInt(SCENEID);
        }
        return savedScene;
    }

    public static float GetSavedX()
    {
        float savedX = 0;

        if (PlayerPrefs.HasKey(XPOS))
        {
            savedX = PlayerPrefs.GetFloat(XPOS);
        }
        return savedX;
    }

    public static float GetSavedY()
    {
        float savedY = 0;

        if (PlayerPrefs.HasKey(YPOS))
        {
            savedY = PlayerPrefs.GetFloat(YPOS);
        }
        return savedY;
    }

    public static int GetSavedScore()
    {
        int savedScore = 0;

        if (PlayerPrefs.HasKey(SCORE))
        {
            savedScore = PlayerPrefs.GetInt(SCORE);
        }
        return savedScore;
    }
}