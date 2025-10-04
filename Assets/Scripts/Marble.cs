using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Marble : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private Image m_BaseImage;
    [SerializeField] private Image[] m_Trims;

    public List<Sprite> Trims { get; private set; }

    public Sprite BaseImage { get; private set; }

    public void OnPointerClick(PointerEventData eventData)
    {
        print("I was clicked");
        GameManager.Instance.HandleMarbleClick(this);
    }

    public void SetBaseImage(Sprite baseImage)
    {
        BaseImage = baseImage;
        m_BaseImage.sprite = baseImage;
    }

    public void SetTrims(Sprite[] trims)
    {
        for(int i = 0; i < Mathf.Min(trims.Length, m_Trims.Length); i++)
        {
            m_Trims[i].sprite = trims[i];
            Trims.Add(trims[i]);
        }
    }

    private void Awake()
    {
        Trims = new();
    }
}
