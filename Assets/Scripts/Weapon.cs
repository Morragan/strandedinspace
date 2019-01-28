using UnityEngine;

public class Weapon : MonoBehaviour
{
    public float fireRate = 0;
    public int damage = 10;
    public LayerMask whatToHit;
    public Transform bulletTrailPrefab;
    public Transform muzzleFlashPrefab;
    public Transform hitPrefab;
    public float effectSpawnRate = 10;
    public float camShakeAmount = 0.04f;
    public float camShakeLength = 0.08f;
    [SerializeField]
    string weaponSound = "DefaultShot";

    float timeToFire = 0;
    float timeToSpawnEffect = 0;
    Transform firePoint;
    CameraShake camShake;
    AudioManager audioManager;

    private void Awake()
    {
        firePoint = transform.Find("FirePoint");
        if (firePoint == null) Debug.LogError("Fire point object not found!");
    }

    // Update is called once per frame
    void Update()
    {
        // singleburst weapon
        if (fireRate == 0)
        {
            if (Input.GetButtonDown("Fire1"))
            {
                Shoot();
            }
        }
        else
        {
            if (Input.GetButton("Fire1") && Time.time > timeToFire)
            {
                timeToFire = Time.time + 1 / (fireRate * PlayerStats.instance.weaponFireRateMultiplier);
                Shoot();
            }
        }
    }

    private void Start()
    {
        camShake = GameMaster.gm.GetComponent<CameraShake>();
        if (camShake == null)
            Debug.LogError("No Camera Shake script found on GM object");

        audioManager = AudioManager.instance;
        if (audioManager == null)
        {
            Debug.LogError("No Audio Manager found!");
        }
    }

    void Shoot()
    {
        Vector2 mousePos = new Vector2(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y);
        Vector2 firePointPos = new Vector2(firePoint.position.x, firePoint.position.y);
        RaycastHit2D hit = Physics2D.Raycast(firePointPos, mousePos - firePointPos, 100, whatToHit);

        //Debug.DrawLine(firePointPos, (mousePos - firePointPos) * 100, Color.cyan, Time.deltaTime, false);
        if (hit.collider != null)
        {
            Enemy enemy = hit.collider.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.DamageEnemy(Mathf.RoundToInt(damage * PlayerStats.instance.weaponDamageMultiplier));
            }
        }

        if (Time.time >= timeToSpawnEffect)
        {
            Vector3 hitPosition;
            Vector3 hitNormal;

            if (hit.collider == null)
            {
                hitPosition = (mousePos - firePointPos) * 30;
                hitNormal = new Vector3(9999, 9999, 9999);
            }
            else
            {
                hitPosition = hit.point;
                hitNormal = hit.normal;
            }
            Effect(hitPosition, hitNormal);
            timeToSpawnEffect = Time.time + 1 / effectSpawnRate;
        }
    }

    void Effect(Vector3 hitPosition, Vector3 hitNormal)
    {
        Transform trail = Instantiate(bulletTrailPrefab, firePoint.position, firePoint.rotation);
        LineRenderer lr = trail.GetComponent<LineRenderer>();
        if (lr != null)
        {
            lr.SetPosition(0, firePoint.position);
            lr.SetPosition(1, hitPosition);
        }
        Destroy(trail.gameObject, 0.04f);

        if(hitNormal != new Vector3(9999, 9999, 9999))
        {
            Transform hitParticle = Instantiate(hitPrefab, hitPosition, Quaternion.FromToRotation(Vector3.right, hitNormal));
            Destroy(hitParticle.gameObject, 1f);
        }

        Transform muzzleClone = Instantiate(muzzleFlashPrefab, firePoint.position, firePoint.rotation);
        muzzleClone.parent = firePoint;
        float size = Random.Range(0.6f, 0.9f);
        muzzleClone.localScale = new Vector3(size, size, size);
        Destroy(muzzleClone.gameObject, 0.02f);
        audioManager.PlaySound(weaponSound);
        camShake.Shake(camShakeAmount, camShakeLength);
    }
}
