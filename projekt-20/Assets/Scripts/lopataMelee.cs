using UnityEngine;

public class MeleeWeapon : MonoBehaviour
{
    public float damage = 20f;
    public float attackRange = 2f; 
    public float attackCooldown = 1f; 
    public Camera fpsCam; 
    public ParticleSystem attackEffect;
    public Animator animator; 

    private float lastAttackTime = 0f; 

    void Update()
    {
   
        if (Input.GetButtonDown("Fire1") && Time.time >= lastAttackTime + attackCooldown)
        {
            Attack();
            lastAttackTime = Time.time; 
        }
    }

    void Attack()
    {


        if (animator != null)
        {
            animator.SetTrigger("Attack");
        }


        if (attackEffect != null)
        {
            attackEffect.Play();
        }
       

        RaycastHit hit;
        if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, attackRange))
        {
            Debug.Log("Hit: " + hit.transform.name);


            Target target = hit.transform.GetComponent<Target>();
            if (target != null)
            {
                target.TakeDamage(damage); 
            }
        }
        
    }
}