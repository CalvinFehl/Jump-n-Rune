using UnityEngine;
using System;

public class AudioManager : MonoBehaviour
{
    public static event Action OnPlayerIsMoving;
    public static event Action OnPlayerIsNotMoving;
    public static event Action OnSpiritIsMoving;
    public static event Action OnSpiritIsNotMoving;
    public static event Action OnSpiritDirected;

    [SerializeField] private AudioSource SpiritAudioSource;
    [SerializeField] private AudioSource PlayerAudioSource;
    [SerializeField] private AudioSource CameraAudioSource;
    [SerializeField] private AudioClip BackGroundAtmosphere;
    [SerializeField] private AudioClip[] FootstepSounds;
    [SerializeField] private AudioClip SpritWooshSound;
    [SerializeField] private AudioClip SpiritMovementSound;

    public static AudioManager Instance;

    bool isPlayerMoving = false;
    bool isSpiritMoving = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        CameraAudioSource.clip = BackGroundAtmosphere;
        CameraAudioSource.Play();
        CameraAudioSource.loop = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnEnable()
    {
        OnPlayerIsMoving += PlayFootstepSound;
        OnPlayerIsNotMoving += StopFootstepSound;
        OnSpiritIsMoving += PlaySpiritMovementSound;
        OnSpiritIsNotMoving += StopSpiritMovementSound;
        OnSpiritDirected += PlaySpiritWooshSound;
    }

    void OnDisable()
    {
        OnPlayerIsMoving -= PlayFootstepSound;
        OnPlayerIsNotMoving -= StopFootstepSound;
        OnSpiritIsMoving -= PlaySpiritMovementSound;
        OnSpiritIsNotMoving -= StopSpiritMovementSound;
        OnSpiritDirected -= PlaySpiritWooshSound;
    }

    void PlayFootstepSound()
    {
        isPlayerMoving = true;
        while (!PlayerAudioSource.isPlaying && isPlayerMoving)
        {
            PlayerAudioSource.clip = FootstepSounds[UnityEngine.Random.Range(0, FootstepSounds.Length)];
            PlayerAudioSource.Play();
        }
    }

    void StopFootstepSound()
    {
        isPlayerMoving = false;
    }

    void PlaySpiritMovementSound()
    {
        isSpiritMoving = true;
        while (!SpiritAudioSource.isPlaying && isSpiritMoving)
        {
            SpiritAudioSource.clip = SpiritMovementSound;
            SpiritAudioSource.Play();
        }
    }

    void StopSpiritMovementSound()
    {
        isSpiritMoving = false;
    }

    void PlaySpiritWooshSound()
    {
        SpiritAudioSource.PlayOneShot(SpritWooshSound);
    }
}
