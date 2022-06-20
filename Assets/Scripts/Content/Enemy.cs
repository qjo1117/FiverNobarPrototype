using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public PlayerController _target = null;
    public NavMeshAgent     _agent = null;
    public float            _hp = 100.0f;


    void Start()
    {
        Init();
    }

    private void Init()
	{
        if (_agent == null) {
            _agent = GetComponent<NavMeshAgent>();
        }

        if (_target == null) {
            _target = Managers.Game.Player;
        }
    }

    public void Attack(float attack)
	{
        _hp -= attack;
        if(_hp <= 0) {
            Managers.Resource.DelPrefab(gameObject);
		}
	}


    void Update()
    {
        _agent.SetDestination(_target.transform.position);

        if (Physics.Raycast(transform.position, -Vector3.up, 1.1f, 1 << 6)) {
            transform.position -= Vector3.up * 0.1f;
        }
    }
}
