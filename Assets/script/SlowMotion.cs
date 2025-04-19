using UnityEngine;

public class CommandSlowMotion : MonoBehaviour
{
    public float slowTimeScale = 0.2f;
    public float maxSlowDuration = 3f; // 최대 지속 시간 (초)

    private float defaultTimeScale = 1f;
    private bool isSlowMotion = false;
    private float slowTimer = 0f;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            ToggleSlowMotion();
        }

        if (isSlowMotion)
        {
            slowTimer += Time.unscaledDeltaTime;

            if (slowTimer >= maxSlowDuration)
            {
                ResetTime();
            }
        }
    }

    void ToggleSlowMotion()
    {
        if (!isSlowMotion)
        {
            isSlowMotion = true;
            Time.timeScale = slowTimeScale;
            Time.fixedDeltaTime = 0.02f * Time.timeScale;
            slowTimer = 0f;
        }
        else
        {
            ResetTime();
        }
    }

    void ResetTime()
    {
        isSlowMotion = false;
        Time.timeScale = defaultTimeScale;
        Time.fixedDeltaTime = 0.02f;
        slowTimer = 0f;
    }
}
