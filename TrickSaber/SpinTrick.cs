using System;
using System.Collections;
using UnityEngine;

namespace TrickSaber
{
    public class SpinTrick : Trick
    {
        private Transform _saberModelTransform;
        private Vector3 _spinVelocity;
        private bool _isVelocityDependent;

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
                _spinVelocity = new Vector3(Math.Abs(angularVelocity.x)+Math.Abs(angularVelocity.y), 0, 0);
                angularVelocity = Quaternion.Inverse(MovementController.ControllerRotation) * angularVelocity;
                if (angularVelocity.x < 0) _spinVelocity *= -1;
            }
            else
            {
                var speed = 6;
                if (PluginConfig.Instance.SpinDirection == SpinDir.Backward.ToString()) speed *= -speed;
                _spinVelocity = new Vector3(speed, 0, 0);
            }

            _spinVelocity *= PluginConfig.Instance.SpinSpeed;
        }

        public override void OnTrickUpdate()
        {
            var vel = _spinVelocity;
            if (!_isVelocityDependent) vel *= (float)Math.Pow(Value, 3);
            _saberModelTransform.Rotate(vel);
        }

        IEnumerator LerpToOriginalRotation()
        {
            var rot = _saberModelTransform.localRotation;
            while (Quaternion.Angle(rot, Quaternion.identity)>5f)
            {
                rot = Quaternion.Lerp(rot, Quaternion.identity, Time.deltaTime * 20);
                _saberModelTransform.localRotation = rot;
                yield return new WaitForEndOfFrame();
            }
            _saberModelTransform.localRotation = Quaternion.identity;
            Reset();
        }

        public override void OnTrickEndRequested()
        {
            StartCoroutine(LerpToOriginalRotation());
        }
    }
}