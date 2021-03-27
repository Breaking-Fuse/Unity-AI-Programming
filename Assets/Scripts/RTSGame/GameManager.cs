using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public enum GameStates { Idle, Playing, PlayerWin, EnemyWin }
    public enum GameTeams { Player, Enemy }
    public enum ClassTypes { Warrior, Ranger, Rogue, Healer }

    public GameStates State = GameStates.Idle;
    public ClassTypes selectedType = ClassTypes.Warrior;

    #region Player/Enemy Managers
    public UnitManager playerUnitManager;
    public BaseManager playerBaseManager;
    public UnitManager enemyUnitManager;
    public BaseManager enemyBaseManager;
    #endregion

    private void Update()
    {
        CheckForAnyInput();

        //Start the Game / Allow Unit Placement after Player Buildings are placed
        if (State != GameStates.Playing && playerBaseManager.currentNumOfBuildings >= playerBaseManager.maxNumOfBuildings)
            State = GameStates.Playing;

        CheckForWinner();
    }

    /// <summary>
    /// If there is no Winner yet, check for any Input
    /// </summary>
    private void CheckForAnyInput()
    {
        if (State == GameStates.Idle || State == GameStates.Playing)
        {
            if (Input.anyKeyDown)
            {
                SelectClassType();
                CheckForPlacement();
            }
        }
    }

    /// <summary>
    /// Check the game state then place a building or unit
    /// </summary>
    private void CheckForPlacement()
    {
        if (Input.GetMouseButtonDown(0))
        {
            switch (State)
            {
                case GameStates.Idle:
                    playerBaseManager.PlaceBuildingOfSelectedType(selectedType);
                    break;
                case GameStates.Playing:
                    if (playerBaseManager.HasSelectedType(selectedType))
                        playerUnitManager.PlaceUnitOfSelectedType(selectedType);
                    break;
            }
        }
    }

    /// <summary>
    /// Check remaining number of Buidings in each BaseManager to determine a Winner.
    /// </summary>
    private void CheckForWinner()
    {
        if (State == GameStates.Playing && playerBaseManager.currentNumOfBuildings <= 0)
        {
            Debug.Log("Enemy Wins");
            State = GameStates.EnemyWin;
        }
        else if(State == GameStates.Playing && enemyBaseManager.currentNumOfBuildings <= 0)
        {
            Debug.Log("Player Wins");
            State = GameStates.PlayerWin;
        }
    }

    /// <summary>
    /// Checks input of Keys 1-4 to select a Building/Class-type
    /// </summary>
    private void SelectClassType()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
            selectedType = ClassTypes.Warrior;
        else if (Input.GetKeyDown(KeyCode.Alpha2))
            selectedType = ClassTypes.Ranger;
        else if (Input.GetKeyDown(KeyCode.Alpha3))
            selectedType = ClassTypes.Rogue;
        else if (Input.GetKeyDown(KeyCode.Alpha4))
            selectedType = ClassTypes.Healer;
    }

}
