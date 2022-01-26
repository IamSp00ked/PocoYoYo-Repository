using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    private bool isAttacking=true;
    private bool ableToAttack;
    public float attackRangeZ;
    public float attackRangeX;
    public LayerMask layerMask;

    public RaycastHit[] hits;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        if(isAttacking)
        {
            hits = Physics.BoxCastAll(transform.position, transform.localScale / 2f, transform.forward, Quaternion.identity, attackRangeZ,layerMask);
        }
    }
    private void DealDamage()
    {

    }
}
