using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.XR;

namespace TrickSaber
{
    public class MovementController : MonoBehaviour
    {
        public VRController Controller;
        public SaberTrickManager SaberTrickManager;

        public Vector3 Velocity = Vector3.zero;
        public Vector3 AngularVelocity = Vector3.zero;
        public float SaberSpeed;

        private Vector3 _prevPos = Vector3.zero;
        private Vector3 _prevRot = Vector3.zero;

        public Vector3 ControllerPosition = Vector3.zero;
        public Quaternion ControllerRotation = Quaternion.identity;

        //Velocity calc
        private readonly Vector3[] _probedVelocities = new Vector3[8];
        private int _currentProbeIndex;

        void Start()
        {
        }

        private void Update()
        {
            (ControllerPosition, ControllerRotation) = GetTrackingPos();
            if (Controller.enabled)
            {
                var controllerPosition = Controller.position;
                var currentProbe = (controllerPosition - _prevPos) / Time.deltaTime;
                AddProbe(currentProbe);
                Velocity = GetAverageVelocity();
                SaberSpeed = currentProbe.magnitude;
                var euler = ControllerRotation.eulerAngles;
                AngularVelocity = (euler - _prevRot) / Time.deltaTime;
                _prevPos = controllerPosition;
                _prevRot = euler;
            }
        }

        void AddProbe(Vector3 vel)
        {
            if (_currentProbeIndex > _probedVelocities.Length - 1) _currentProbeIndex = 0;
            _probedVelocities[_currentProbeIndex] = vel;
            _currentProbeIndex++;
        }

        Vector3 GetAverageVelocity()
        {
            Vector3 avg = Vector3.zero;
            for (int i = 0; i < _probedVelocities.Length; i++)
            {
                avg += _probedVelocities[i];
            }

            return avg / _probedVelocities.Length;
        }

        private (Vector3, Quaternion) GetTrackingPos()
        {
            var success = SaberTrickManager.VrPlatformHelper.GetNodePose(Controller.node, Controller.nodeIdx, out var pos, out var rot);
            if (!success) return (new Vector3(-0.2f, 0.05f, 0f), Quaternion.identity);
            return (pos, rot);
        }
    }
}
