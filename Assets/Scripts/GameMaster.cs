using UnityEngine;
using System.Collections;

public class GameMaster : MonoBehaviour
{

    public static GameMaster gm;

    public static int RemainingLives { get; private set; } = 3;

    void Awake()
    {
        if (gm == null)
        {
            gm = GameObject.FindGameObjectWithTag("GM").GetComponent<GameMaster>();
        }
    }

    public Transform playerPrefab;
    public Transform spawnPoint;
    public float spawnDelay = 2;
    public Transform spawnPrefab;
    public ParticleSystem moneyRainPrefab;
    public string respawnCountdownSoundName = "RespawnCountdown";
    public string spawnSoundName = "Spawn";
    public string gameOverSoundName = "GameOver";
    public CameraShake cameraShake;
    public delegate void UpgradeMenuCallback(bool active);
    public delegate void PauseCallback(bool active);
    public UpgradeMenuCallback onToggleUpgradeMenu;
    public PauseCallback onGamePause;
    public static int Money;
    public Transform Camera;

    [SerializeField] GameObject gameOverUI;
    [SerializeField] int maxLives = 3;
    [SerializeField] GameObject upgradeMenu;
    [SerializeField] GameObject gamePauseUI;
    [SerializeField] private int startingMoney = 100;
    [SerializeField] WaveSpawner waveSpawner;
    [SerializeField] int moneyGoal = 100;

    private AudioManager audioManager;
    private float timeToEndGod = 0;
    private float time = 0F;
    private float timeToTakeMoney = 1F/33.33333F;
    private bool isGodTime = false;
    private WaitForSeconds waitForSeconds = new WaitForSeconds(1F / 30F);
    private ParticleSystem moneyRain;

    void Start()
    {
        if (cameraShake == null)
        {
            Debug.LogError("No camera shake referenced in GameMaster");
        }
        RemainingLives = maxLives;

        if (upgradeMenu == null) 
        {
            Debug.LogError("No upgrade menu referenced in GameMaster");
        }

        if (gamePauseUI == null)
        {
            Debug.LogError("No pause UI referenced in GameMaster");
        }

        audioManager = AudioManager.instance;
        if (audioManager == null) Debug.LogError("Game Master: No Audio Manager found in the scene!");

        Money = startingMoney;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.U))
        {
            ToggleUpgradeMenu();
        }
        else if (Input.GetKeyDown(KeyCode.P))
        {
            ToggleGamePause();
        }
        // TODO: DarkkMane
        if (Money >= moneyGoal && timeToEndGod < Time.time)
            StartDarkkEvent();
        time += Time.deltaTime;
        if(isGodTime && Money > 0 && timeToTakeMoney < time)
        {
            Money--;
            time = 0;
            if (Money == 0)
                EndDarkkEvent();
        }
    }

    private void StartDarkkEvent()
    {
        timeToEndGod = Time.time + 100000F;
        isGodTime = true;
        audioManager.PlaySound("DARKKMANE");
        // pause
        onGamePause.Invoke(true);
        waveSpawner.enabled = false;
        DarkkGod.Instance.Show();
        
        moneyRain = Instantiate(moneyRainPrefab, new Vector3(Camera.transform.position.x - 6.2F, Camera.transform.position.y + 3.05F, Camera.transform.position.z + 0F), Quaternion.Euler(60, 90, -90), Camera);
        moneyRain.transform.parent = Camera.transform;
    }

    private void EndDarkkEvent()
    {
        isGodTime = false;
        onGamePause.Invoke(false);
        waveSpawner.enabled = true;
        DarkkGod.Instance.Hide();
        Destroy(moneyRain.gameObject);
    }

    private void ToggleUpgradeMenu()
    {
        upgradeMenu.SetActive(!upgradeMenu.activeSelf);
        waveSpawner.enabled = !upgradeMenu.activeSelf;
        onToggleUpgradeMenu.Invoke(upgradeMenu.activeSelf);
    }

    private void ToggleGamePause()
    {
        gamePauseUI.SetActive(!gamePauseUI.activeSelf);
        waveSpawner.enabled = !gamePauseUI.activeSelf;
        onGamePause.Invoke(gamePauseUI.activeSelf);
    }

    public IEnumerator _RespawnPlayer()
    {
        audioManager.PlaySound(respawnCountdownSoundName);
        yield return new WaitForSeconds(spawnDelay);
        audioManager.PlaySound(spawnSoundName);
        Instantiate(playerPrefab, spawnPoint.position, spawnPoint.rotation);
        Transform clone = Instantiate(spawnPrefab, spawnPoint.position, spawnPoint.rotation);
        Destroy(clone.gameObject, 3f);
    }

    public void EndGame()
    {
        audioManager.PlaySound(gameOverSoundName);
        gameOverUI.SetActive(true);
    }

    public static void KillPlayer(Player player)
    {
        Destroy(player.gameObject);
        RemainingLives--;
        if(RemainingLives <= 0)
        {
            gm.EndGame();
        }
        else
        {
            gm.StartCoroutine(gm._RespawnPlayer());
        }
    }

    public static void KillEnemy(Enemy enemy)
    {
        gm._KillEnemy(enemy);
        
    }
    public void _KillEnemy(Enemy _enemy)
    {
        Transform _clone = Instantiate(_enemy.deathParticles, _enemy.transform.position, Quaternion.identity);
        Destroy(_clone.gameObject, 5f);
        cameraShake.Shake(_enemy.shakeAmt, _enemy.shakeLength);
        Money += _enemy.moneyDrop;
        audioManager.PlaySound(_enemy.deathSoundName);
        Destroy(_enemy.gameObject);
    }

}
