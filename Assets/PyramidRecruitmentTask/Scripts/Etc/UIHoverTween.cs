using System;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;
using UnityEngine.EventSystems;

namespace PyramidRecruitmentTask
{
    public class UIHoverTween : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private float _scaleChangePercent;
        [SerializeField] private float _duration;
        [SerializeField] private Ease  _ease;

        private Vector3? _initialScale;

        private void OnDisable()
        {
            if (_initialScale.HasValue)
            {
                transform.localScale = _initialScale.Value;
            }
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (!_initialScale.HasValue)
            {
                _initialScale = transform.localScale;
            }

            Vector3?                                     newScale = _initialScale * _scaleChangePercent;
            TweenerCore<Vector3, Vector3, VectorOptions> tween    = transform.DOScale(newScale.Value, _duration);
            tween.SetEase(_ease);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (!_initialScale.HasValue)
            {
                _initialScale = transform.localScale;
            }

            TweenerCore<Vector3, Vector3, VectorOptions> tween = transform.DOScale(_initialScale.Value, _duration);
            tween.SetEase(_ease);
        }
    }
}