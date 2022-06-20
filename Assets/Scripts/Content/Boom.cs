using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boom : MonoBehaviour
{
    [Header("Stat")]
    public float        _radius = 0.5f;
    public float        _damege = 70.0f;

    public float        _velocity = 0.0f;
    public float        _maxVelocity = 500.0f;

    public float        _groundDelay = 0.0f;
    public float        _groundMaxDelay = 0.1f;

    public float        _explosionDelay = 0.0f;
    public float        _explosionMaxDelay = 2.0f;

    public float        _explosionRadius = 3.0f;
    public float        _explosionForce = 500.0f;

    public float        _monsterCheck = 1.0f;

    [Header("부가기능")]
    public int          _bounce = 1;
    public Rigidbody    _rigid = null;



    // Start is called before the first frame update
    void Start()
    {
        Init();

    }

	private void Init()
	{
        if(_rigid == null) {
            _rigid = GetComponent<Rigidbody>();
        }

        transform.localScale = Vector3.one * _radius;
        GetComponent<SphereCollider>().radius = _radius / 2.0f;

        _velocity = 0.0f;
        _groundDelay = 0.0f;
        _explosionDelay = 0.0f;
        _bounce = 1;

        _rigid.velocity = Vector3.zero;
    }

	// Update is called once per frame
	void Update()
    {
        MonsterCollisionExplosion();
        GroundEnterToAddForce();
        ExplosionCheck();
    }

    private void ExplosionCheck()
	{
        _explosionDelay += Time.deltaTime;
        if (_explosionMaxDelay > _explosionDelay) {
            return;
        }
        Explosion();
    }

    public void CreateBoom(Vector3 pos, Vector3 velocity)
	{
        Init();

        transform.position = pos;
        _rigid.AddForce(velocity);
    }

 //   private void BoundWell()
	//{
 //       RaycastHit[] hit = Physis.SphereCastAll()

 //   }

    private void MonsterCollisionExplosion()
    {
        Vector3 origin = transform.position;
        RaycastHit hit;

        RaycastHit[] colliders = Physics.SphereCastAll(origin, _monsterCheck, Vector3.up, 0.0f, 1 << 7);

        if (colliders.Length != 0) {
            Explosion();    
        }
    }


    public void Explosion()
	{
        _explosionDelay = 0.0f;

        Vector3 origin = transform.position;

        // Enemy에 해당하는 녀석들을 검사해서 폭탄 중점기준으로 밀어버린다.
        RaycastHit[] colliders = Physics.SphereCastAll(origin, _explosionRadius, Vector3.up, 0.0f, 1 << 7 | 1 << 8);
        if (colliders.Length != 0) {
            foreach(RaycastHit hit in colliders) {
                Vector3 vecDist = origin - hit.collider.transform.position;

                float dist = vecDist.magnitude;
                float force = _explosionForce / dist;

                if(hit.collider.gameObject.GetComponent<PlayerController>() != null) {
                    force *= 1.4f;
                }

                hit.collider.gameObject.GetComponent<Rigidbody>().AddForce(-vecDist.normalized * force);

                Enemy enemy = hit.collider.gameObject.GetComponent<Enemy>();
                if(enemy) {
                    enemy.Attack(_damege);
                }
            }
		}

        Managers.Resource.NewPrefab("Fire_FX").transform.position = transform.position;

        Managers.Resource.DelPrefab(gameObject);
    }

    private void GroundEnterToAddForce()
	{
        _groundDelay += Time.deltaTime;

        if (_groundMaxDelay > _groundDelay) {
            return;
		}

        _groundDelay = 0.0f;
        
        Vector3 origin = transform.position;
        if (Physics.Raycast(origin, -Vector3.up, 0.51f)) {
            _velocity = _maxVelocity / _bounce;
            _bounce += 1;

            _rigid.AddForce(Vector3.up * _velocity);
        }
    }

	private void OnDrawGizmos()
	{
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, _explosionRadius);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _monsterCheck);
    }
}
