using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ErikaControls : MonoBehaviour
{
    NavMeshAgent agent;
    GameObject player;
    Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("MainCamera");
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (anim.GetBool("isDead"))
        {
            return;
        }

        transform.LookAt(new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z), Vector3.up);


        if ((Vector3.Distance(transform.position, player.transform.position) > 20)  && (!agent.pathPending) && (!agent.hasPath)) 
        {
            //Debug.Log("Set distance");
            anim.SetBool("isShootRange", false);
            anim.SetBool("isRunRange", true);
            agent.SetDestination(player.transform.position);
        }


        if (!agent.velocity.Equals(Vector3.zero))
        {
            if (!anim.GetBool("isRunRange"))
            {
                
                //Debug.Log("Bool running");
                anim.SetBool("isShootRange", false);
                anim.SetBool("isRunRange", true);
            }
        }

        if (!agent.pathPending)
        {
            if (Vector3.Distance(agent.destination, player.transform.position) > 10f)
            {
                /*Debug.Log(agent.destination);
                Debug.Log(player.transform.position);
                Debug.Log(Vector3.SqrMagnitude(agent.destination - player.transform.position));
                */
                agent.ResetPath();
                agent.SetDestination(player.transform.position);
            }
            if (agent.remainingDistance <= agent.stoppingDistance + 15)
            {
                //Debug.Log("Remaining distance");
                if ((!agent.hasPath) || (agent.velocity.sqrMagnitude == 0f)
                    || (agent.nextPosition == agent.transform.position))
                {
                    agent.ResetPath();
                    anim.SetBool("isRunRange", false);
                    anim.SetBool("isShootRange", true);
                }
            }
        }
    }
}
