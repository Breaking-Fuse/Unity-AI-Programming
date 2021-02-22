using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkWP : MonoBehaviour
{
    public Transform[] path;
    private int curNode;
    private Vector3 goal;
    private float speed = 4.0f;
    private float accuracy = 0.5f;
    private float rotSpeed = 4f;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        goal = new Vector3(path[curNode].position.x, 
                            path[curNode].position.y, 
                            path[curNode].position.z);
        Vector3 direction = goal - this.transform.position;

        if (direction.magnitude > accuracy)
        {
            this.transform.rotation = Quaternion.Slerp(this.transform.rotation, 
                                                        Quaternion.LookRotation(direction), 
                                                        rotSpeed * Time.deltaTime);
            this.transform.Translate(0, 0, speed * Time.deltaTime);
        }
        else
        {
            if (curNode < path.Length - 1)
            {
                curNode++;
            }
            else
            {
                curNode = Random.Range(1, path.Length - 1);
            }
        }
    }
}
