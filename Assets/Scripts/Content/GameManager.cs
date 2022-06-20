using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager
{
    private PlayerController _player = null;
    public PlayerController Player { get { return _player; } }

    public void Init()
    {
        _player = GameObject.FindObjectOfType<PlayerController>();
        if(_player == null) {
            _player = Managers.Resource.NewPrefab("Player").GetComponent<PlayerController>();
        }
    }


    public void Update()
    {
        
    }
}
