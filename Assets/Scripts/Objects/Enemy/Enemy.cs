using UnityEngine;

[RequireComponent(typeof(Health))]
public abstract class Enemy : ObjectScript
{
    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        //InitializeComponents
        health = GetComponent<Health>();
    }

    public override void Death()
    {
        if (audioSourceOuf != null)
            audioSourceOuf.Play();
        if(deathEffect != null)
            deathEffect.Play();
        rig2D.AddForce(-transform.up * jumpForce, ForceMode2D.Impulse);
        ProgressData.AddProgressPoint(1);
        isDeath = true;
    }
}
