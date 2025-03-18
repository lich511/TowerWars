using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_PlayerController : MonoBehaviour
{
	// Start is called before the first frame update

	public float speed = 0.1f;

	public Material NormalM;
	public Material EmissevM;

	public Material EnemyM;

	GameObject SelectTower;

	void Start()
    {
        
    }

	// Update is called once per frame
	void Update()
	{
		Vector3 directt = new Vector3(0, 0, 0);
		if (Input.GetKey(KeyCode.W))
			directt.z += 1;
		if (Input.GetKey(KeyCode.S))
			directt.z -= 1;
		if (Input.GetKey(KeyCode.D))
			directt.x += 1;
		if (Input.GetKey(KeyCode.A))
			directt.x -= 1;
		directt = directt.normalized;
		directt = directt * speed;
		transform.position = transform.position + directt;

		Click();
	}

	public void Click()
	{
		if (Input.touchCount > 0)
		{
			Touch touch = Input.GetTouch(0);

			if(touch.phase == TouchPhase.Ended)
			{
				Ray ray = Camera.main.ScreenPointToRay(touch.position);
				RaycastHit hit;

				Debug.Log("touch!");

				if (Physics.Raycast(ray, out hit))
				{
					print(hit.collider.gameObject.name);
					if (hit.collider.gameObject.tag == "Tower")
					{
						Debug.Log("Башня");
						if (hit.collider.gameObject.GetComponent<S_Tower>().team == 0)
						{
							if (SelectTower == hit.collider.gameObject)
							{
								SelectTower.GetComponent<MeshRenderer>().SetMaterials(new List<Material>() { NormalM });
								SelectTower = null;
							}
							else
							{
								if (SelectTower != null)
									SelectTower.GetComponent<MeshRenderer>().SetMaterials(new List<Material>() { NormalM });

								SelectTower = hit.collider.gameObject;
								SelectTower.GetComponent<MeshRenderer>().SetMaterials(new List<Material>() { EmissevM });
							}
						}
						else
						{
							if (SelectTower != null)
							{
								GameObject[] go = GameObject.FindGameObjectsWithTag("Unit");
								foreach (var item in go)
								{
									if (item.GetComponent<S_Unit>().owner == SelectTower)
									{
										item.GetComponent<S_Unit>().target = hit.collider.transform.position;
										item.GetComponent<S_Unit>().orderType = "attack";
									}
								}
							}
						}
					}
					else
					{
						//if (SelectTower != null)
						//{
						//	GameObject[] go = GameObject.FindGameObjectsWithTag("Unit");
						//	foreach (var item in go)
						//	{
						//		if (item.GetComponent<S_Unit>().owner == SelectTower)
						//		{
						//			item.GetComponent<S_Unit>().target = hit.point;
						//			item.GetComponent<S_Unit>().orderType = "attack";
						//		}
						//	}
						//}
					}
				}
			}

			//Vector3 diference = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			//Vector3 diference = Camera.main.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y, 0));

			
		}
	}

}
