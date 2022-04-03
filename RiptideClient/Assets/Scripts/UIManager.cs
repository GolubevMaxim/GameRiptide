using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum UIs
{
    Authorization = 0,
    Character,
    Game,
}

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject canvas;
    
    [Header("UIs")]
    [SerializeField] private GameObject activeUI;
    [SerializeField] private GameObject[] menuUIs;
    
    [Header("Input fields")]
    [SerializeField] private InputField loginField;
    [SerializeField] private InputField passwordField;

    public string Login => loginField.text;
    public string Password => passwordField.text;
    
    private static UIManager _singleton;

    public static UIManager Singleton
    {
        get => _singleton;
        private set
        {
            if (_singleton == null)
                _singleton = value;
            else if (_singleton != value)
            {
                Debug.Log($"{nameof(UIManager)} instance already exists, destroying duplicate.");
                Destroy(value);
            }
        }
    }

    private void Awake()
    {
        Singleton = this;
    }

    private void Start()
    {
        DontDestroyOnLoad(canvas);
        
        foreach (var ui in menuUIs){
            ui.SetActive(false);
        }
        
        activeUI.SetActive(true);

        loginField.text = "user0";
        passwordField.text = "123456789";
    }

    public void ConnectClicked()
    {
        SetUI((int) UIs.Character);
        NetworkManager.Singleton.Connect();
    }

    public void EnterGameClicked()
    {
        SceneManager.LoadScene("Room testing");
    }
    
    public void SetUI(int num)
    {
        activeUI.SetActive(false);
    
        if (num < 0 || num > menuUIs.Length) 
            throw new ArgumentOutOfRangeException(nameof(num));
        
        activeUI = menuUIs[num];
        activeUI.SetActive(true);
    }
}
