using System;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;

namespace PyramidRecruitmentTask.Etc
{
    public class FloatTween : MonoBehaviour
    {
        [SerializeField] private Transform _floatingTransform;
        [SerializeField] private float     _floatHeight;
        [SerializeField] private float     _floatDuration;
        [SerializeField] private Ease      _ease;

        private TweenerCore<Vector3, Vector3, VectorOptions> _movementTween;

        private void OnEnable()
        {
            _movementTween = _floatingTransform.DOLocalMoveY(transform.position.y + _floatHeight, _floatDuration);
            _movementTween.SetLoops(-1, LoopType.Yoyo);
            _movementTween.SetEase(_ease);
            _movementTween.Play();
        }

        private void OnDisable()
        {
            if (_movementTween.IsActive())
            {
                _movementTween.Kill();
                _movementTween = null;
            }
        }
    }
}