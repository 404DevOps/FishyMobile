using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class StartGameUIScript : MonoBehaviour
{
    private Button btnStart;

    // Start is called before the first frame update
    void OnEnable()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;
        btnStart = root.Q<Button>("btnStart");
        btnStart.clicked += Start_Clicked;
    }

    private void Start_Clicked()
    {
        GameManager.Instance.StartGame();
        this.gameObject.SetActive(false);
    }
}
