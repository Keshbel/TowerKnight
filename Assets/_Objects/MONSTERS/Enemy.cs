using System;
using System.Collections;
using System.Collections.Generic;
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
    public Health health;

    private TweenerCore<Vector3, Vector3, VectorOptions> _tweenHorizontalMove;
    // Start is called before the first frame update
    void Start()
    {
        //InitializeComponents
        health = GetComponent<Health>();
        _objectBottomSpawner = FindObjectOfType<ObjectBottomSpawner>();
        _rigidbody2D = GetComponent<Rigidbody2D>();
        //StartRandomXPosition
        gameObject.transform.localPosition = new Vector3(Random.Range(-Screen.width/2+100, Screen.width/2-100), this.gameObject.transform.localPosition.y);
        //Jumping
        StartCoroutine(JumpingRoutine());
    }

    private void OnDestroy()
    {
        _tweenHorizontalMove.Kill();
        StopAllCoroutines();
    }

    private IEnumerator JumpingRoutine()
    {
        _rigidbody2D.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);
        var rand = Random.Range(0, 2);
        print(rand);
        if (rand == 0)
            _tweenHorizontalMove = transform.DOLocalMoveX(transform.localPosition.x + 100, 4f);
        else
        {
            _tweenHorizontalMove = transform.DOLocalMoveX(transform.localPosition.x - 100, 4f);
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

    public void OnCollisionEnter2D(Collision2D other)
    {
        if (other.collider.CompareTag("BottomBorder"))
        {
            _objectBottomSpawner.currentExistObjects.Remove(this.gameObject);
            if (gameObject.CompareTag("SafeObject"))
                _objectBottomSpawner.countSafeObject--;
            else
            {
                _objectBottomSpawner.countDangerousObject--;
            }
            Destroy(this.gameObject);
        }

        if (other.collider.CompareTag("Player"))
        {
            health.ChangeHealth(-1);
            if (health.currentHealth <= 0)
                _rigidbody2D.AddForce(-transform.up * jumpForce, ForceMode2D.Impulse);
        }
    }
}
