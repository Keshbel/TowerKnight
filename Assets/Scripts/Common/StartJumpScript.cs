using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class StartJumpScript : MonoBehaviour
{
    [Header("GO")]
    public GameObject earth;
    public GameObject stars;
    [Header("IsStartJump?")]
    public static Action StartJump;
    public bool isStartJump;

    void Update()
    {
        if (!isStartJump)
            if (Input.anyKeyDown && Input.GetAxis("Jump") == 0)
                Jumping();
    }

    public void Jumping()
    {
        earth.transform.DOLocalMoveY(-900, 2f).OnComplete(()=> earth.SetActive(false));
        stars.transform.DOLocalMoveY(0, 1f);
        StartJump?.Invoke();
        isStartJump = true;
    }
}
