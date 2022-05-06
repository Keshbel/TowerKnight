using System.Collections;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(Health))]
public abstract class Enemy : MonoBehaviour
{
    private ObjectBottomSpawner _objectBottomSpawner;
    private Rigidbody2D _rigidbody2D;
    public float jumpForce = 1.2f;
    [Header("Health")]
    public Health health;
    [Header("Death")]
    public bool isDeath;
    public ParticleSystem deathEffect;
    public AudioSource audioSourceOuf;

    private TweenerCore<Vector3, Vector3, VectorOptions> _tweenHorizontalMove;
    // Start is called before the first frame update
    void Start()
    {
        //InitializeComponents
        health = GetComponent<Health>();
        _objectBottomSpawner = FindObjectOfType<ObjectBottomSpawner>();
        _rigidbody2D = GetComponent<Rigidbody2D>();
        //StartRandomXPosition
        var pPos = transform.parent.position;
        gameObject.transform.localPosition = new Vector3(Random.Range(pPos.x -9, pPos.x + 9), this.gameObject.transform.localPosition.y); // x = -Screen.width/2+100, Screen.width/2-100
        //Jumping
        StartCoroutine(JumpingRoutine());
    }

    public void OnCollisionEnter2D(Collision2D other)
    {
        if (other.collider.CompareTag("Player")) //столкновение с игроком
        {
            health.ChangeHealth(-1);
            if (health.currentHealth <= 0 && !isDeath)
            {
                Death();
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
            Destroy(this.gameObject);
        }
    }
    
    private void OnDestroy()
    {
        _tweenHorizontalMove.Kill();
        StopCoroutine(JumpingRoutine());
    }

    private IEnumerator JumpingRoutine()
    {
        _rigidbody2D.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);
        var rand = Random.Range(0, 2);
        print(rand);
        if (rand == 0)
            _tweenHorizontalMove = transform.DOLocalMoveX(transform.localPosition.x + 1, 4f);
        else
        {
            _tweenHorizontalMove = transform.DOLocalMoveX(transform.localPosition.x - 1, 4f);
        }
        yield return new WaitForSeconds(3f);
        _rigidbody2D.gravityScale = 0.001f;
        yield return new WaitForSeconds(0.2f);
        for (int i = 0; i < 10; i++)
        {
            yield return new WaitForSeconds(0.01f);
            _rigidbody2D.gravityScale += 0.02f;
        }
    }
    

    private void Death()
    {
        if (audioSourceOuf != null)
            audioSourceOuf.Play();
        if(deathEffect != null)
            deathEffect.Play();
        _rigidbody2D.AddForce(-transform.up * jumpForce, ForceMode2D.Impulse);
        ProgressData.AddProgressPoint(1);
        isDeath = true;
    }
}
