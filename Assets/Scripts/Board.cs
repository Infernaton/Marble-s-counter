using UnityEngine;

public class Board : MonoBehaviour
{
    [SerializeField] private int m_Lenght = 5;
    [SerializeField] private int m_Width = 5;
    public int Lenght { get { return m_Lenght; } }
    public int Width { get { return m_Width; } }
}
