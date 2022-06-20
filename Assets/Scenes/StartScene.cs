using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartScene : BaseScene
{
    

	void Start()
    {

		Managers.Pool.RegisterPrefab("Boom", 20);
		Managers.Pool.RegisterPrefab("Fire_FX", 20);
		Managers.Pool.RegisterPrefab("Spawn", 4);

	}

	// Update is called once per frame
	void Update()
	{

        
	}

	// ★ 풀링 확인은 Unity에 Destroy영역에 Pool_Root -> TestCube_Root를 펼쳐보면 확인가능 ★

	public override void Clear()
    {

    }
}
