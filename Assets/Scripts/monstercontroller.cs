using UnityEngine;
using UnityEngine.AI;

public class MonsterController : MonoBehaviour
{
    public Camera cam;
    public NavMeshAgent monster;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                monster.SetDestination(hit.point);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //Look at other 

        transform.LookAt(other.transform.position);

        //Shoot Other

    }

    private void OnTriggerExit(Collider other)
    {
        //go back to path
    }
}
