using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Utils;
using static Unity.VisualScripting.Member;

public class Marble : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private Image m_BaseImage;
    [SerializeField] private Image[] m_Trims;

    [SerializeField] private float m_GiggleAnimationTime = 0.1f;

    [Header("Audio")]
    [SerializeField] private AudioSource m_Source;

    [SerializeField] private AudioClip m_WrongChoice;
    [SerializeField] private AudioClip m_PickupMarble;
    [SerializeField] private AudioClip m_TouchMarble;

    [SerializeField] private AudioClip m_ResetTarget;

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
        AudioClip clip;
        if (isRefMarble)
            clip = m_ResetTarget;
        else
            clip = m_TouchMarble;
            
        SoundModifier.PlayOneShotAdjustPitch(m_Source, clip);
        yield return new WaitForSeconds(clip.length);
        yield return Anim.PopOut(0.1f, GetComponent<CanvasGroup>());

        Destroy(gameObject);
    }

    public void Wrong()
    {
        SoundModifier.PlayOneShotAdjustPitch(m_Source, m_WrongChoice);
    }
}
