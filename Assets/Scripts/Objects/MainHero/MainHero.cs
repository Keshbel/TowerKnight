using UnityEngine;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
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
    public float inputType; //скорость текущего передвижения по горизонтальной оси
    [Range(0, 100f)] public float speed = 10f; //контроль скорости
    private Vector2 _moveVelocity;
    
    [Header("Attack")]
    public bool isAttack;
    [Range(0, 100f)] public float jumpForce;
    [Range(0, 100f)] public float jumpTime;
    public AudioSource dashSound;
    private TweenerCore<Vector3, Vector3, VectorOptions> _tweenVertical;

    [Header("Positions")]
    public Transform startPosition;
    public Transform fallPosition;

    public GameObject pickupHeartEffect;
    public GameObject brokenHeartEffect;
    
    // Start is called before the first frame update
    void Start()
    {
        //DOTween.SetTweensCapacity(10000, 50);
        StartJumpScript.StartJump += StartJump;
        
        /*if (Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.WindowsEditor)*/
        //inputType = 
        /*if (Application.platform != RuntimePlatform.Android &&
            Application.platform != RuntimePlatform.IPhonePlayer) return;
        inputType = Input.acceleration.x;
        Debug.Log("Игра на мобильном устройстве");*/
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.transform.position.y - transform.position.y >= 100)
            return;
        
        if (other.gameObject.CompareTag("SafeObject"))
        {
            MoveToStartPosition();
            if (other.gameObject.GetComponent<Buff>())
            {
                health.ChangeHealth(1);
                Instantiate(pickupHeartEffect, transform);
            }
        }
        
        if (other.gameObject.CompareTag("DangerousObject"))
        {
            GiveDamage(1);
            MoveToStartPosition();
        }

        if (other.gameObject.CompareTag("BottomBorder"))
        {
            GiveDamage(1);
            MoveToStartPosition();
        }
    }
    
    // Update is called once per frame
    void Update()
    {
        #if UNITY_STANDALONE
        horizontalMove = Input.GetAxisRaw("Horizontal") * speed;
        #endif
        
        #if UNITY_ANDROID || UNITY_IOS
        horizontalMove = Input.acceleration.x * speed;
        #endif
        
        if(Input.GetAxis("Jump") > 0 && isMovement)
            Attack();

        #region FlipFace
        if (horizontalMove < 0 && facingRight)
        {
            FlipFace();
        }
        if (horizontalMove > 0 && !facingRight)
        {
            FlipFace();
        }
        #endregion

        if (health.currentHealth <= 0)
            SceneManager.LoadScene(1);

        /*if (transform.localPosition.x - Screen.width > 200 || transform.localPosition.x + Screen.width < 200
                                                           || transform.localPosition.y - Screen.height > 200
                                                           || transform.localPosition.y + Screen.height < 200)
        {
            transform.localPosition = Vector3.zero;
            isAttack = false;
            isMovement = true;
        }*/
    }
    
    private void FixedUpdate()
    {
        if (isMovement)
        {
            _moveVelocity = new Vector2(horizontalMove, rb.velocity.y);
            rb.velocity = _moveVelocity;
        }
    }
    
    private void OnDestroy()
    {
        _tweenVertical.Kill();
        StartJumpScript.StartJump -= StartJump;
    }
    
    private void MoveToStartPosition()
    {
        Invoke(nameof(IsAttackFalse), 0.1f);
        rb.Sleep();
        _tweenVertical?.Kill();
        _tweenVertical = transform.DOMoveY(startPosition.localPosition.y, jumpTime). 
            OnComplete(() =>
            {
                rb.WakeUp();
            });
        animator.SetTrigger("Jump");
    }

    private void Attack()
    {
        if (!isAttack)
        {
            dashSound.Play();
            isAttack = true;
            rb.WakeUp();
            _tweenVertical?.Kill();
            _tweenVertical = transform.DOMoveY(fallPosition.position.y, jumpTime * 1.5f);
            animator.SetTrigger("Attack");
        }
    }

    public void GiveDamage(int value)
    {
        Instantiate(brokenHeartEffect, transform);
        health.ChangeHealth(-value);
    }
    
    private void FlipFace()
    {
        facingRight = !facingRight;

        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    private void StartJump()
    {
        isAttack = false;
        isMovement = true;
        _tweenVertical?.Kill();
        _tweenVertical = transform.DOMoveY(startPosition.position.y, jumpTime*2). 
            OnComplete(() => rb.Sleep());
        animator.SetTrigger("Jump");
    }

    private void IsAttackFalse()
    {
        isAttack = false;
    }
}
