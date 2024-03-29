using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ModalWindow : MonoBehaviour
{
    [SerializeField] private TMP_Text _message;
    [SerializeField] private Button _accept;
    [SerializeField] private TMP_Text _label;

    public void Initialize(ModalWindowModel modalWindowModel)
    {
        _message.text = modalWindowModel.Message;
        _accept.onClick.AddListener(LoadMenuScene);
        _label.text = modalWindowModel.Label;
        gameObject.SetActive(true);
    }

    private void LoadMenuScene()
    {
        SceneManager.LoadScene(GlobalStrings.Menu);
    }
}