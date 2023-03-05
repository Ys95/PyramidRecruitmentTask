using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;
using UnityEngine.EventSystems;

namespace PyramidRecruitmentTask.Etc
{
    public class UIHoverTween : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private float _scaleChangePercent;
        [SerializeField] private float _duration;
        [SerializeField] private Ease  _ease;

        private Vector3?                                     _initialScale;
        private TweenerCore<Vector3, Vector3, VectorOptions> _hoverTween;

        private void OnDisable()
        {
            if (_initialScale.HasValue)
            {
                transform.localScale = _initialScale.Value;
            }

            if (_hoverTween.IsActive())
            {
                _hoverTween.Kill();
                _hoverTween = null;
            }
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (!_initialScale.HasValue)
            {
                _initialScale = transform.localScale;
            }
            
            if (_hoverTween.IsActive())
            {
                _hoverTween.Kill();
            }

            Vector3? newScale = _initialScale * _scaleChangePercent;
            _hoverTween = transform.DOScale(newScale.Value, _duration);
            _hoverTween.SetEase(_ease);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (!_initialScale.HasValue)
            {
                _initialScale = transform.localScale;
            }
            
            if (_hoverTween.IsActive())
            {
                _hoverTween.Kill();
            }

            _hoverTween = transform.DOScale(_initialScale.Value, _duration);
            _hoverTween.SetEase(_ease);
        }
    }
}