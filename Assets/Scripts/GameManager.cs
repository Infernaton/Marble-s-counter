using System.Collections.Generic;
using UnityEngine;
using Utils;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Board m_Board;

    [SerializeField] private Sprite[] m_MarbleTrims;
    [SerializeField] private Sprite[] m_MarbleBases;

    [SerializeField] private Marble m_MarblePrefab;

    public Marble refMarble;

    private List<Marble> _marbleList = new();
    private List<int> _indexCorrectMarble = new();

    public static GameManager Instance = null;

    private void Awake()
    {
        if (Instance == null) // If there is no instance already
            Instance = this;
        else if (Instance != this)
            Destroy(gameObject);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        refMarble = GenerateMarble(new Vector2(0, 0), transform);
        GenerateAllMarble();
    }

    private Marble GenerateMarble(Vector2 pos, Transform parent)
    {
        Marble marble = Instantiate(m_MarblePrefab, parent);
        marble.gameObject.transform.localPosition = pos;

        marble.SetBaseImage(m_MarbleBases[UnityEngine.Random.Range(0, m_MarbleBases.Length - 1)]);
        marble.SetTrims(Utils.Random.Shuffle(m_MarbleTrims));

        return marble;
    }

    public void GenerateAllMarble()
    {
        int pxSize = 74;
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

    public void HandleMarbleClick(Marble clickedMarble)
    {
        int index = _marbleList.FindIndex(0, _marbleList.Count-1, (Marble m) => m.BaseImage.Equals(clickedMarble.BaseImage) && m.Trims.Equals(clickedMarble.Trims));

        if (_indexCorrectMarble.Contains(index)) 
        { 
            print("Correct");
            Destroy(clickedMarble.gameObject);
            _indexCorrectMarble.Remove(index);
            if (_indexCorrectMarble.Count == 0) print("none to find");
        }
    }
}
