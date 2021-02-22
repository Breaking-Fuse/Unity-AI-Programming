using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
    public enum MoveState {Moving, Waiting}

    // Initialize Variables
    public Vector3 goal;
    private MoveState myMoveState = MoveState.Waiting;
    float speed = 2f;
    float accuracy = .5f;
    float rotSpeed = 2f;

    // Start is called before the first frame update
    void Start()
    {
        //initialize target (this transform's position upon initialization)
        goal = this.transform.position;

    }

    // Update is called once per frame
    void Update()
    {
        //Initialize Raycast vars
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        //Perform raycast with left-click and use hit to determine new target position
        if (Physics.Raycast(ray, out hit) && Input.GetMouseButton(0))
        {
            goal = new Vector3(hit.point.x, this.transform.position.y, hit.point.z);
            myMoveState = MoveState.Moving;
        }

        //Direction of this transform
        Vector3 direction = goal - transform.position;

        //if Distance too high
        if (Vector3.Distance(transform.position, goal) > accuracy)
        {
            this.transform.rotation = Quaternion.Slerp(this.transform.rotation, 
                                                       Quaternion.LookRotation(direction), 
                                                       Time.deltaTime * rotSpeed);
            this.transform.Translate(0, 0, speed * Time.deltaTime);
        }

        //Made it to the goal
        if (Vector3.Distance(transform.position, goal) <= accuracy && myMoveState == MoveState.Moving)
        {
            Debug.Log($"{this} has made it to their goal!");
            
            StartCoroutine("Dance");

            myMoveState = MoveState.Waiting;
        }

    }

    IEnumerator Dance()
    {
        Debug.Log("Dancing");
        goal = new Vector3(this.transform.position.x,
                               this.transform.position.y + 5,
                               this.transform.position.z);
        this.transform.localScale *= 1.50f;
        this.transform.Rotate(0, 0, 360);
        yield return new WaitForSeconds(2f);
        this.transform.localScale /= 1.50f;
        goal = new Vector3(this.transform.position.x + Random.Range(-10, 10), 
                            0.5f, 
                           this.transform.position.z + Random.Range(-10, 10));
        this.transform.rotation = Quaternion.Euler(0,0,0);
        yield return null;
    }

}
