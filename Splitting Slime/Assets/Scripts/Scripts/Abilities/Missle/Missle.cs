using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missle : MonoBehaviour
{
    [SerializeField]
    private float missleTime = 3, missleSpeed = 4, missleDamage = 5;
    private float currentMissleTime = 0;
    private Transform target;
    private Vector3 targetPos;
    private bool shooting;

    public void ShootMissle(Transform t)
    {
        currentMissleTime = 0;
        target = t;
        targetPos = target.position;
        shooting = true;
        StartCoroutine(Tracking());
    }

    private void Update()
    {
        if (shooting)
        {
            Vector3 difference = targetPos - transform.position;
            float rotationz = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0.0f, 0.0f, rotationz);
            transform.position = Vector3.MoveTowards(transform.position, targetPos, missleSpeed * Time.deltaTime);
            currentMissleTime += Time.deltaTime;
        }
    }

    private IEnumerator Tracking()
    {
        while(currentMissleTime < missleTime)
        {
            targetPos = target.position;
            yield return new WaitForSeconds(0.5f);
        }
        gameObject.SetActive(false);
        yield break;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            other.GetComponent<EnemyHitHandler>().applyDamage(5);
        }
    }
}
