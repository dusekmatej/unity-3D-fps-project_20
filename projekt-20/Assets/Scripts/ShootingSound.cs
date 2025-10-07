using UnityEngine;

public class Gun : MonoBehaviour
{
    public AudioSource audioSource;  // Assign in the Inspector
    public AudioClip shootingSound;  // Drag your first sound clip here
    public AudioClip secondSound;    // Drag your second sound clip here

    void Update()
    {
        
        if (Input.GetButtonDown("Fire1"))
        {
            Shoot();
        }
    }

    void Shoot()
    {
        
        audioSource.PlayOneShot(shootingSound);

        
        audioSource.PlayOneShot(secondSound);

        
       
    }
}
