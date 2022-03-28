using UnityEngine;
using UnityEngine.UI;

public enum UIs
{
    Authorization = 0,
    Character,
    Game,
}

public class UIManager : MonoBehaviour
{
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

    [SerializeField] private GameObject canvas;
    
    [Header("UIs")]
    [SerializeField] private GameObject activeUI;
    [SerializeField] private GameObject[] menuUIs;
    
    [Header("Input fields")]
    [SerializeField] public InputField loginField;
    [SerializeField] public InputField passwordField;
    
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
        GameManager.SetRoom("Room testing");
    }
    
    public void SetUI(int num)
    {
        activeUI.SetActive(false);
        if (num < 0 || num > menuUIs.Length) return;
        activeUI = menuUIs[num];
        activeUI.SetActive(true);
    }
}
