// Simulate a particle system without Unity's particle system.
// Unity's PS cannot achieve the behaviour I'm making, it seems.
// Each candy is sent moving in an arc, and when it dies, it spawns a separate particle system.

using System.Collections;
using UnityEngine;

public class PSCandy : MonoBehaviour
{
    [SerializeField] protected ParticleSystem psCandySub;

    float _speedMin = 15, _speedMax = 17;
    float _gravityMin = 2f, _gravityMax = 2.75f;
    float _angleBase = -65f, _angleRange = 3f;
    float _deathTime = 1.25f;

    private void Start()
    {
        //Launch();
        Launch(15.5f, 18, 1, 1.75f, _angleBase, _angleRange, _deathTime);  // Seems to work? Maybe??
    }

    // Assumed: This candy is spawned in the intended position.
    // Launches a candy from an enemy to the allies.
    public void Launch()
    {
        StartCoroutine(ILaunch());
    }

    // Assumed: This candy is spawned in the intended position.
    // Launches a candy from an enemy to the allies.
    public void Launch(float speedMin, float speedMax, float gravityMin, float gravityMax, float angleBase, float angleRange, float deathTime)
    {
        _speedMin = speedMin;
        _speedMax = speedMax;
        _gravityMin = gravityMin;
        _gravityMax = gravityMax;
        _angleBase = angleBase;
        _angleRange = angleRange;
        _deathTime = deathTime;
        StartCoroutine(ILaunch());
    }

    // Potentially more suitable implementation than these parameters, the particle system parameters,
    //     would be the target position and height ranges instead of the speed and gravity ranges
    // Would be more flexible, too. But hey, this is just a non-serious experiment.
    private IEnumerator ILaunch()
    {
        Vector3 startPosition = transform.position;
        float x1 = startPosition.x;
        float y1 = startPosition.y;
        float z = startPosition.z;

        float angle = _angleBase + Random.Range(-_angleRange, _angleRange);
        float speed = Random.Range(_speedMin, _speedMax);
        float speedX = 0.15f * Mathf.Abs(speed / Mathf.Cos(Mathf.Deg2Rad * angle));  // cos(angle) = speed / speedx -> speedx = speed / cos(angle)
        float speedY = 0.4f *  Mathf.Abs(speed / Mathf.Sin(Mathf.Deg2Rad * angle));  // speedy = speed / sin(angle)
        float gravity = 10f * Random.Range(_gravityMin, _gravityMax);
        float x2 = x1 - speedX * _deathTime;  // x = x0 + v*t, but in the other direction.

        float startTime = Time.time;
        float endTime = startTime + _deathTime;
        float t;
        float y;
        while (Time.time <= endTime)
        {
            t = (Time.time - startTime) / _deathTime;
            y = y1 + speedY * t - gravity * t * t / 2;
            transform.position = new Vector3(Mathf.Lerp(x1, x2, t), y, z);
            yield return new WaitForEndOfFrame();
        }
        OnDeath();
    }

    private void OnDeath()
    {
        Instantiate(psCandySub, transform.position, Quaternion.identity, null);
        Destroy(gameObject);
    }
}
