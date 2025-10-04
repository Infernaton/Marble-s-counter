using UnityEngine;
using UnityEngine.UI;

public class Marble : MonoBehaviour
{
    [SerializeField] private Image m_BaseImage;
    [SerializeField] private Image[] m_Trims;

    public void SetBaseImage(Sprite baseImage)
    {
        m_BaseImage.sprite = baseImage;
    }

    public void SetTrims(Sprite[] trims)
    {
        for(int i = 0; i < Mathf.Min(trims.Length, m_Trims.Length); i++)
        {
            m_Trims[i].sprite = trims[i];
        }
    }
}
