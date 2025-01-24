using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

public class Gun : MonoBehaviour
{

    private Ray ray;
    public float maxDistance;
    public LayerMask objectToHit;
    public GameObject bulletPrefab;
    public int maxAmo = 30;
    private int amo;
    public bool canShoot = true;
    public AudioSource fire1;
    public AudioSource fire2;
    public AudioSource fire3;
    public AudioSource fire4;
    public AudioSource toutch;
    public AudioSource reload;
    public AudioSource noAmo;

    private GameObject target;
    // Start is called before the first frame update
    void Start()
    {
        amo = maxAmo;
        target = GameObject.Find("Player2");
    }

    // Update is called once per frame
    void Update()
    {
        HandleShoot();
        Reload();
    }

    public void HandleShoot()
    {
        if (!canShoot) return;
        if (Input.GetKeyDown(KeyCode.Mouse0)) 
        {
            Shoot();
        }
    }

    public void Shoot()
    {
        if (amo != 0)   
        {  
            var fireSounds = new AudioSource[] { fire1, fire2, fire3, fire4 };
            fireSounds[UnityEngine.Random.Range(0, fireSounds.Length)].Play();
            Vector3 bulletPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z);
            GameObject bullet = Instantiate(bulletPrefab, bulletPosition, transform.rotation);
            bullet.transform.localPosition = new Vector3(bullet.transform.position.x, bullet.transform.position.y, bullet.transform.position.z);

            //Physics.Raycast(transform.position, Vector3.forward, 1000);
            RayCastManager();
            amo--;
            if(canShoot) GameObject.Find("Game Values").GetComponent<WS>().SendMessage("shoot", new Dictionary<string, object> {{"shoot", "shoot"}});
        }
        else
        {
            noAmo.Play();
        }
    }

    private void RayCastManager()
    {
        Vector3 rayOrigin = Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0.5f));
        
        RaycastHit hit;
        if (Physics.Raycast(rayOrigin, Camera.main.transform.forward, out hit, 1000 /*objectToHit*/))
        {
            if (hit.collider.name == "Player2")
            {
                
                toutch.Play();
                GameObject.Find("Game Values").GetComponent<WS>().SendMessage("hit", new Dictionary<string, object> {{"hit", "hit"}});
                target.GetComponent<PlayerControler>().playerHealth -= 10; 
                target.GetComponent<PlayerControler>().CheckHp();
            }
        }
        
    }

    private void Reload()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            reload.Play();
            Debug.Log("Reload");
            var task = Task.Run(() => Debug.Log("Reloading"));
            if (task.Wait(TimeSpan.FromSeconds(3000)))
            {
                Debug.Log("Reloading");
                amo = maxAmo;
            }   
        }
    }
}
