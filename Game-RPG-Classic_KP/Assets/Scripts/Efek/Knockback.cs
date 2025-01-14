using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knockback : MonoBehaviour
{
    public bool gettingKnockedBack { get; private set; }

    [SerializeField] private float knockBackTime = .2f;

    private Rigidbody2D rb;

    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
    }

    public void GetKnockedBack(Transform damageSource, float knockBackThrust) {
        gettingKnockedBack = true;
        Vector2 difference = (transform.position - damageSource.position).normalized * knockBackThrust * rb.mass;
        rb.AddForce(difference, ForceMode2D.Impulse); 
        StartCoroutine(KnockRoutine(knockBackTime));
    }
    public void GetKnockedBackBoss(Transform damageSource, float knockBackThrust, float time) {
        gettingKnockedBack = true;
        Vector2 difference = (transform.position - damageSource.position).normalized * knockBackThrust * rb.mass;
        rb.AddForce(difference, ForceMode2D.Impulse); 
        StartCoroutine(KnockRoutine(time));
    }

    private IEnumerator KnockRoutine(float waktu) {
        yield return new WaitForSeconds(waktu);
        rb.velocity = Vector2.zero;
        gettingKnockedBack = false;
    }
}
