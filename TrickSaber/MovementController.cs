using System;
using UnityEngine;

namespace TrickSaber
{
    public class MovementController : MonoBehaviour
    {
        private Vector3[] _angularVelocityBuffer;
        private int _currentProbeIndex;

        private Vector3 _prevPos = Vector3.zero;
        private Quaternion _prevRot = Quaternion.identity;

        //Velocity calc
        private Vector3[] _velocityBuffer;
        public Vector3 AngularVelocity = Vector3.zero;
        public VRController Controller;

        public Vector3 ControllerPosition = Vector3.zero;
        public Quaternion ControllerRotation = Quaternion.identity;
        public SaberTrickManager SaberTrickManager;

        public Vector3 Velocity = Vector3.zero;
        public VRPlatformHelper VrPlatformHelper;
        public Vector3 LocalControllerPosition => Controller.gameObject.transform.localPosition;
        public Quaternion LocalControllerRotation => Controller.gameObject.transform.localRotation;

        public float SaberSpeed => Velocity.magnitude;

        private void Start()
        {
            _velocityBuffer = new Vector3[PluginConfig.Instance.VelocityBufferSize];
            _angularVelocityBuffer = new Vector3[PluginConfig.Instance.VelocityBufferSize];
        }

        private void Update()
        {
            if (Controller.enabled)
            {
                ControllerPosition = Controller.position;
                ControllerRotation = Controller.rotation;
                Velocity = (ControllerPosition - _prevPos) / Time.deltaTime;
                AngularVelocity = GetAngularVelocity(_prevRot, ControllerRotation);
                AddProbe(Velocity, AngularVelocity);
                _prevPos = ControllerPosition;
                _prevRot = ControllerRotation;
            }
        }

        private void AddProbe(Vector3 vel, Vector3 ang)
        {
            if (_currentProbeIndex > _velocityBuffer.Length - 1) _currentProbeIndex = 0;
            _velocityBuffer[_currentProbeIndex] = vel;
            _angularVelocityBuffer[_currentProbeIndex] = ang;
            _currentProbeIndex++;
        }

        public Vector3 GetAverageVelocity()
        {
            Vector3 avg = Vector3.zero;
            for (int i = 0; i < _velocityBuffer.Length; i++) avg += _velocityBuffer[i];

            return avg / _velocityBuffer.Length;
        }

        public Vector3 GetAverageAngularVelocity()
        {
            Vector3 avg = Vector3.zero;
            for (int i = 0; i < _velocityBuffer.Length; i++) avg += _angularVelocityBuffer[i];

            return avg / _velocityBuffer.Length;
        }

        private Vector3 GetAngularVelocity(Quaternion foreLastFrameRotation, Quaternion lastFrameRotation)
        {
            var q = lastFrameRotation * Quaternion.Inverse(foreLastFrameRotation);
            if (Math.Abs(q.w) > 1023.5f / 1024.0f)
                return new Vector3(0, 0, 0);
            float gain;
            if (q.w < 0.0f)
            {
                var angle = Math.Acos(-q.w);
                gain = (float) (-2.0f * angle / (Math.Sin(angle) * Time.deltaTime));
            }
            else
            {
                var angle = Math.Acos(q.w);
                gain = (float) (2.0f * angle / (Math.Sin(angle) * Time.deltaTime));
            }

            return new Vector3(q.x * gain, q.y * gain, q.z * gain);
        }
    }
}