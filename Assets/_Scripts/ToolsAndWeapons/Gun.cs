using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Gun : MonoBehaviour
{
    [Header("-- Stats --")]
    public float damage = 10f;
    public float range = 100f;
    public float shootRate;
    public float ammo;
    public float maxAmmo;
    [SerializeField] bool Mele;

    [Header("-- States --")]
    public bool reloading = false;
    public bool shooting = false;

    [Header("-- Visuals --")]
    public Image crosshair;
    public ParticleSystem muszzleFlash;

    private float nextTimeTofire = 0f;

    public void Shoot()
    {
        if(Time.time >= nextTimeTofire)
        {
            nextTimeTofire = Time.time + 1f / shootRate;
            if(!Mele)
            {
                muszzleFlash.Play();
                RaycastHit hit;
                if (Physics.Raycast(gameManager.instance.playerCamera.transform.position, gameManager.instance.playerCamera.transform.forward, out hit, range))
                {
                    IDamage target = hit.transform.GetComponent<IDamage>();
                    if (target != null)
                    {
                        target.TakeDamage(damage);
                    }
                }
                Debug.Log("Shotting");
            }
            else
            {
                
                RaycastHit hit;
                if (Physics.Raycast(gameManager.instance.playerCamera.transform.position, gameManager.instance.playerCamera.transform.forward, out hit, range))
                {
                    IDamage target = hit.transform.GetComponent<IDamage>();
                    if (target != null)
                    {
                        target.TakeDamage(damage);
                    }
                }
                Debug.Log("Swinging");
            }
            
        }
        
    }
}
