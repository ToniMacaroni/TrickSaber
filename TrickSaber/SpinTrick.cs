using System.Collections;
using UnityEngine;

namespace TrickSaber
{
    public class SpinTrick : Trick
    {
        private float _currentRotation;
        private float _spinSpeedMultiplier;

        public override void OnConstruct()
        {
            _spinSpeedMultiplier = PluginConfig.Instance.SpinSpeed;
            if (PluginConfig.Instance.SpinDirection == SpinDir.Backward.ToString())
                _spinSpeedMultiplier = -_spinSpeedMultiplier;
        }

        public override void OnTrickStart()
        {
            _currentRotation = 0;
            SaberTrickManager.Controller.enabled = false;
        }

        public override void OnTrickUpdate()
        {
            SaberTrickManager.Saber.transform.localRotation = SaberTrickManager._controllerRotation;
            SaberTrickManager.Saber.transform.localPosition = SaberTrickManager._controllerPosition;
            _currentRotation += 18 * _spinSpeedMultiplier;
            SaberTrickManager.Saber.transform.Rotate(Vector3.right, _currentRotation);
        }

        public override void OnTrickEndRequested()
        {
            SaberTrickManager.Controller.enabled = true;
            Reset();
        }
    }
}