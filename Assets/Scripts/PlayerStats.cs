using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public static PlayerStats instance;

    public int regenRate = 2;
    public int maxHealth = 100;
    public float movementSpeed = 4;
    public float weaponDamageMultiplier = 1f;
    public float weaponFireRateMultiplier = 1f;

    private int _curHealth;
    public int curHealth
    {
        get { return _curHealth; }
        set { _curHealth = Mathf.Clamp(value, 0, maxHealth); }
    }

    void Awake()
    {
        if (instance == null) instance = this;
    }
}
