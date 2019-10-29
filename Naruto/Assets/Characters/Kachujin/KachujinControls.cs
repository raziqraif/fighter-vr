using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class KachujinControls : MonoBehaviour
{
    NavMeshAgent agent;
    GameObject player;
    Animator anim;
    string[] attackList = new string[4]{ "jumpKicking", "kicking", "punching", "roundhouseKicking" };

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
    }

    
    // Update is called once per frame
    void Update()
    {
        if (anim.GetBool("dying"))
        {
            return;
        }

        transform.LookAt(new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z), Vector3.up);


        if ((Vector3.Distance(transform.position, player.transform.position) > 4f) && (!agent.pathPending) && (!agent.hasPath))
        {
            Debug.Log("Set distance");
            setAnimationParameter("running");
            agent.SetDestination(player.transform.position);
        }


        if (!agent.velocity.Equals(Vector3.zero))
        {
            if (!anim.GetBool("running"))
            {
                //Debug.Log("Bool running");
                setAnimationParameter("running");
            }
        }

        if (!agent.pathPending)
        {
            if (agent.remainingDistance <= agent.stoppingDistance + 1f)
            {
                //Debug.Log("Kachujin: Remaining distance");
                if ((!agent.hasPath) || (agent.velocity.sqrMagnitude == 0f)
                    || (agent.nextPosition == agent.transform.position))
                {
                    agent.ResetPath();
                    setAnimationParameter(attackList[new System.Random().Next(0, 3)]);
                }
            }
            //Debug.Log("Kachujin" + anim.GetBool("running"));
             
       }
    }

    void setAnimationParameter(string str)
    {
        anim.SetBool("running", false);
        anim.SetBool("jumpKicking", false);
        anim.SetBool("kicking", false);
        anim.SetBool("punching", false);
        anim.SetBool("roundhouseKicking", false);
        anim.SetBool("dying", false);

        anim.SetBool(str, true);

    }
}
