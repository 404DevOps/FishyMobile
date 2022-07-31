using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class GameOverUIScript : MonoBehaviour
{
    public int Score { get; set; }
    public bool HasWon { get; set; }

    private Button btnRestart;
    private Label txtGameOver;
    private Label txtScore;

    // Start is called before the first frame update
    void OnEnable()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;
        btnRestart = root.Q<Button>("btnRestart");
        txtGameOver = root.Q<Label>("txtGameOver");
        txtScore = root.Q<Label>("txtScore");

        btnRestart.clicked += Restart_Clicked;
        txtGameOver.text = HasWon ? "You ate the whole Ocean!" : "Gulp. You died!";
        txtScore.text = "Score: " + Score;
    }

    private void Restart_Clicked()
    {
        GameManager.Instance.StartGame();
    }
}
