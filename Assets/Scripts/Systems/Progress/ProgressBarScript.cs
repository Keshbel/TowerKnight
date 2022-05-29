using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBarScript : MonoBehaviour
{
    [SerializeField] private Image progressBarFilling;
    [SerializeField] private Image progressImage;
    [SerializeField] private ProgressScript progress;
    [SerializeField] private Gradient gradient;
    [SerializeField] private TextMeshProUGUI progressText;

    [Header("ColorsHeartChanged")]
    public Color defaultProgressColor;
    public Color changedProgressColor;

    [Header("Tweens")] 
    private TweenerCore<Vector3, Vector3, VectorOptions> _tweenScale;
    private TweenerCore<Color, Color, ColorOptions> _tweenColor;
    // Start is called before the first frame update
    private void Start()
    {
        defaultProgressColor = progressImage.color;
        OnProgressChanged(0);
    }

    private void Awake()
    {
        ProgressData.ProgressChanged += OnProgressChanged;
    }
    private void OnDestroy()
    {
        ProgressData.ProgressChanged -= OnProgressChanged;
        _tweenColor?.Kill();
        _tweenScale?.Kill();
    }
    
    private void OnProgressChanged(float valuePercantage)
    {
        if ( progressBarFilling.fillAmount < valuePercantage)
            ProgressPlusAnimationChanged();
        progressBarFilling.fillAmount = valuePercantage;
        progressBarFilling.color = gradient.Evaluate(valuePercantage);
        progressText.text = valuePercantage*100 + "%";
    }

    private void ProgressPlusAnimationChanged()
    {
        _tweenColor?.Kill();
        _tweenScale?.Kill();
        _tweenScale = progressImage.transform.DOScale(0.8f, 0.1f).OnComplete(() => progressImage.transform.DOScale(0.9f, 0.1f));
        _tweenColor = progressImage.DOColor(changedProgressColor, 0.2f).OnComplete(() => progressImage.DOColor(defaultProgressColor, 0.2f));
    }
}
