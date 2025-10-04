using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Utils;

public class GameManager : MonoBehaviour
{
    [Header("Marble Generation")]
    [SerializeField] private Sprite[] m_MarbleTrims;
    [SerializeField] private Sprite[] m_MarbleBases;

    [SerializeField] private Marble m_MarblePrefab;

    [Header("UI")]
    [SerializeField] private Board m_Board;
    [SerializeField] private Canvas m_GoalFrame;
    [SerializeField] private TMP_Text m_Score;
    [SerializeField] private TMP_Text m_Timer;

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
        refMarble = GenerateMarble(new Vector2(0, 0), m_GoalFrame.transform);
        GenerateAllMarble();
    }

    private Marble GenerateMarble(Vector2 pos, Transform parent)
    {
        Marble marble = Instantiate(m_MarblePrefab, parent);
        marble.gameObject.transform.localPosition = pos;

        marble.SetBaseImage(m_MarbleBases[UnityEngine.Random.Range(0, m_MarbleBases.Length)]);
        marble.SetTrims(Utils.Random.Shuffle(m_MarbleTrims));

        return marble;
    }

    public void GenerateAllMarble()
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
    }

    public void ResetBoard()
    {
        for(int i = 0; i < m_Board.transform.childCount; i++ )
        {
            Destroy(m_Board.transform.GetChild(i).gameObject);
        }
        _marbleList = new();
        _indexCorrectMarble = new();
    }

    public void HandleMarbleClick(Marble clickedMarble)
    {
        int index = _marbleList.FindIndex(0, _marbleList.Count, (Marble m) => m.BaseImage.Equals(clickedMarble.BaseImage) && m.Trims.Equals(clickedMarble.Trims));

        if (_indexCorrectMarble.Contains(index))
        {
            print("++Correct");
            clickedMarble.PickUp();
            _indexCorrectMarble.Remove(index);
            Score++;
            if (_indexCorrectMarble.Count == 0)
            {
                print("none to find");
                ResetBoard();
                GenerateAllMarble();
            }
        }
        else
        {
            print("--Incorrect");
            clickedMarble.Wrong();
            if (Score > 0) Score--;
        }
    }
}
