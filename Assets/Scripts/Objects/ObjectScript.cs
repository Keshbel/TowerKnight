using System.Collections;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

public class ObjectScript : MonoBehaviour
{
    public string objName;
    
    //components
    public Rigidbody2D rig2D;
    public Health health;

    [Header("Death")]
    public bool isDeath;
    public ParticleSystem deathEffect;
    public AudioSource audioSourceOuf;

    [Header("Jump")]
    public bool isJumping = true;
    public float jumpForce = 1.2f;
    private Tweener _tweenHorizontalMove;

    public virtual void Start()
    {
        if (isJumping)
        {
            if (!rig2D)
                rig2D = GetComponent<Rigidbody2D>();
            
            //StartRandomXPosition
            ObjectsUtility.RandomStartPosition(gameObject);
            StartCoroutine(JumpingRoutine());
        }
    }
    
    private void OnDestroy()
    {
        _tweenHorizontalMove?.Kill();
        StopAllCoroutines();
    }
    
    public virtual void OnCollisionEnter2D(Collision2D other)
    {
        if (other.collider.CompareTag("Player")) //столкновение с игроком
        {
            if (health)
            {
                health.ChangeHealth(-1);
                if (health.currentHealth <= 0)
                    Death();
            }
            else
            {
                Destroy(gameObject);
            }
        }
        
        if (other.collider.CompareTag("BottomBorder")) //столкновение с нижней границей
        {
            if (gameObject.CompareTag("SafeObject"))
                CountObjectsStatic.CountSafeObject--;
            else
            {
                CountObjectsStatic.CountDangerousObject--;
            }
            Destroy(gameObject);
        }
    }

    public virtual IEnumerator JumpingRoutine()
    {
        rig2D.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);
        var rand = Random.Range(0, 2);
        print(rand);
        if (rand == 0)
            _tweenHorizontalMove = transform.DOLocalMoveX(transform.localPosition.x + 1, 4f);
        else
        {
            _tweenHorizontalMove = transform.DOLocalMoveX(transform.localPosition.x - 1, 4f);
        }
        yield return new WaitForSeconds(3f);
        rig2D.gravityScale = 0.001f;
        yield return new WaitForSeconds(0.2f);
        for (int i = 0; i < 10; i++)
        {
            yield return new WaitForSeconds(0.01f);
            rig2D.gravityScale += 0.02f;
        }
    }
    
    public virtual void Death()
    {
        Destroy(gameObject);
    }
}
