using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class S_Unit : MonoBehaviour
{
    public float team;
    public float speed = 0.1f;
    public float maxhp = 100;
    public float hp;
    public float damage = 10;
    public float rageAttack = 0.25f;
	public GameObject owner;
    public string orderType;
    public Vector3 target;
    public GameObject attackTarget;
	public float distTarget;
    float culdaunAttack;
    public float culdaunAttackMax = 1f;


	float angle = 0;
	float orbit;
	float orbit_speed = 1f;
	// Start is called before the first frame update
	void Start()
    {
        hp = maxhp;
        orderType = "idle";

		angle = UnityEngine.Random.Range(0, 360);
		orbit = UnityEngine.Random.Range(1f, 4f);
		orbit_speed = UnityEngine.Random.Range(0.005f, 0.015f);
	}
    public void ChekTarget()
    {
        attackTarget = null;
		Ray ray = new Ray(transform.position, transform.forward);
		Debug.DrawRay(transform.position, transform.forward);
		RaycastHit hitData;

		if (Physics.Raycast(ray, out hitData))
        {
			distTarget = Vector3.Distance(transform.position, hitData.collider.transform.position);
			if (distTarget <= rageAttack && Vector3.Distance(transform.position, hitData.collider.transform.position) > 0.2f)
			{
                if (hitData.collider.gameObject.tag == "Unit")
                {
                    if (hitData.collider.gameObject.GetComponent<S_Unit>().team != team)
                        attackTarget = hitData.collider.gameObject;
                    else if (Vector3.Distance(transform.position, hitData.collider.transform.position) <= 0.25f)
                    {
                        attackTarget = hitData.collider.gameObject;
                    }
                }
                else if (hitData.collider.gameObject.tag == "Tower")
                {
					if (hitData.collider.gameObject.GetComponent<S_Tower>().team != team)
						attackTarget = hitData.collider.gameObject;
					else if (Vector3.Distance(transform.position, hitData.collider.transform.position) <= 0.25f)
					{
						attackTarget = hitData.collider.gameObject;
					}
				}
			}
			else if(distTarget <= rageAttack)
			{
				if (hitData.collider.gameObject.tag == "Unit")
				{
					if (hitData.collider.gameObject.GetComponent<S_Unit>().team != team)
						attackTarget = hitData.collider.gameObject;
					else if (hitData.collider.gameObject.GetComponent<S_Unit>().distTarget < distTarget)
					{
						attackTarget = hitData.collider.gameObject;
					}
				}
			}
		}
	}

    public void AttackUnit()
    {
		if(culdaunAttack <= 0)
		{
			attackTarget.GetComponent<S_Unit>().TakeDamage(damage);
			culdaunAttack = culdaunAttackMax;
		}
    }
	public void AttackTower()
	{
		if (culdaunAttack <= 0)
		{
			attackTarget.GetComponent<S_Tower>().TakeDamage(damage, team);
			culdaunAttack = culdaunAttackMax;
			TakeDamage(damage);
		}
	}
	public void TakeDamage(float Damage)
    {
		if (hp <= damage)
			Destroy(gameObject);
		else
			hp -= damage;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        ChekTarget();

        if (culdaunAttack > 0)
            culdaunAttack -= Time.deltaTime;

        switch (orderType)
        {
            case "attack":
				Vector3 _direct = (target - transform.position).normalized;
				_direct.y = 0f;

				Quaternion rotation = Quaternion.LookRotation(_direct, Vector3.up);
				transform.rotation = rotation;

				if (attackTarget == null)
                {
					_direct *= speed;
					transform.position = transform.position + _direct;
				}
                else
                {
                    if(attackTarget.tag == "Unit")
                    {
                        if(attackTarget.GetComponent<S_Unit>().team != team)
							AttackUnit();
                    }
                    else if (attackTarget.tag == "Tower")
                    {
						if (attackTarget.GetComponent<S_Tower>().team != team)
							AttackTower();
					}
                }
                break;
			case "idle":

				angle += orbit_speed;
				double _x = Math.Cos(angle) * orbit + owner.transform.position.x;
				double _z = Math.Sin(angle) * orbit + owner.transform.position.z;

				Vector3 _direct2 = (new Vector3((float)_x, 0.2f, (float)_z) - transform.position).normalized;
				Quaternion rotation2 = Quaternion.LookRotation(_direct2, Vector3.up);

				if (attackTarget != null)
				{
					if (attackTarget.tag == "Unit")
					{
						if (attackTarget.GetComponent<S_Unit>().team != team)
							AttackUnit();
						else
						{
							transform.rotation = rotation2;

							transform.position = new Vector3((float)_x, 0.2f, (float)_z);
						}
					}
				}
				else
				{
					transform.rotation = rotation2;

					transform.position = new Vector3((float)_x, 0.2f, (float)_z);
				}
				break;
			case "remove":
				Vector3 _direct3 = (target - transform.position).normalized;
				_direct.y = 0f;

				Quaternion rotation3 = Quaternion.LookRotation(_direct3, Vector3.up);
				transform.rotation = rotation3;

				if (attackTarget == null)
				{
					_direct3 *= speed;
					transform.position = transform.position + _direct3;
				}
				else
				{
					if (attackTarget.tag == "Tower")
					{
						if (attackTarget.GetComponent<S_Tower>().team == team)
						{
							angle = UnityEngine.Random.Range(0, 360);
							orbit = UnityEngine.Random.Range(1f, 4f);
							orbit_speed = UnityEngine.Random.Range(0.005f, 0.015f);

							orderType = "idle";
						}
					}
				}
				break;
		}

	}
}
