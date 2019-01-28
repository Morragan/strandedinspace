using UnityEngine;
using UnityEngine.UI;

public class WaveUI : MonoBehaviour
{
    [SerializeField]
    WaveSpawner spawner;
    [SerializeField]
    Animator waveAnimator;
    [SerializeField]
    Text waveCountdownText;
    [SerializeField]
    Text waveCountText;

    private WaveSpawner.SpawnState previousState;

    private void Start()
    {
        if (spawner == null)
        {
            Debug.LogError("No spawner referenced");
            enabled = false;
        }
        if (waveAnimator == null)
        {
            Debug.LogError("No Wave Animator referenced");
            enabled = false;
        }
        if (waveCountdownText == null)
        {
            Debug.LogError("No Wave Countdown Text referenced");
            enabled = false;
        }
        if (waveCountText == null)
        {
            Debug.LogError("No Wave Count Text referenced");
            enabled = false;
        }
    }

    private void Update()
    {
        switch (spawner.State)
        {
            case WaveSpawner.SpawnState.Counting:
                UpdateCountingUI();
                break;
            case WaveSpawner.SpawnState.Spawning:
                UpdateSpawningUI();
                break;
        }
        previousState = spawner.State;
    }

    void UpdateCountingUI()
    {
        if (previousState != WaveSpawner.SpawnState.Counting)
        {
            waveAnimator.SetBool("WaveIncoming", false);
            waveAnimator.SetBool("WaveCountdown", true);
        }
        waveCountdownText.text = ((int)spawner.WaveCountdown + 1).ToString();
    }

    void UpdateSpawningUI()
    {
        if (previousState != WaveSpawner.SpawnState.Spawning)
        {
            waveAnimator.SetBool("WaveCountdown", false);
            waveAnimator.SetBool("WaveIncoming", true);

            waveCountText.text = (spawner.NextWave + 1).ToString();
        }
    }
}
