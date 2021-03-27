using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridWP : MonoBehaviour
{
    public Node[,] grid;

    public GameObject prefabWP;
    public Material goalMat;
    public Material wallMat;

    private Vector3 goal;
    private float speed = 10f;
    private float accuracy = 0.5f;
    private float rotSpeed = 10f;


    private List<Node> path = new List<Node>();
    private Node startNode;
    private Node endNode;
    private int curNode;
    private int gridSpacing = 5;

    private void Start()
    {
        //Set up this agent with random properties
        SetAgentVals();

        //Create Grid
        grid = new Node[,]
        {
            {
                new Node(),
                new Node(),
                new Node(false),
                new Node(false),
                new Node(),
                new Node(),
                new Node(false),
                new Node(false),
                new Node(false),
                new Node(),
                new Node(),
                new Node(false)
            },
            {
                new Node(false),
                new Node(),
                new Node(),
                new Node(),
                new Node(false),
                new Node(),
                new Node(),
                new Node(),
                new Node(false),
                new Node(),
                new Node(),
                new Node()
            },
            {
                new Node(false),
                new Node(),
                new Node(),
                new Node(false),
                new Node(),
                new Node(),
                new Node(),
                new Node(false),
                new Node(false),
                new Node(),
                new Node(),
                new Node(false)
            },
            {
                new Node(),
                new Node(false),
                new Node(),
                new Node(false),
                new Node(),
                new Node(false),
                new Node(),
                new Node(),
                new Node(false),
                new Node(),
                new Node(),
                new Node()
            },
            {
                new Node(false),
                new Node(),
                new Node(),
                new Node(false),
                new Node(false),
                new Node(),
                new Node(),
                new Node(false),
                new Node(),
                new Node(),
                new Node(),
                new Node(false)
            },
            {
                new Node(false),
                new Node(),
                new Node(),
                new Node(false),
                new Node(),
                new Node(false),
                new Node(),
                new Node(),
                new Node(),
                new Node(),
                new Node(false),
                new Node(false)
            },
            {
                new Node(),
                new Node(false),
                new Node(),
                new Node(false),
                new Node(),
                new Node(false),
                new Node(false),
                new Node(),
                new Node(false),
                new Node(),
                new Node(false),
                new Node()
            },
            {
                new Node(),
                new Node(false),
                new Node(),
                new Node(false),
                new Node(),
                new Node(),
                new Node(),
                new Node(false),
                new Node(false),
                new Node(),
                new Node(false),
                new Node(false)
            },
            {
                new Node(false),
                new Node(false),
                new Node(),
                new Node(),
                new Node(),
                new Node(false),
                new Node(false),
                new Node(false),
                new Node(),
                new Node(),
                new Node(),
                new Node(false)
            },
            {
                new Node(),
                new Node(),
                new Node(false),
                new Node(false),
                new Node(),
                new Node(false),
                new Node(false),
                new Node(),
                new Node(),
                new Node(),
                new Node(),
                new Node(false)
            },
            {
                new Node(false),
                new Node(),
                new Node(),
                new Node(false),
                new Node(),
                new Node(false),
                new Node(false),
                new Node(),
                new Node(),
                new Node(),
                new Node(),
                new Node()
            },
            {
                new Node(false),
                new Node(),
                new Node(),
                new Node(),
                new Node(),
                new Node(),
                new Node(),
                new Node(false),
                new Node(false),
                new Node(),
                new Node(),
                new Node(false)
            },
            {
                new Node(),
                new Node(false),
                new Node(false),
                new Node(false),
                new Node(),
                new Node(),
                new Node(),
                new Node(false),
                new Node(),
                new Node(false),
                new Node(),
                new Node()
            },
            {
                new Node(false),
                new Node(false),
                new Node(false),
                new Node(),
                new Node(),
                new Node(false),
                new Node(),
                new Node(),
                new Node(),
                new Node(),
                new Node(false),
                new Node(false)
            },
            {
                new Node(),
                new Node(false),
                new Node(),
                new Node(false),
                new Node(),
                new Node(),
                new Node(),
                new Node(),
                new Node(false),
                new Node(),
                new Node(),
                new Node(false)
            },
            {
                new Node(),
                new Node(),
                new Node(false),
                new Node(),
                new Node(),
                new Node(),
                new Node(),
                new Node(false),
                new Node(),
                new Node(false),
                new Node(),
                new Node(false)
            },
            {
                new Node(),
                new Node(false),
                new Node(false),
                new Node(false),
                new Node(false),
                new Node(),
                new Node(),
                new Node(false),
                new Node(),
                new Node(false),
                new Node(),
                new Node(false)
            },
            {
                new Node(),
                new Node(false),
                new Node(false),
                new Node(),
                new Node(),
                new Node(false),
                new Node(),
                new Node(),
                new Node(),
                new Node(false),
                new Node(),
                new Node()
            },
            {
                new Node(false),
                new Node(),
                new Node(false),
                new Node(false),
                new Node(),
                new Node(),
                new Node(),
                new Node(),
                new Node(false),
                new Node(false),
                new Node(),
                new Node()
            },
            {
                new Node(),
                new Node(),
                new Node(false),
                new Node(false),
                new Node(),
                new Node(),
                new Node(false),
                new Node(),
                new Node(),
                new Node(),
                new Node(),
                new Node()
            }
        };

        //Initialize Grid Points

        for (int i = 0; i < grid.GetLength(0); i++)
        {
            for (int j = 0; j < grid.GetLength(1); j++)
            {
                grid[i, j].WP = Instantiate(prefabWP, 
                                            new Vector3(i * gridSpacing, 
                                                        this.transform.position.y, 
                                                        j * gridSpacing), 
                                            Quaternion.identity);
                
                if (!grid[i, j].Walkable)
                {
                    grid[i, j].WP.GetComponent<Renderer>().material = wallMat;
                }
                else
                {
                    grid[i, j].Neighbors = GetAdjacentNodes(grid, i, j);
                }
            }
        }

        startNode = grid[0, 0];
        endNode = grid[grid.GetLength(0)-1, grid.GetLength(1)-1];
        startNode.Walkable = true;
        endNode.WP.GetComponent<Renderer>().material = goalMat;

        this.transform.position = new Vector3(startNode.WP.transform.position.x, 
                                                this.transform.position.y, 
                                                startNode.WP.transform.position.z);

    }

    private void SetAgentVals()
    {
        this.speed = Random.Range(5, 10);
        this.accuracy = Random.Range(0.2f, 0.8f);
        this.rotSpeed = Random.Range(3f, 15f);
    }
    List<Node> BFS (Node start, Node end)
    {
        Queue<Node> toVisit = new Queue<Node>();
        List<Node> visited = new List<Node>();

        Node currentNode = start;
        currentNode.Depth = 0;
        toVisit.Enqueue(currentNode);

        List<Node> finalPath = new List<Node>();

        while(toVisit.Count > 0)
        {
            currentNode = toVisit.Dequeue();

            if (visited.Contains(currentNode)) continue;

            visited.Add(currentNode);

            if (currentNode.Equals(end))
            {
                while(currentNode.Depth != 0)
                {
                    foreach(Node n in currentNode.Neighbors)
                    {
                        if(n.Depth == currentNode.Depth - 1)
                        {
                            finalPath.Add(currentNode);
                            currentNode = n;
                            break;
                        }
                    }
                }
                finalPath.Reverse();
                break;
            }
            else
            {
                foreach(Node n in currentNode.Neighbors)
                {
                    if(!visited.Contains(n) && n.Walkable)
                    {
                        n.Depth = currentNode.Depth + 1;
                        toVisit.Enqueue(n);
                    }
                }
            }
        }

        return finalPath;
        
    }

    private List<Node> GetAdjacentNodes(Node[,] m, int i, int j)
    {
        List<Node> l = new List<Node>();

        //node up
        if(i - 1 >= 0)
        {
            if(m[i-1, j].Walkable)
            {
                l.Add(m[i - 1, j]);
            }
        }

        //node down
        if (i + 1 < m.GetLength(0))
        {
            if (m[i + 1, j].Walkable)
            {
                l.Add(m[i + 1, j]);
            }
        }

        //node left
        if (j-1 >= 0)
        {
            if(m[i, j - 1].Walkable)
            {
                l.Add(m[i, j - 1]);
            }
        }

        //node right
        if(j+1 < m.GetLength(1))
        {
            if(m[i, j + 1].Walkable)
            {
                l.Add(m[i, j + 1]);
            }
        }

        return l;
    }

    private void LateUpdate()
    {
        //Calculate Shortest Path
        if (Input.GetKeyDown(KeyCode.Return))
        {
            this.transform.position = new Vector3(startNode.WP.transform.position.x,
                                                    this.transform.position.y,
                                                    startNode.WP.transform.position.z);
            curNode = 0;

            //Use BFS
            path = BFS(startNode, endNode);
        }

        //if there is No path, do nothing
        if (path.Count == 0) return;

        //Set goal position
        goal = new Vector3(path[curNode].WP.transform.position.x,
                            this.transform.position.y,
                            path[curNode].WP.transform.position.z);
        //Set Direction
        Vector3 direction = goal - this.transform.position;

        //Move torward goal
        if (direction.magnitude > accuracy)
        {
            this.transform.rotation = Quaternion.Slerp(this.transform.rotation,
                                                        Quaternion.LookRotation(direction),
                                                        rotSpeed * Time.deltaTime);
            this.transform.Translate(0, 0, speed * Time.deltaTime);
        }
        else
        {
            if (curNode < path.Count - 1)
            {
                curNode++;
            }
        }

    }

    public class Node
    {
        //Initialize Variables
        private int depth;
        private bool walkable;
        private GameObject wp = new GameObject();
        private List<Node> neighbors = new List<Node>(); //Initialized by GetAdjacentNodes()

        //Getters and Setters
        public int Depth { get => depth; set => depth = value; }
        public bool Walkable { get => walkable; set => walkable = value; }

        public GameObject WP { get => wp; set => wp = value; }
        public List<Node> Neighbors { get => neighbors; set => neighbors = value; }

        //Constructor
        public Node()
        {
            this.depth = -1;
            this.walkable = true;
        }

        public Node(bool walkable)
        {
            this.depth = -1;
            this.walkable = walkable;
        }

        public override bool Equals(System.Object obj)
        {
            if (obj == null) return false;
            Node n = obj as Node;
            if ((System.Object)n == null) return false;
            if (this.wp.transform.position.x == n.WP.transform.position.x && this.wp.transform.position.z == n.WP.transform.position.z) return true;
            return false;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            return base.ToString();
        }
    }

}
