using UnityEngine;

public class SFXManager : MonoBehaviour
{
    public static SFXManager Instance { get; private set; }

    private AudioSource audioSource;

    public AudioClip playerAttackClip; // ���� ����
    public AudioClip jumpSound; // ���� ����
    public AudioClip runSound; // �޸��� ����


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject); // �ߺ� ����
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
