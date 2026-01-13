using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;



public class mainmenu : MonoBehaviour
{
    [SerializeField] public string PrototypeScene = "PrototypeScene";

    public void OnClick()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;

        var PlayBtn = root.Q<Button>("PlayButton");
        var ExitBtn = root.Q<Button>("ExitButton");

        PlayBtn.clicked += PlayGame;
        ExitBtn.clicked += ExitGame;
    }

    public void PlayGame()
    {
        SceneManager.LoadScene("PrototypeScene");
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}