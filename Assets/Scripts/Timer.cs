using TMPro;
using UnityEngine;
using UnityEngine.Events;
using Utils;

public class Timer : MonoBehaviour
{
    [SerializeField] private float m_InitTimer = 10;
    [SerializeField] private float m_TimerMini = 3;
    [SerializeField] private TMP_Text m_TextTimer;

    [SerializeField] private AnimationCurve m_TimeResetCurve;

    [SerializeField] private UnityEvent m_InvokeTimerOut;

    [Header("Animation")]
    [SerializeField] private Color m_EndTimerColor;
    [SerializeField] private AnimationCurve m_AnimationDisplayColor;
    private float cdAnimation;

    private float _currentTime;
    private bool _isPlaying;


    public float CurrentTime
    {
        get => _currentTime;
        set
        {
            _currentTime = value;

            // get the total full seconds.
            var t0 = (int)_currentTime;

            // get the 2 most significant values of the milliseconds.
            var ms = (int)((_currentTime - t0) * 1000);
            m_TextTimer.text = string.Format("{0:00}:{1:000}", t0, ms);

            if (_currentTime <= 4 && cdAnimation <= 0)
            {
                StartCoroutine(Anim.ChangeColor(0.75f, m_TextTimer, m_EndTimerColor, m_AnimationDisplayColor));
                StartCoroutine(Anim.Giggle(0.2f, m_TextTimer.transform, 1f, 2f));
                cdAnimation = 1f;
                //Make sound
            }
        }
    }

    public static Timer Instance = null;

    private void Awake()
    {
        if (Instance == null) // If there is no instance already
            Instance = this;
        else if (Instance != this)
            Destroy(gameObject);
    }

    private void Start()
    {
        ResetTimer();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!_isPlaying) return;

        CurrentTime -= Time.fixedDeltaTime;
        cdAnimation -= Time.fixedDeltaTime;

        if (CurrentTime <= 0)
        {
            StopTimer();
            m_InvokeTimerOut.Invoke();
        }
    }

    public void StartTimer()
    {
        _isPlaying = true;
    }

    public void StopTimer()
    {
        _isPlaying = false;
    }

    public void ResetTimer()
    {
        CurrentTime = Mathf.Lerp(m_InitTimer, m_TimerMini, m_TimeResetCurve.Evaluate(GameManager.Instance.Score / 100f));
    }
}
