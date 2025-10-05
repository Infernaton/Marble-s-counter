using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Utils;

public class Marble : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private Image m_BaseImage;
    [SerializeField] private Image[] m_Trims;

    [SerializeField] private float m_GiggleAnimationTime = 0.1f;

    private float _currentCDAnimation;

    public List<Sprite> Trims { get; private set; }

    public Sprite BaseImage { get; private set; }

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

    private void Update()
    {
        if(_currentCDAnimation > 0)
        {
            _currentCDAnimation -= Time.deltaTime;
        }
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if (_currentCDAnimation > 0) return;

        StartCoroutine(Anim.Giggle(m_GiggleAnimationTime, transform, 1f, 2f));
        _currentCDAnimation = m_GiggleAnimationTime;
        GameManager.Instance.HandleMarbleClick(this);
    }

    public IEnumerator PickUp(bool isRefMarble)
    {
        //Make sound
        yield return Anim.PopOut(0.1f, GetComponent<CanvasGroup>());
        //if (isRefMarble) // Make reset sound
        //else // make pickup sound
        Destroy(gameObject);
    }

    public void Wrong()
    {
        //Make sound
    }
}
