using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_Tower : MonoBehaviour
{
    public Material teamMaterial;
    public float team;
    public float maxhp;
    public float hp;
	public float maxUnit = 100f;
	public GameObject UnitPrefab;

    float culdaun;
    public float culdaunMax = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        hp = maxhp;
    }

	public void TakeDamage(float damage, float teamunit)
	{
		if (hp <= damage)
		{
			team = teamunit;
			GameObject[] go = GameObject.FindGameObjectsWithTag("MainCamera");

			if (team == 1)
				teamMaterial = go[0].GetComponent<S_PlayerController>().EnemyM;
			else
				teamMaterial = go[0].GetComponent<S_PlayerController>().NormalM;

			hp = maxhp;

			GameObject[] go2 = GameObject.FindGameObjectsWithTag("Unit");
			foreach (var item in go2)
			{
				if (item.GetComponent<S_Unit>().owner == gameObject)
				{
					Destroy(item);
				}
			}
		}
		else
			hp -= damage;
	}

	// Update is called once per frame
	void FixedUpdate()
    {
        if(culdaun <= 0)
        {

			GameObject[] go = GameObject.FindGameObjectsWithTag("Unit");
			int _co = 0;
			foreach (var item in go)
			{
				if (item.GetComponent<S_Unit>().owner == gameObject)
					_co++;
			}

			if (_co < maxUnit)
			{
				culdaun = culdaunMax;

				GameObject unit = Instantiate(UnitPrefab);
				unit.transform.position = transform.position;
				S_Unit _UC = unit.GetComponent<S_Unit>();
				_UC.owner = gameObject;
				_UC.team = team;

				_UC.GetComponent<MeshRenderer>().SetMaterials(new List<Material>() { teamMaterial });
			}
        }
        else
            culdaun -= Time.deltaTime;
    }
}
