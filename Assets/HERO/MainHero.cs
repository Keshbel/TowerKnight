using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEditor.SearchService;
using UnityEngine.SceneManagement;

public class MainHero : MonoBehaviour
{
    [Header("Components")]
    public GameObject hero;  
    public Rigidbody2D rb;
    public Animator animator;
    public Health health;
    
    
    [Header("HorizontalMovement")] 
    public bool isMovement; // может ли двигаться персонаж?
    public bool facingRight; // повернут ли вправо?
    public float horizontalMove = 0f; //скорость текущего передвижения по горизонтальной оси
    [Range(0, 100f)] public float speed = 10f; //контроль скорости
    private Vector2 _moveVelocity;
    
    [Header("Attack")]
    public bool isAttack = false;
    [Range(0, 100f)] public float jumpForce;
    [Range(0, 100f)] public float jumpTime;
    public Sequence sequence;
    public TweenerCore<Vector3, Vector3, VectorOptions> tweenVertical;

    [Header("Positions")]
    public Transform startPosition;
    public Transform fallPosition;
    
    // Start is called before the first frame update
    void Start()
    {
        isMovement = true;
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        horizontalMove = Input.GetAxisRaw("Horizontal") * speed;
        //Walk();
        if (horizontalMove < 0 && facingRight)
        {
            FlipFace();
        }
        if (horizontalMove > 0 && !facingRight)
        {
            FlipFace();
        }
        
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Attack();
        }
        
        if (health.currentHealth <= 0)
            SceneManager.LoadScene(0);
    }

    private void FixedUpdate()
    {
        if (isMovement)
        {
            _moveVelocity = new Vector2(horizontalMove, rb.velocity.y);
            rb.velocity = _moveVelocity;
        }
    }
    
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.collider.CompareTag("RedEnemy"))
        {
            health.ChangeHealth(-1);
            MoveToStartPosition();
        }
        if (other.collider.CompareTag("Enemy"))
        {
            MoveToStartPosition();
        }

        if (other.collider.CompareTag("BottomBorder"))
        {
            SceneManager.LoadScene(0);
        }
    }

    private void OnDestroy()
    {
        tweenVertical.Kill();
    }

    /*public void Walk()
    {
        _moveVelocity.x = Input.GetAxis("Horizontal");
        rb.velocity = new Vector2(_moveVelocity.x * speed, rb.velocity.y);
    }*/

    private void MoveToStartPosition()
    {
        isAttack = false;
        tweenVertical?.Kill();
        tweenVertical = transform.DOMoveY(startPosition.position.y, jumpTime); //rb.DOMoveY(startPosition.position.y, jumpTime, false);
        animator.SetTrigger("Jump");
        
        rb.Sleep();
    }
    private void Attack()
    {
        if (!isAttack)
        {
            tweenVertical?.Kill();
            tweenVertical = transform.DOMoveY(fallPosition.position.y, jumpTime * 1.5f);
            animator.SetTrigger("Attack");
            //sequence.Append(rb.DOMoveY(fallPosition.position.y, jumpTime*1.5f));
        }
        isAttack = true;
    }

    public void FlipFace()
    {
        facingRight = !facingRight;

        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    
}
