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
    public int playerDamage;
    public float knockbackFoce;

    public RaycastHit[] hits;
    public GameObject targetsFound;
    public List<GameObject> foundHealth = new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("amount of colliders inside: "+ hits.Length);
        isAttacking = Input.GetMouseButtonDown(0);
        if (isAttacking && hits.Length!=0)
            DealDamage();
        Debug.Log("attackerar: "+isAttacking);

    }

    private void FixedUpdate()
    {
            hits = Physics.BoxCastAll(transform.position, transform.localScale / 2f, transform.forward, Quaternion.identity, attackRangeZ,layerMask);
    }
    private void DealDamage()
    {
        if (hits.Length != 0)
        {
            foreach (RaycastHit hit in hits)
            {
                hit.collider.gameObject.GetComponentInParent<Health>().TakeDamage(playerDamage, knockbackFoce, gameObject);
            }
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(transform.localScale.x,transform.localScale.y, attackRangeZ));
        Gizmos.color = Color.green;
    }
}
