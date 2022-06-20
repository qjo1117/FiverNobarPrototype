using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoomFire : MonoBehaviour
{
    public ParticleSystem _system = null;
    public float _duration = 0.0f;
    public float _maxDuration = 2.0f;

    void Start()
    {
        
    }

    private void Init()
	{
        if(_system == null) {
            _system = GetComponent<ParticleSystem>();
        }
        _duration = 0.0f;

        transform.localScale = Vector3.one * 0.5f;
    }

    // Update is called once per frame
    void Update()
    {
        _duration += Time.deltaTime;
        if(_maxDuration > _duration) {
            return;
		}

        Managers.Resource.DelPrefab(gameObject);
    }
}
