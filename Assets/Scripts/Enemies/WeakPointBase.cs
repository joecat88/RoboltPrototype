using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Metodi richiamato dal player quando entra nel trigger WeakPoint */
public abstract class WeakPointBase : DropSteamTank, WeakPointInterface
{
    // TODO considerare di fare la classe abstract cosi da avere i metodi più flessibili, ad esempio controllo collider per chi non muorec con un colpo

    [SerializeField] protected int currentLife;
    [SerializeField] protected bool hasParentObj;
    protected Animator animator;
    public AudioSource pop;

    // references
    public EnemyDetectionIA enemyDetectionIA;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public abstract void HitEnemy();
 

    public int GetEnemyCurrentLife()
    {
        return currentLife;
    }

    public bool IsHeadWeakPoint() 
    {
        return true;
    }

    public void DestroyAnimation()
    {
        // per evitare che il nemico continui a colpire durante l'animazione di morte
        enemyDetectionIA.SetIsDying(true);

        GetComponent<BoxCollider>().enabled = false;
        // animazione morte con evento distroy alla fine
        animator.SetTrigger("ToDeath");
        pop.Play();
    }

    // Metodo chiamato dalla fine dell'animazione di distruzione
    public virtual void DestroyObj()
    {
        DropTank();
        if (hasParentObj)
            Destroy(transform.parent.gameObject, 0);
        else
            Destroy(gameObject, 0);

    }

    private void DropTank()
    {
        for (int i = 0; i < steamTankAmount; i++)
        {
            Vector3 spawnPosition = new Vector3(transform.position.x, transform.position.y + (0.4f + ((i + 1) / 2) ), transform.position.z);
            GameObject steamTank = Instantiate(steamTankPrefab, spawnPosition, Quaternion.identity);
        }
    }

}
