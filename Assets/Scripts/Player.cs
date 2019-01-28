using UnityEngine;

[RequireComponent(typeof(Platformer2DUserControl))]
public class Player : MonoBehaviour
{
    public int fallBoundary = -20;
    public string deathSoundName = "PlayerDeath";
    public string damageSoundName = "Grunt";

    [SerializeField]
    private StatusIndicator statusIndicator;

    private AudioManager audioManager;
    private PlayerStats stats;

    void Start()
    {
        stats = PlayerStats.instance;
        if (statusIndicator == null)
        {
            Debug.LogError("No status indicator referenced on Player");
        }
        else
        {
            statusIndicator.SetHealth(stats.curHealth, stats.maxHealth);
        }

        audioManager = AudioManager.instance;
        if (audioManager == null)
        {
            Debug.LogError("No Audio Manager found!");
        }

        // Subscribe pause method
        GameMaster.gm.onGamePause += OnGamePauseToggle;
        GameMaster.gm.onToggleUpgradeMenu += OnGamePauseToggle;
        stats.curHealth = stats.maxHealth;

        InvokeRepeating("RegenHealth", 1f/stats.regenRate, 1f/stats.regenRate);
    }

    void Update()
    {
        if (transform.position.y <= fallBoundary)
            DamagePlayer(9999999);
    }

    public void DamagePlayer(int damage)
    {
        stats.curHealth -= damage;
        if (stats.curHealth <= 0)
        {
            audioManager.PlaySound(deathSoundName);
            GameMaster.KillPlayer(this);
        }
        else
        {
            audioManager.PlaySound(damageSoundName);
        }

        statusIndicator.SetHealth(stats.curHealth, stats.maxHealth);
    }

    void RegenHealth()
    {
        stats.curHealth++;
        statusIndicator.SetHealth(stats.curHealth, stats.maxHealth);
    }

    void OnGamePauseToggle(bool active)
    {
        GetComponent<Platformer2DUserControl>().enabled = !active;
		Weapon _weapon = GetComponentInChildren<Weapon>();
		if (_weapon != null)
			_weapon.enabled = !active;
        ArmRotation _armRotation = GetComponentInChildren<ArmRotation>();
        if(_armRotation != null)
            _armRotation.enabled = !active;
    }

    private void OnDestroy()
    {
        GameMaster.gm.onGamePause -= OnGamePauseToggle;
        GameMaster.gm.onToggleUpgradeMenu -= OnGamePauseToggle;
    }
}
