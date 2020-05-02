using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class ProjectileCollisionCheck : MonoBehaviour
{
    [SerializeField] LayerMask whatCanCollide;
    const int resultsSize = 5;
    Collider[] results = new Collider[resultsSize];
    BoxCollider myCollider;

    // Start is called before the first frame update
    void Start()
    {
        myCollider = GetComponent<BoxCollider>(); 
    }

    // Update is called once per frame
    void Update()
    {
        
        if (Physics.OverlapBoxNonAlloc(myCollider.bounds.center, myCollider.bounds.size, results, Quaternion.identity, whatCanCollide) > 0)
        {
            foreach (Collider collider in results)
            {
                if (collider)
                {
                    collider.GetComponent<MoveProjectile>().Hit();
                }
            }
        }
    }
}
