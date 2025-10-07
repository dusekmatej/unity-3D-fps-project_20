using UnityEngine;

public class Shooting : MonoBehaviour
{
    public Animator animator;
    public string shootAnimationTrigger = "Shoot";

    void Update()
    {
       
        if (Input.GetButtonDown("Fire1"))
        {
            Shoot();
        }
    }

    void Shoot()
    {
        
        animator.SetTrigger(shootAnimationTrigger);

       
    }
}