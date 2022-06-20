using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn : MonoBehaviour
{
    [SerializeField]
    public Vector3          _position = Vector3.zero;
    public ParticleSystem   _particle = null;

    public float            _duration = 0.0f;
    public float            _maxDuration = 6.0f;
    public bool             _isSpawn = false;

    public float            _radius = 3.0f;

    void Start()
    {
        _position = transform.position;
        _particle = GetComponent<ParticleSystem>();
        _particle.Play();
    }

    
    void Update()
    {
        // 스폰가능 여부만 체크할꺼
        if(_isSpawn == true) {
            _isSpawn = false;
		}

        _duration += Time.deltaTime;
        if(_maxDuration > _duration) {
            return;
		}
        _duration -= _maxDuration;

        Vector3 origin = transform.position;
        Collider[] colliders = Physics.OverlapSphere(origin, _radius);
        foreach(Collider collider in colliders) { 
            if(collider.GetComponent<PlayerController>() != null) {
                _isSpawn = true;
            }
        }

    }

	public void OnDrawGizmos()
	{
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, _radius);
	}
}
