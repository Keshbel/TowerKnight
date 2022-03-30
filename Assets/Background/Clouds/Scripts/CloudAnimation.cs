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
        
        Vector3 _rotateCloud = new Vector3(0, 0, 1f);

        private bool _isExist = true;

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
            _isExist = false;
            _tweenerCoreMoveX?.Kill();
            _tweenerCoreScale?.Kill();
            _tweenerCoreRotate?.Kill();
            StopCoroutine(CloudAnimationRoutine());
            
            _tweenerCoreMoveX?.Kill();
            _tweenerCoreScale?.Kill();
            _tweenerCoreRotate?.Kill();
        }

        private IEnumerator CloudAnimationRoutine()
        {
            while (true)
            {
                if (!_isExist)
                    yield break;
                
                _tweenerCoreScale?.Kill();
                _tweenerCoreRotate?.Kill();
                _tweenerCoreScale = transform.DOScale(0.99f, 0.3f).OnComplete(() => _tweenerCoreScale = transform.DOScale(1f, 0.3f));
                _tweenerCoreRotate = transform.DORotate(_rotateCloud, 0.3f)
                    .OnComplete(() => _tweenerCoreRotate = transform.DORotate(-_rotateCloud, 0.3f));

                yield return new WaitForSeconds(0.6f);
            }
        }

        public void TranslateCloud(GameObject cloud, List<GameObject> currentCloudsExist)
        {
            _tweenerCoreMoveX?.Kill();
            _tweenerCoreMoveX = cloud.transform.DOLocalMoveX(Screen.width, 20f)
                .OnComplete(() => { 
                    currentCloudsExist.Remove(cloud);
                    Destroy(cloud); 
                });
        }
    }
}

