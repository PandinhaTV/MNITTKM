using UnityEngine;
using TMPro;
public class rifle : Interactable
{
   public Transform PlayerTransform;
    
    public GameObject Rifle;
    public float drop = 2f;
    
    public float fireRate = 0.2f;
    private float nextFireTime = 1f;
    private PlayerUI playerUI;
    public Transform muzzlePoint;
    public GameObject bulletHolePrefab;
    public float range = 100f;
    public float damage = 25f;
    
    public float moveSpeed = 30f; // Speed of movement
    public float rotateSpeed = 30f; // Speed of rotation

    public AudioSource gunSound;
    [SerializeField]
    private TextMeshProUGUI Ammotext;
    public int ammo = 7;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Rifle.GetComponent<Rigidbody>().isKinematic = false;
        gunSound = GetComponentInChildren<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
       
        if (transform.IsChildOf(PlayerTransform))
        {
           Ammotext.text = "Ammo = " + ammo;
           if (Input.GetButton("Fire1") && Time.time >= nextFireTime && (ammo > 0))
           {
               ammo -= 1;
               nextFireTime = Time.time + fireRate;
               Shoot();
           }
           if (Input.GetKeyDown("q") )
           {
               UnequipObject();
           } 
        }
            
    }
    protected override void Interact()
    {
        Debug.Log("Interact with" + gameObject.name);

        EquipObject();
    }
    public void Commence (float amount)
    {
        drop -= amount;
        if (drop<= 0f)
        {
            Go();
        }
    }

    void Go ()
    {
        Destroy(gameObject);
    }
    void UnequipObject()
    {
        
        PlayerTransform.DetachChildren();
        Rifle.transform.eulerAngles = new Vector3(Rifle.transform.eulerAngles.x, Rifle.transform.eulerAngles.y, Rifle.transform.eulerAngles.z - 45);
        Rifle.GetComponent<Rigidbody>().isKinematic = false;
    }
    
    void EquipObject()
    {
        if (PlayerTransform.childCount == 0 )
        {
          
          Rifle.GetComponent<Rigidbody>().isKinematic = true;
          Rifle.transform.position = PlayerTransform.transform.position;
          Rifle.transform.rotation = PlayerTransform.transform.rotation;
          Rifle.transform.SetParent(PlayerTransform);  
        }
        
    }

    void Shoot()
    {
        //if (muzzleFlash != null) muzzleFlash.Play(); // Play muzzle flash effect
        if (gunSound != null)
        {
            Debug.Log("Playing gun sound!");
            gunSound.pitch = Random.Range(0.95f, 1.05f);
            gunSound.volume = Random.Range(0.9f, 1f);
            gunSound.Play();
        }
        else
        {
            Debug.LogWarning("Gun sound AudioSource is NOT assigned!");
        } // Play shooting sound

        RaycastHit hit;
        if (Physics.Raycast(muzzlePoint.position, -muzzlePoint.forward, out hit, range))
        {
            Debug.Log("Hit " + hit.collider.name);
            
            EnemyHitbox hitbox = hit.collider.GetComponent<EnemyHitbox>();
            if (hitbox != null)
            {
                hitbox.RegisterHit(25f, hit.point, hit.normal); // Deal 25 damage
            }
            Rigidbody rb = hit.collider.GetComponent<Rigidbody>();
            if (rb != null)
            {
                Vector3 forceDirection = hit.normal * -1; // Opposite of the surface normal
                float forceAmount = 5f; // Adjust this value for more or less impact

                rb.AddForceAtPosition(forceDirection * forceAmount, hit.point, ForceMode.Impulse);
            }
            CreateBulletHole(hit.point, hit.normal, hit.collider.transform);
            
        }
    }
    void CreateBulletHole(Vector3 position, Vector3 normal, Transform parent)
    {
        // Instantiate the bullet hole at the hit point
        GameObject bulletHole = Instantiate(bulletHolePrefab, position + normal * 0.01f, Quaternion.LookRotation(normal));
        bulletHole.transform.rotation = Quaternion.LookRotation(-normal);
        bulletHole.transform.Rotate(0, 0, Random.Range(0f, 360f)); // Random rotation for realism
        bulletHole.transform.SetParent(parent); // Stick to the object
        // Destroy the bullet hole after 10 seconds to save performance
        Destroy(bulletHole, 10f);
    }
}
