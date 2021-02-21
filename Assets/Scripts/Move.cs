using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
    // Initialize Variables
    public Vector3 target;
    float speed = 2f;
    float accuracy = .5f;
    float rotSpeed = 2f;

    // Start is called before the first frame update
    void Start()
    {
        //initialize target (this transform's position upon initialization)
        target = this.transform.position;
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
            target = new Vector3(hit.point.x, this.transform.position.y, hit.point.z);
            
        }

        //Direction of this transform
        Vector3 direction = target - transform.position;

        //if Distance too high
        if (Vector3.Distance(transform.position, target) > accuracy)
        {
            this.transform.rotation = Quaternion.Slerp(this.transform.rotation, 
                                                       Quaternion.LookRotation(direction), 
                                                       Time.deltaTime * rotSpeed);
            this.transform.Translate(0, 0, speed * Time.deltaTime);
        }

    }

}
