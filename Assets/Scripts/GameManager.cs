using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Board m_Board;

    [SerializeField] private Sprite[] m_MarbleTrims;
    [SerializeField] private Sprite[] m_MarbleBases;

    [SerializeField] private Marble m_MarblePrefab;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GenerateAllMarble();
    }

    private void GenerateMarble(Vector2 pos)
    {
        Marble marble = Instantiate(m_MarblePrefab, pos, Quaternion.identity, m_Board.transform);
        int index = Random.Range(0, m_MarbleBases.Length-1);
        print(index);
        print(m_MarbleBases.Length);
        marble.SetBaseImage(m_MarbleBases[index]);

        marble.SetTrims(Utils.Random.Shuffle(m_MarbleTrims));
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

                GenerateMarble(new Vector2 (wPos * pxSize, lPos * pxSize));
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
