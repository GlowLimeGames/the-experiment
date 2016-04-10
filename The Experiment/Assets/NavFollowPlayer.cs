using UnityEngine;
using System.Collections;

public class NavFollowPlayer : MonoBehaviour
{
    public GameObject target;
    public bool follow = false;

    private Animator anim;
    private NavMeshAgent nav;
    private Vector3 prevPosition;

    void Start()
    {
        anim = GetComponent<Animator>();
        nav = GetComponent<NavMeshAgent>();
        prevPosition = transform.position;
    }

    void Update()
    {
        nav.SetDestination(target.transform.position);

        float currentSpeed = (transform.position - prevPosition).magnitude / Time.deltaTime;
        prevPosition = transform.position;

        if (currentSpeed > 0.01f)
        {
            Vector3 targetDir = target.transform.position - transform.position;
            Vector3 newDir = Vector3.RotateTowards(transform.forward, targetDir, 1.5f * Time.deltaTime, 0.0F);
            transform.rotation = Quaternion.LookRotation(newDir);
            anim.SetBool("Walking", true);
        }
        else
        {
            anim.SetBool("Walking", false);
        }
    }
}
