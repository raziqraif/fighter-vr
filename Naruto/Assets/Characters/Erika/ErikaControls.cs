using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ErikaControls : MonoBehaviour
{
    NavMeshAgent agent;
    GameObject player;
    public GameObject arrow;
    Animator anim;

    float waitTime = 0f;
    
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
        if (anim.GetBool("isDead"))
        {
            return;
        }

        waitTime -= Time.deltaTime;

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
            else if (agent.remainingDistance <= agent.stoppingDistance + 15)
            {
                //Debug.Log("Remaining distance");
                if ((!agent.hasPath) || (agent.velocity.sqrMagnitude == 0f)
                    || (agent.nextPosition == agent.transform.position))
                {
                    agent.ResetPath();
                    anim.SetBool("isRunRange", false);
                    anim.SetBool("isShootRange", true);
                    if (waitTime <= 0)
                    {
                        ApplyForce();
                        waitTime = 1.5f;
                    }
                    
                }
            }
        }
    }

    void ApplyForce()
    {
        Debug.Log("Arrow shot.");
        GameObject newArrow = Instantiate(arrow, arrow.transform.position, arrow.transform.rotation);
        newArrow.GetComponent<MeshRenderer>().enabled = true;
        //newArrow.GetComponent<CapsuleCollider>().enabled = true;

        transform.LookAt(new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z), Vector3.up);

        Rigidbody targetBody = newArrow.GetComponent<Rigidbody>();

        targetBody.velocity = transform.TransformDirection(Vector3.forward) * 40;

        //targetBody.AddRelativeForce(transform.TransformDirection(Vector3.forward));
        

        //newArrow.GetComponent<MeshRenderer>().enabled = true;
        //newArrow.transform.rotation = Quaternion.Euler(0, 0, 0);
        //newArrow.transform.LookAt(player.transform);
        //transform.LookAt(new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z), Vector3.up);

        //newArrow.GetComponent<Rigidbody>().AddRelativeForce(newArrow.transform.forward * arrowShootForce);
        //newArrow.GetComponent <Rigidbody> ().AddRelativeForce((player.transform.position - arrow.transform.position));
    }

}
