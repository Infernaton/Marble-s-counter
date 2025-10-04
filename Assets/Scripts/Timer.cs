using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class Timer : MonoBehaviour
{
    [SerializeField] private float m_InitTimer = 10;
    [SerializeField] private float m_TimerMini = 3;
    [SerializeField] private TMP_Text m_TextTimer;

    [SerializeField] private AnimationCurve m_TimeResetCurve;

    [SerializeField] private UnityEvent m_InvokeTimerOut;

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
            var ms = (int)((_currentTime - t0) * 100);
            m_TextTimer.text = string.Format("{0:00}:{1:000}", t0, ms);
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

        CurrentTime -= Time.deltaTime;

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
