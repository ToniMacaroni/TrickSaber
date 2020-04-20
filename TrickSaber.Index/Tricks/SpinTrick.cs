using System;
using System.Collections;
using TrickSaber.Index;
using UnityEngine;

namespace TrickSaber.Tricks
{
    public class SpinTrick : Trick
    {
        private bool _isVelocityDependent;
        private Transform _saberModelTransform;
        private float _spinSpeed;
        private float _finalSpinSpeed;

        public override TrickAction TrickAction => TrickAction.Spin;

        public override void OnInit()
        {
            _saberModelTransform = SaberTrickModel.OriginalSaberModel.transform;
            _isVelocityDependent = PluginConfig.Instance.IsSpeedVelocityDependent;
        }

        public override void OnTrickStart()
        {
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

        public override void OnTrickUpdate()
        {
            _finalSpinSpeed = _spinSpeed;
            if (!_isVelocityDependent) _finalSpinSpeed *= (float) Math.Pow(Value, 3);
            _saberModelTransform.Rotate(Vector3.right, _finalSpinSpeed);
            _saberModelTransform.localRotation.ToAngleAxis(out var angle, out _);
            ModUI.Instance.SetText(angle.ToString());
        }

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
            _saberModelTransform.localRotation.ToAngleAxis(out var angle, out _);
            var isNegative = _finalSpinSpeed < 0;
            var multiplier = PluginConfig.Instance.SpinSpeed * 15;
            while (Quaternion.Angle(_saberModelTransform.localRotation, Quaternion.identity) > 2f)
            {
                angle = Mathf.Lerp(angle, 359.9f, Time.deltaTime * multiplier);
                if (isNegative) _saberModelTransform.localRotation = Quaternion.Inverse(Quaternion.AngleAxis(angle, Vector3.right));
                else _saberModelTransform.localRotation = Quaternion.AngleAxis(angle, Vector3.right);
                yield return new WaitForEndOfFrame();
            }

            _saberModelTransform.localRotation = Quaternion.identity;
            Reset();
        }

        public override void OnTrickEndRequested()
        {
            StartCoroutine(PluginConfig.Instance.CompleteRotationMode ? CompleteRotation() : LerpToOriginalRotation());
        }
    }
}