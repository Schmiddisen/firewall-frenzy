using System;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Header("References")] 
    [SerializeField] private Rigidbody2D rb;

    [Header("Attributes")] 
    [SerializeField] private float bulletSpeed = 5f;
    [SerializeField] private float knockbackStrength = 0;
    [SerializeField] private float rotaionOffset = 0;

    private int bulletDamage;

    [SerializeField] private float aliveTimeUntilDespawn = 10;
    

    private Transform target;
    private float timeAlive;

    public void setTarget(Transform _target, int dmg)
    {
        timeAlive = 0f;
        target = _target;
        bulletDamage = dmg;
    }

    private void FixedUpdate()
    {
        timeAlive += Time.deltaTime;
        if (timeAlive > aliveTimeUntilDespawn)
        {
            Destroy(gameObject);
        }
        
        if (!target) return;
        Vector2 direction = (target.position - transform.position).normalized;

        rb.linearVelocity = direction * bulletSpeed;
        rotateTowardsTarget();
    }

    private void rotateTowardsTarget() {
        float angle = Mathf.Atan2(target.position.y - transform.position.y,
            target.position.x - transform.position.x) * Mathf.Rad2Deg;
        
        angle += this.rotaionOffset;
        
        Quaternion targetRotation = Quaternion.Euler(new Vector3(0f, 0f, angle - 90f));
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation,
            250 * Time.deltaTime);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (target == null) return;
        //If it collides with another Enemy, that is NOT the target, dont do the damage
        if (other.collider != target.GetComponent<Collider2D>()) return;

        other.gameObject.GetComponent<Enemy>().takeDamage(bulletDamage);
        other.gameObject.GetComponent<Enemy>().knockback(knockbackStrength);
        Destroy(gameObject);
    }
}
