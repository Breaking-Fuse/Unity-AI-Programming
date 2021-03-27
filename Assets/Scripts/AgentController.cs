using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AgentController : MonoBehaviour
{
    //Knock Settings
    public float knockRadius = 12.0f;

    // Update is called once per frame
    void Update()
    {
        bool hasKnocked = false;
        if (Input.GetKeyUp(KeyCode.Space))
        {
            StartCoroutine(PlayKnock());
            hasKnocked = true;
        }
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, knockRadius);
        for (int i = 0; i < hitColliders.Length; i++)
        {
            if (hitColliders[i].tag == "Guard" && hasKnocked)
            {
                hitColliders[i].GetComponent<GuardController>().InvestigatePoint(this.transform.position);
                Debug.Log(i.ToString() + " heard a knock");
            }
        }


        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 100))
            {
                this.GetComponent<NavMeshAgent>().SetDestination(hit.point);
            }
        }       
    }


    IEnumerator PlayKnock()
    {
        AudioSource audio = GetComponent<AudioSource>();
        audio.Play();
        yield return new WaitForSeconds(audio.clip.length);
    }
}
