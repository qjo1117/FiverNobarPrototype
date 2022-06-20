using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public List<Spawn> _spawns = new List<Spawn>();
    public List<Enemy> _enemys = new List<Enemy>();

    public float _spawnTime = 10.0f;

    void Start()
    {
        Init();
    }

    private void Init()
	{
        // 그래 이건 수동으로 하자.
        //for(int i = 0; i < _spawnCount; ++i) {
        //    Spawn spawn = Managers.Resource.NewPrefab("Spawn").GetComponent<Spawn>();
        //    _spawns.Add(spawn);
        //}

        Transform spawnParent = transform.GetChild(0);
        for (int i = 0; i < spawnParent.childCount; ++i) {
            Transform spawn = spawnParent.GetChild(i);
            if(spawn.GetComponent<Spawn>() != null) {
                _spawns.Add(spawn.GetComponent<Spawn>());
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        CheckSpawn();
    }

    private void CheckSpawn()
	{
        foreach(Spawn spawn in _spawns) {
            if(spawn._isSpawn == true) {            // 스폰이 가능한 경우
                Managers.Log(spawn._position);
                MonsterSpawn(spawn._position);
            }
		}
	}

    private void MonsterSpawn(Vector3 position)
	{
        Enemy enemy = Managers.Resource.NewPrefab("Enemy", transform).GetComponent<Enemy>();
        _enemys.Add(enemy);
        enemy.transform.position = position;
    }
}
