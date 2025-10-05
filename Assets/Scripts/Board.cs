using System.Collections;
using UnityEngine;
using Utils;

public class Board : MonoBehaviour
{
    [SerializeField] private int m_Lenght = 5;
    [SerializeField] private int m_Width = 5;
    [SerializeField] private Vector2 m_BoardFinalPosition;
    [SerializeField] private AnimationCurve m_Slide;

    [SerializeField] private AudioSource m_ShuffleBoard;

    private Vector2 _boardInitPos;
    public int Lenght { get => m_Lenght; }
    public int Width { get => m_Width; }

    public void PlayShuffleBoard() => SoundModifier.PlayAdjustPitch(m_ShuffleBoard);

    private void Awake()
    {
        _boardInitPos = transform.localPosition;
    }

    public IEnumerator SlideInAnimation()
    {
        yield return Anim.MoveUI(GetComponent<RectTransform>(), m_BoardFinalPosition, 0.2f, m_Slide);
    }

    public IEnumerator SlideOutAnimation()
    {
        yield return Anim.MoveUI(GetComponent<RectTransform>(), _boardInitPos, 0.2f, m_Slide);
    }
}
