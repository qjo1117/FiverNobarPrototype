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

	// �� Ǯ�� Ȯ���� Unity�� Destroy������ Pool_Root -> TestCube_Root�� ���ĺ��� Ȯ�ΰ��� ��

	public override void Clear()
    {

    }
}
