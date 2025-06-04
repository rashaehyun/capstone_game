using UnityEngine;

public class SFXManager : MonoBehaviour
{
    public static SFXManager Instance { get; private set; }

    private AudioSource audioSource;

    public AudioClip playerAttackClip; // 공격 사운드
    public AudioClip jumpSound; // 점프 사운드
    public AudioClip runSound; // 달리기 사운드


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject); // 중복 방지
        }
    }

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.volume = 0.5f;
    }

    public void PlaySound(AudioClip clip)
    {
        audioSource.PlayOneShot(clip);
    }

    public void PlayAttackSound()
    {
        PlaySound(playerAttackClip);
    }
    public void PlayJumpSound()
    {
        PlaySound(jumpSound);
    }
}
