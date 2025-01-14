using System;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Header("References")] 
    [SerializeField] private Rigidbody2D rb;

    [Header("Attributes")] 
    [SerializeField] private float bulletSpeed = 5f;

    [SerializeField] private int bulletDamage = 1;
    [SerializeField] private float aliveTimeUntilDespawn = 10;
    

    private Transform target;
    private float timeAlive;

    public void setTarget(Transform _target)
    {
        timeAlive = 0f;
        target = _target;
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
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        other.gameObject.GetComponent<Enemy>().takeDamage(bulletDamage);
        Destroy(gameObject);
    }
}
