using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardController : MonoBehaviour
{
    public enum GuardState { Patrol, Investigate, Chase}
    public GuardState currentState = GuardState.Patrol;

    //Player-Detection Settings
    Transform player;
    float fovDist = 20f;
    float fovAngle = 45;

    //Where player was last seen
    Vector3 lastPlaceSeen;

    //Chase Settings
    float chasingSpeed = 2.0f;
    float chasingRotSpeed = 5.0f;
    float chasingAccuracy = 1f;

    //Patrol Settings
    public float patrolDistance = 10.0f;
    float patrolWait = 5.0f;
    float patrolTimePassed = 0f;

    //Hunger Settings
    float hungerUpdateWait = 10.0f; //Gets a speed increase every 10 seconds
    float hungerUpdateTimePassed = 0f; //track since last speed increase



    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        patrolTimePassed = patrolWait;
        hungerUpdateTimePassed = hungerUpdateWait;
        lastPlaceSeen = this.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(this + " State: " + currentState);
        if (ICanSee(player)) currentState = GuardState.Chase;
        else
        {
            if (currentState == GuardState.Chase) currentState = GuardState.Investigate;
        }

        switch (currentState)
        {
            case GuardState.Patrol:
                Patrol();
                break;
            case GuardState.Investigate:
                Investigate();
                break;
            case GuardState.Chase:
                Chase(player);
                break;
            default:
                Patrol();
                break;
        }

        if (CanEatPlayer()) 
            RestartGame();

        //Max Speed is 5.5 (just below player's speed)
        if (chasingSpeed <= 5.5f) 
            BuildHunger();
    }

    bool ICanSee(Transform player)
    {
        Vector3 direction = player.position - this.transform.position;
        float angle = Vector3.Angle(direction, this.transform.forward);

        RaycastHit hit;

        if (Physics.Raycast(this.transform.position, direction, out hit) && hit.collider.gameObject.tag == "Player"
            && direction.magnitude < fovDist && angle < fovAngle) 
        {
            return true;
        }

        return false;
    }

    void Chase(Transform player)
    {
        this.GetComponent<UnityEngine.AI.NavMeshAgent>().isStopped = true;
        this.GetComponent<UnityEngine.AI.NavMeshAgent>().ResetPath();

        Vector3 direction = player.position - this.transform.position;
        this.transform.rotation = Quaternion.Slerp(this.transform.rotation, 
                                                    Quaternion.LookRotation(direction), 
                                                    Time.deltaTime * this.chasingRotSpeed);

        if (direction.magnitude > this.chasingAccuracy)
        {
            this.transform.Translate(0, 0, Time.deltaTime * this.chasingSpeed);
            lastPlaceSeen = player.position;
        }
    }

    void Investigate()
    {
        if (Vector3.Distance(this.transform.position, lastPlaceSeen) <= chasingAccuracy)
        {
            currentState = GuardState.Patrol;
        }
        else
        {
            this.GetComponent<UnityEngine.AI.NavMeshAgent>().SetDestination(lastPlaceSeen);
        }
    }

    void Patrol()
    {
        patrolTimePassed += Time.deltaTime;
        
        if (patrolTimePassed > patrolWait)
        {
            patrolTimePassed = 0f;

            Vector3 patrollingPoint = lastPlaceSeen;
            patrollingPoint += new Vector3(Random.Range(-patrolDistance, patrolDistance),
                                            0f,
                                            Random.Range(-patrolDistance, patrolDistance));

            this.GetComponent<UnityEngine.AI.NavMeshAgent>().SetDestination(patrollingPoint);

        }

    }

    public void InvestigatePoint (Vector3 point)
    {
        lastPlaceSeen = point;
        currentState = GuardState.Investigate;
    }

    void BuildHunger()
    {
        hungerUpdateTimePassed += Time.deltaTime;

        if (hungerUpdateTimePassed > hungerUpdateWait)
        {
            hungerUpdateTimePassed = 0f;
            chasingSpeed += .5f;

            //if speed Somehow goes above 5.5, Make it 5.5
            if (chasingSpeed > 5.5f) 
                chasingSpeed = 5.5f;
        }

        Debug.Log($"{this} Speed after Hunger Increase: " + chasingSpeed);
    }

    public bool CanEatPlayer()
    {
        if (Vector3.Distance(player.position, this.transform.position) < 1.5f)
        {
            return true;
        }
        return false;
    }

    public void RestartGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
    }
}
