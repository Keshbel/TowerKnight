using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundScript : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private MeshRenderer meshRenderer;

    private Vector2 _meshOffset;
    // Start is called before the first frame update
    void Start()
    {
        _meshOffset = meshRenderer.sharedMaterial.mainTextureOffset;

        gameObject.GetComponent<RectTransform>().sizeDelta.Set(Screen.width, Screen.height);
    }

    private void OnDisable()
    {
        meshRenderer.sharedMaterial.mainTextureOffset = _meshOffset;
    }

    // Update is called once per frame
    void Update()
    {
        var x = Mathf.Repeat(Time.time * speed, 1);
        var offset = new Vector2(x, _meshOffset.y);

        meshRenderer.sharedMaterial.mainTextureOffset = offset;
    }
}
