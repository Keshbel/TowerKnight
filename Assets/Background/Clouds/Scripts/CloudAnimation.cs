using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;

namespace Clouds
{
    public class CloudAnimation : MonoBehaviour
    {
        private TweenerCore<Vector3, Vector3, VectorOptions> _tweenerCoreMoveX;

        private TweenerCore<Vector3, Vector3, VectorOptions> _tweenerCoreScale;
        private TweenerCore<Quaternion, Vector3, QuaternionOptions> _tweenerCoreRotate;
        
        Vector3 _rotateCloud = new Vector3(0, 0, 3f);

        // Start is called before the first frame update
        void Start()
        {
            if (transform != null)
                gameObject.transform.localPosition = new Vector3(this.gameObject.transform.localPosition.x,
                Random.Range(-Screen.height/2 + 300, Screen.height/2 - 300));
            StartCoroutine(CloudAnimationRoutine());
        }

        private void OnDestroy()
        {
            StopCoroutine(CloudAnimationRoutine());
            StopAllCoroutines();

            _tweenerCoreMoveX?.Complete();
            _tweenerCoreMoveX?.Kill();
            _tweenerCoreScale?.Complete();
            _tweenerCoreRotate?.Complete();
            _tweenerCoreScale?.Kill();
            _tweenerCoreRotate?.Kill();
        }

        private IEnumerator CloudAnimationRoutine()
        {
            _tweenerCoreScale?.Kill();
            _tweenerCoreRotate?.Kill();
            _tweenerCoreScale = transform.DOScale(0.95f, 1f).OnComplete(() => transform.DOScale(1f, 1f));
            _tweenerCoreRotate = transform.DORotate(_rotateCloud, 1f)
                .OnComplete(() => transform.DORotate(-_rotateCloud, 1f));

            yield return new WaitForSeconds(2f);
            
            _tweenerCoreScale?.Kill();
            _tweenerCoreRotate?.Kill();
            _tweenerCoreScale = transform.DOScale(0.95f, 1f).OnComplete(() => transform.DOScale(1f, 1f));
            _tweenerCoreRotate = transform.DORotate(_rotateCloud, 1f)
                .OnComplete(() => transform.DORotate(-_rotateCloud, 1f));
            
            yield return new WaitForSeconds(2f);
            
            _tweenerCoreScale?.Kill();
            _tweenerCoreRotate?.Kill();
            _tweenerCoreScale = transform.DOScale(0.95f, 1f).OnComplete(() => transform.DOScale(1f, 1f));
            _tweenerCoreRotate = transform.DORotate(_rotateCloud, 1f)
                .OnComplete(() => transform.DORotate(-_rotateCloud, 1f));
            // ReSharper disable once IteratorNeverReturns
        }

        public void TranslateCloud(GameObject cloud, List<GameObject> currentCloudsExist)
        {
            _tweenerCoreMoveX?.Kill();
            _tweenerCoreMoveX = cloud.transform.DOLocalMoveX(Screen.width * 0.5f, 10f, false)
                .OnComplete(() => { 
                    currentCloudsExist.Remove(cloud);
                    Destroy(cloud); 
                });
        }
    }
}

