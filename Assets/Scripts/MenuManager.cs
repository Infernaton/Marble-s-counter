using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Utils;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private CanvasGroup m_Container;
    [SerializeField] private TMP_Text m_GameOverText;
    [SerializeField] private Image m_TitleScreenText;

    public static MenuManager Instance = null;

    private void Awake()
    {
        if (Instance == null) // If there is no instance already
            Instance = this;
        else if (Instance != this)
            Destroy(gameObject);
    }

    private void Start()
    {
        SetMainMenu();
    }

    // Update is called once per frame
    void Update()
    {
        if (!m_Container.gameObject.activeSelf || GameManager.Instance.State == GameState.Play)
            return;

        if (Input.GetMouseButtonDown(0))
        {
            if (GameManager.Instance.State == GameState.Menu)
            {
                StartCoroutine(GameManager.Instance.InitGameLoop());
            } else
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    public void SetMainMenu()
    {
        StartCoroutine(Anim.FadeIn(0.1f, m_Container));
        m_TitleScreenText.gameObject.SetActive(true);
    }

    public void SetGameoverMenu()
    {
        StartCoroutine(Anim.FadeIn(0.15f, m_Container));
        m_GameOverText.gameObject.SetActive(true);
    }

    public IEnumerator ResetDisplayMenu()
    {
        yield return Anim.FadeOut(0.15f, m_Container);
        m_TitleScreenText.gameObject.SetActive(false);
        m_GameOverText.gameObject.SetActive(false);
    }
}
