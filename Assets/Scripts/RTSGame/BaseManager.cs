using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseManager : MonoBehaviour
{
    public GameManager gameManager;
    public GameManager.GameTeams Team;
    public List<GameObject> Buildings = new List<GameObject>();

    #region Player Building Prefabs
    public GameObject prefabWarriorBuilding;
    public GameObject prefabRangerBuilding;
    public GameObject prefabRogueBuilding;
    public GameObject prefabHealerBuilding;
    #endregion

    #region Enemy Building Prefabs
    public GameObject prefabEnemyWarriorBuilding;
    public GameObject prefabEnemyRangerBuilding;
    public GameObject prefabEnemyRogueBuilding;
    public GameObject prefabEnemyHealerBuilding;
    #endregion

    public int currentNumOfBuildings = 0;
    public int maxNumOfBuildings = 8;

    private void Awake()
    {
        CreateEnemyBuildings();
    }
    public void Update()
    {
        CountBuildings();
    }

    private void CountBuildings()
    {
        currentNumOfBuildings = 0;
        foreach (var b in Buildings)
        {
            if (b != null)
            {
                currentNumOfBuildings++;
            }
        }
    }

    /// <summary>
    /// Hard-coded method to create the default buildings for the Enemy.
    /// </summary>
    private void CreateEnemyBuildings()
    {
        if (this.Team == GameManager.GameTeams.Enemy)
        {
            AddBuildingToBase(Instantiate(prefabEnemyWarriorBuilding, new Vector3(125, 4, 112), Quaternion.identity));
            AddBuildingToBase(Instantiate(prefabEnemyWarriorBuilding, new Vector3(112, 4, 125), Quaternion.identity));
            AddBuildingToBase(Instantiate(prefabEnemyRogueBuilding, new Vector3(138, 4, 112), Quaternion.identity));
            AddBuildingToBase(Instantiate(prefabEnemyRogueBuilding, new Vector3(112, 4, 138), Quaternion.identity));
            AddBuildingToBase(Instantiate(prefabRangerBuilding, new Vector3(138, 4, 125), Quaternion.identity));
            AddBuildingToBase(Instantiate(prefabRangerBuilding, new Vector3(125, 4, 138), Quaternion.identity));
            AddBuildingToBase(Instantiate(prefabEnemyHealerBuilding, new Vector3(125, 4, 125), Quaternion.identity));
            AddBuildingToBase(Instantiate(prefabEnemyHealerBuilding, new Vector3(138, 4, 138), Quaternion.identity));
            currentNumOfBuildings = maxNumOfBuildings;
        }
    }

    /// <summary>
    /// Place a Building at the Player Base using a selectedType
    /// </summary>
    /// <param name="selectedType"></param>
    public void PlaceBuildingOfSelectedType(GameManager.ClassTypes selectedType)
    {
        if (currentNumOfBuildings < maxNumOfBuildings)
        {
            //Raycast to point on screen and check if the building can spawn here
            RaycastHit hit;
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 100) && hit.collider.gameObject.tag == "Base")
            {
                //Check the current class selection and Instantiate that Building at hit.point
                switch (selectedType)
                {
                    case GameManager.ClassTypes.Warrior:
                        AddBuildingToBase(Instantiate(prefabWarriorBuilding, hit.point, Quaternion.identity));
                        break;
                    case GameManager.ClassTypes.Ranger:
                        AddBuildingToBase(Instantiate(prefabRangerBuilding, hit.point, Quaternion.identity));
                        break;
                    case GameManager.ClassTypes.Rogue:
                        AddBuildingToBase(Instantiate(prefabRogueBuilding, hit.point, Quaternion.identity));
                        break;
                    case GameManager.ClassTypes.Healer:
                        AddBuildingToBase(Instantiate(prefabHealerBuilding, hit.point, Quaternion.identity));
                        break;
                }
                currentNumOfBuildings++;
            }
        }
        else
        {
            Debug.Log("Maximum Num Of Buildings Reached");
        }
    }

    /// <summary>
    /// Check this base for a specified ClassType.
    /// </summary>
    /// <param name="_selectedType"></param>
    /// <returns></returns>
    public virtual bool HasSelectedType(GameManager.ClassTypes _selectedType)
    {
        foreach (GameObject building in this.Buildings)
        {
            if (building.GetComponent<BuildingController>().type == _selectedType)
            {
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// Add a GameObject to the Buildings List<>
    /// </summary>
    /// <param name="building"></param>
    public virtual void AddBuildingToBase(GameObject building)
    {
        Buildings.Add(building);
    }

    /// <summary>
    /// Remove a GameObject from the Buildings List<>
    /// </summary>
    /// <param name="building"></param>
    public virtual void RemoveBuildingFromBase(GameObject building)
    {
        Debug.Log("Removing Building");
        Buildings.Remove(building);
        Destroy(building);
    }

    /// <summary>
    /// Take Damage from an enemy Unit by sending the damage to a random building in the Buildings List<>
    /// </summary>
    /// <param name="receivedDamage"></param>
    public virtual void TakeDamage(float receivedDamage)
    {
        Debug.Log(this + " UNDER ATTACK!");
        int rand = Random.Range(0, Buildings.Count);
        if (Buildings[rand] != null)
            Buildings[rand].GetComponent<BuildingController>().TakeDamage(receivedDamage);
    }

}
