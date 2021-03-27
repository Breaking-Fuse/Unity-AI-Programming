using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class UnitManager : MonoBehaviour
{
    public enum UnitStates {Searching, AttackingUnit, AttackingBase, Defending, Taunting, Fleeing, Dead }
    public GameManager gameManager;
    public GameManager.GameTeams Team;
    public BaseManager baseManager;

    #region Unit Prefabs
    public GameObject prefabWarriorUnit;
    public GameObject prefabRangerUnit;
    public GameObject prefabRogueUnit;
    public GameObject prefabHealerUnit;
    #endregion

    //List to keep track of this Team's Units
    public List<GameObject> Units = new List<GameObject>();

    //Rival Base location for the units to target
    public GameObject rivalBase;

    public GameObject prefabLeftEnemySpawnArea;
    public GameObject prefabRightEnemySpawnArea;

    private int currentNumOfUnits = 0;
    private int maxNumOfUnits = 8;

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    private void Update()
    {
        EnemySelectRandomType();
    }

    /// <summary>
    /// Add a unit to this manager's Unit List.
    /// </summary>
    public void AddToUnitList(GameObject unit)
    {
        Units.Add(unit);
        unit.GetComponent<UnitController>().manager = this;
        unit.GetComponent<UnitController>().Team = this.Team;
        unit.GetComponent<UnitController>().target = rivalBase;
        currentNumOfUnits++;
    }

    /// <summary>
    /// Delete a unit from this manager's Unit List.
    /// </summary>
    public void RemoveFromUnitList(GameObject unit)
    {
        Units.Remove(unit);
        currentNumOfUnits--;
    }


    /// <summary>
    /// Place a Unit using a selectedType at the mousePosition inside a valid UnitSpawnArea
    /// </summary>
    /// <param name="selectedType"></param>
    public void PlaceUnitOfSelectedType(GameManager.ClassTypes selectedType)
    {
        if (currentNumOfUnits < maxNumOfUnits)
        {
            //Raycast to point on screen and check if the unit can spawn here
            RaycastHit hit;
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 100) && hit.collider.gameObject.tag == "UnitSpawnArea")
            {
                //Check the current class selection and Instantiate that Unit at hit.point
                switch (selectedType)
                {
                    case GameManager.ClassTypes.Warrior:
                        AddToUnitList(Instantiate(prefabWarriorUnit, hit.point, Quaternion.identity));
                        break;
                    case GameManager.ClassTypes.Ranger:
                        AddToUnitList(Instantiate(prefabRangerUnit, hit.point, Quaternion.identity));
                        break;
                    case GameManager.ClassTypes.Rogue:
                        AddToUnitList(Instantiate(prefabRogueUnit, hit.point, Quaternion.identity));
                        break;
                    case GameManager.ClassTypes.Healer:
                        AddToUnitList(Instantiate(prefabHealerUnit, hit.point, Quaternion.identity));
                        break;
                }
            }
        }
    }


    /// <summary>
    /// If this is the Enemy UnitManager, try to place a unit of the selectedType at either of the two given Spawn Areas
    /// </summary>
    /// <param name="selectedType"></param>
    public void PlaceUnitOfSelectedTypeRandomly(GameManager.ClassTypes selectedType)
    {
        if (this.baseManager.HasSelectedType(selectedType) && currentNumOfUnits < maxNumOfUnits)
        {
            GameObject randomSpawnArea = GetRandomSpawnArea();

            //Check the current class selection and Instantiate that Unit at hit.point
            switch (selectedType)
            {
                case GameManager.ClassTypes.Warrior:
                    AddToUnitList(Instantiate(prefabWarriorUnit, randomSpawnArea.transform.position, Quaternion.identity));
                    break;
                case GameManager.ClassTypes.Ranger:
                    AddToUnitList(Instantiate(prefabRangerUnit, randomSpawnArea.transform.position, Quaternion.identity));
                    break;
                case GameManager.ClassTypes.Rogue:
                    AddToUnitList(Instantiate(prefabRogueUnit, randomSpawnArea.transform.position, Quaternion.identity));
                    break;
                case GameManager.ClassTypes.Healer:
                    AddToUnitList(Instantiate(prefabHealerUnit, randomSpawnArea.transform.position, Quaternion.identity));
                    break;
            }
        }
    }


    /// <summary>
    /// If this is the Enemy UnitManager, select a random type and randomly place the unit.
    /// </summary>
    private void EnemySelectRandomType()
    {
        
        if (this.Team == GameManager.GameTeams.Enemy && gameManager.State == GameManager.GameStates.Playing && this.currentNumOfUnits < this.maxNumOfUnits)
            PlaceUnitOfSelectedTypeRandomly((GameManager.ClassTypes)Random.Range(0, 3));
    }

    /// <summary>
    /// Returns a Random Spawn Area (Left Or Right).
    /// </summary>
    /// <returns></returns>
    private GameObject GetRandomSpawnArea()
    {
        int rand = Random.Range(0, 2);
        switch (rand)
        {
            case 0:
                return prefabLeftEnemySpawnArea;
            case 1:
                return prefabRightEnemySpawnArea;
        }
        return prefabLeftEnemySpawnArea;

    }
}
