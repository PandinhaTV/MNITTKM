using UnityEngine;
using TMPro;
public class Pistol : Interactable
{
    public Transform PlayerTransform;
    
    public GameObject Gun;
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

   
    [SerializeField]
    private TextMeshProUGUI Ammotext;
    public int ammo = 7;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Gun.GetComponent<Rigidbody>().isKinematic = false;
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
        Gun.transform.eulerAngles = new Vector3(Gun.transform.eulerAngles.x, Gun.transform.eulerAngles.y, Gun.transform.eulerAngles.z - 45);
        Gun.GetComponent<Rigidbody>().isKinematic = false;
    }
    
    void EquipObject()
    {
        if (PlayerTransform.childCount == 0 )
        {
          Gun.GetComponent<Rigidbody>().isKinematic = true;
          Gun.transform.position = PlayerTransform.transform.position;
          Gun.transform.rotation = PlayerTransform.transform.rotation;
          Gun.transform.SetParent(PlayerTransform);  
        }
        
    }

    void Shoot()
    {
        //if (muzzleFlash != null) muzzleFlash.Play(); // Play muzzle flash effect
        //if (gunSound != null) gunSound.Play(); // Play shooting sound

        RaycastHit hit;
        if (Physics.Raycast(muzzlePoint.position, -muzzlePoint.forward, out hit, range))
        { 
            if (hit.collider.CompareTag("Enemy"))
            {
                Debug.Log("Hit " + hit.collider.name);
            }
            else CreateBulletHole(hit.point, hit.normal, hit.collider.transform);
            
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
