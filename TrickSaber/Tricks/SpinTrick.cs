using System;
using System.Collections;
using UnityEngine;

namespace TrickSaber.Tricks
{
    public class SpinTrick : Trick
    {
        private bool _isVelocityDependent;
        private Transform _saberModelTransform;
        private float _spinSpeed;
        private float _largestSpinSpeed;
        private float _finalSpinSpeed;

        public override TrickAction TrickAction => TrickAction.Spin;

        public override void OnInit()
        {
            _saberModelTransform = SaberTrickModel.OriginalSaberModel.transform;
            _isVelocityDependent = PluginConfig.Instance.IsSpeedVelocityDependent;
        }

        public override void OnTrickStart()
        {
            _largestSpinSpeed = 0;
            if (_isVelocityDependent)
            {
                var angularVelocity = MovementController.GetAverageAngularVelocity();
                _spinSpeed = Math.Abs(angularVelocity.x) + Math.Abs(angularVelocity.y);
                angularVelocity = Quaternion.Inverse(MovementController.ControllerRotation) * angularVelocity;
                if (angularVelocity.x < 0) _spinSpeed *= -1;
            }
            else
            {
                var speed = 30;
                if (PluginConfig.Instance.SpinDirection == SpinDir.Backward.ToString()) speed *= -1;
                _spinSpeed = speed;
            }

            _spinSpeed *= PluginConfig.Instance.SpinSpeed;
        }

        void Update()
        {
            _finalSpinSpeed = _spinSpeed;
            if (!_isVelocityDependent) _finalSpinSpeed *= (float) Math.Pow(Value, 3);
            if (Math.Abs(_finalSpinSpeed) > Math.Abs(_largestSpinSpeed)) _largestSpinSpeed = _finalSpinSpeed;
            _saberModelTransform.Rotate(Vector3.right * _finalSpinSpeed);
        }

        #region Rotation-end Coroutines
        private IEnumerator LerpToOriginalRotation()
        {
            var rot = _saberModelTransform.localRotation;
            while (Quaternion.Angle(rot, Quaternion.identity) > 5f)
            {
                rot = Quaternion.Lerp(rot, Quaternion.identity, Time.deltaTime * 20);
                _saberModelTransform.localRotation = rot;
                yield return new WaitForEndOfFrame();
            }

            _saberModelTransform.localRotation = Quaternion.identity;
            Reset();
        }

        private IEnumerator CompleteRotation()
        {
            var minSpeed = 8;
            var largestSpinSpeed = _largestSpinSpeed;

            if (Mathf.Abs(largestSpinSpeed) < minSpeed)
            {
                largestSpinSpeed = largestSpinSpeed < 0 ? -minSpeed : minSpeed;
            }

            var threshold = Mathf.Abs(largestSpinSpeed) + 0.1f;
            var angle = Quaternion.Angle(_saberModelTransform.localRotation, Quaternion.identity);

            while (angle > threshold)
            {
                _saberModelTransform.Rotate(Vector3.right * largestSpinSpeed);
                angle = Quaternion.Angle(_saberModelTransform.localRotation, Quaternion.identity);
                yield return new WaitForEndOfFrame();
            }

            _saberModelTransform.localRotation = Quaternion.identity;
            Reset();
        }
        #endregion

        public override void OnTrickEndRequested()
        {
            StartCoroutine(PluginConfig.Instance.CompleteRotationMode ? CompleteRotation() : LerpToOriginalRotation());
        }

        public override void OnTrickEndImmediately()
        {
            _saberModelTransform.localRotation = Quaternion.identity;
            Reset();
        }
    }
}