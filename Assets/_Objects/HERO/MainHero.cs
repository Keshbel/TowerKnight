using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEditor.SearchService;
using UnityEngine.SceneManagement;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

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
    public float horizontalMove; //скорость текущего передвижения по горизонтальной оси
    [Range(0, 100f)] public float speed = 10f; //контроль скорости
    private Vector2 _moveVelocity;
    
    [Header("Attack")]
    public bool isAttack = false;
    [Range(0, 100f)] public float jumpForce;
    [Range(0, 100f)] public float jumpTime;
    private TweenerCore<Vector3, Vector3, VectorOptions> _tweenVertical;

    [Header("Positions")]
    public Transform startPosition;
    public Transform fallPosition;
    
    // Start is called before the first frame update
    void Start()
    {
        DOTween.SetTweensCapacity(10000, 50);
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

        if (transform.localPosition.x - Screen.width > 200 || transform.localPosition.x + Screen.width < 200
                                                           || transform.localPosition.y - Screen.height > 200
                                                           || transform.localPosition.y + Screen.height < 200)
            transform.localPosition = Vector3.zero;
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
        if (other.transform.position.y - transform.position.y >= 50)
            return;
        
        if (other.collider.CompareTag("SafeObject"))
        {
            MoveToStartPosition();
        }
        
        if (other.collider.CompareTag("DangerousObject"))
        {
            health.ChangeHealth(-1);
            MoveToStartPosition();
        }

        if (other.collider.CompareTag("BottomBorder"))
        {
            SceneManager.LoadScene(0);
        }
    }

    private void OnDestroy()
    {
        _tweenVertical.Kill();
    }

    private void MoveToStartPosition()
    {
        isAttack = false;
        _tweenVertical?.Kill();
        _tweenVertical = transform.DOMoveY(startPosition.position.y, jumpTime). 
            OnComplete(() => rb.Sleep());
        animator.SetTrigger("Jump");
    }
    private void Attack()
    {
        if (!isAttack)
        {
            _tweenVertical?.Kill();
            _tweenVertical = transform.DOMoveY(fallPosition.position.y, jumpTime * 1.5f);
            animator.SetTrigger("Attack");
        }
        isAttack = true;
    }

    private void FlipFace()
    {
        facingRight = !facingRight;

        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }
}
