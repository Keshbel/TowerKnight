using System.Collections;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;
using Random = UnityEngine.Random;

public class FirebollScript : MonoBehaviour
{
    private TweenerCore<Vector3, Vector3, VectorOptions> _tweenerCoreMoveX;
    private TweenerCore<Vector3, Vector3, VectorOptions> _tweenerCoreAlert;

    public GameObject fireExplosionPrefub;
    public AudioSource boomSound;

    public Transform alertImage;
    // Start is called before the first frame update
    void Start()
    {
        gameObject.transform.localPosition = new Vector3(Random.Range(-Screen.width/2+100, Screen.width/2-100), this.gameObject.transform.localPosition.y);
        AlertOn();
    }

    private void OnDestroy()
    {
        CountObjectsStatic.CountDangerousObject--;
        _tweenerCoreMoveX?.Kill();
        _tweenerCoreAlert?.Kill();
    }

    private void TranslateFireboll()
    {
        alertImage.gameObject.SetActive(false);
        _tweenerCoreMoveX?.Kill();
        _tweenerCoreMoveX = transform.DOLocalMoveY(Screen.height*1.5f, 4f)
            .OnComplete(() =>
            {
                Destroy(gameObject); 
            });
    }

    private void AlertOn()
    {
        _tweenerCoreAlert = alertImage.DOScale(0.9f, 0.5f).
            OnComplete(() => _tweenerCoreAlert = alertImage.DOScale(1f, 0.5f)).
            OnComplete(() => _tweenerCoreAlert = alertImage.DOScale(0.9f, 0.5f)).
            OnComplete(() => _tweenerCoreAlert = alertImage.DOScale(1f, 0.5f))
            .OnComplete(() => TranslateFireboll());
    }

    private IEnumerator OnCollisionEnter2D(Collision2D collision2D)
    {
        if (collision2D.collider.CompareTag("Player") || collision2D.collider.CompareTag("DangerousObject") ||
            collision2D.collider.CompareTag("SafeObject"))
        {
            GameObject firePuff = Instantiate(fireExplosionPrefub, gameObject.transform.position, Quaternion.identity);
            boomSound.Play();
            yield return new WaitForSeconds(0.05f);
            Destroy(gameObject);
        }
        Destroy(gameObject);
    }
}
