using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Utils;

public enum GameState
{
    Menu,
    Play,
    EndGame
}

public class GameManager : MonoBehaviour
{
    [Header("Marble Generation")]
    [SerializeField] private Sprite[] m_MarbleTrims;
    [SerializeField] private Sprite[] m_MarbleBases;

    [SerializeField] private Marble m_MarblePrefab;

    [SerializeField] private float m_ChanceResetRefMarble;

    [Header("UI")]
    [SerializeField] private Board m_Board;
    [SerializeField] private Canvas m_GoalFrame;
    [SerializeField] private TMP_Text m_Score;

    private Marble refMarble;

    private List<Marble> _marbleList = new();
    private List<int> _indexCorrectMarble = new();
    private int score = 0;

    public int Score
    {
        get { return score; }
        private set
        {
            score = value;
            m_Score.text = string.Format("{0:000}", score);
        }
    }

    private GameState _currentState;

    public GameState State { get => _currentState; set => _currentState = value; }

    public static GameManager Instance = null;

    private void Awake()
    {
        if (Instance == null) // If there is no instance already
            Instance = this;
        else if (Instance != this)
            Destroy(gameObject);
    }

    void Start()
    {
        State = GameState.Menu;
    }

    private IEnumerator SetRefMarble()
    {
        if (refMarble != null)
            yield return refMarble.PickUp(true);
        refMarble = GenerateMarble(new Vector2(0, 0), m_GoalFrame.transform);
        refMarble.transform.localScale = Vector3.one * 0f;
        yield return Anim.PopIn(0.2f, refMarble.GetComponent<CanvasGroup>());
    }

    private Marble GenerateMarble(Vector2 pos, Transform parent)
    {
        Marble marble = Instantiate(m_MarblePrefab, parent);
        marble.gameObject.transform.localPosition = pos;

        marble.SetBaseImage(m_MarbleBases[UnityEngine.Random.Range(0, m_MarbleBases.Length)]);
        marble.SetTrims(Utils.Random.Shuffle(m_MarbleTrims));

        return marble;
    }

    public IEnumerator GenerateAllMarble()
    {
        int pxSize = 70;
        for(int l = 0; l < m_Board.Lenght; l++)
        {
            int lPos = l - m_Board.Lenght / 2;
            for(int w = 0; w < m_Board.Width; w++)
            {
                int wPos = w - m_Board.Width / 2;

                Marble m = GenerateMarble(new Vector2 (wPos * pxSize, lPos * pxSize), m_Board.transform);

                if (m.BaseImage.Equals(refMarble.BaseImage) && Compare.ScrambledEquals(refMarble.Trims, m.Trims))
                {
                    // Add the next index to be added to the list
                    _indexCorrectMarble.Add(_marbleList.Count);
                }
                _marbleList.Add(m);
            }
        }

        if (_indexCorrectMarble.Count < 1)
            yield return GenerateAllMarble();

        m_Board.PlayShuffleBoard();

        yield return m_Board.SlideInAnimation();

        Timer.Instance.StartTimer();
    }

    public IEnumerator ResetBoard()
    {
        _marbleList = new();
        _indexCorrectMarble = new();
        Timer.Instance.StopTimer();
        Timer.Instance.ResetTimer();
        yield return m_Board.SlideOutAnimation();

        for (int i = 0; i < m_Board.transform.childCount; i++)
        {
            Destroy(m_Board.transform.GetChild(i).gameObject);
        }

        // Change to reset SetRefMarble()
        if (UnityEngine.Random.value >= m_ChanceResetRefMarble)
            yield return SetRefMarble();
    }

    public void HandleMarbleClick(Marble clickedMarble)
    {
        int index = _marbleList.FindIndex(0, _marbleList.Count, (Marble m) => m.BaseImage.Equals(clickedMarble.BaseImage) && m.Trims.Equals(clickedMarble.Trims));

        if (_indexCorrectMarble.Contains(index))
        {
            StartCoroutine(clickedMarble.PickUp(false));
            _indexCorrectMarble.Remove(index);
            Score++;
            if (_indexCorrectMarble.Count == 0)
            {
                StartCoroutine(GameLoop());
            }
        }
        else
        {
            clickedMarble.Wrong();
            if (Score > 0) Score--;
        }
    }

    public IEnumerator InitGameLoop()
    {
        State = GameState.Play;
        yield return MenuManager.Instance.ResetDisplayMenu();
        yield return SetRefMarble();
        yield return GenerateAllMarble();
    }

    private IEnumerator GameLoop()
    {
        //Time to make the animation of the last marble to disapear
        yield return new WaitForSeconds(0.1f);
        yield return ResetBoard();
        yield return GenerateAllMarble();
    }

    public void GameOver()
    {
        State = GameState.EndGame;
        MenuManager.Instance.SetGameoverMenu();
    }
}
