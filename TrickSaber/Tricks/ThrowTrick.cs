using System.Collections;
using IPA.Utilities;
using UnityEngine;

namespace TrickSaber
{
    public class ThrowTrick : Trick
    {
        private float _controllerSnapThreshold = 0.3f;
        private float _saberRotSpeed;
        private float _velocityMultiplier = 1;

        private GameObject _vrGameCore;

        public override TrickAction TrickAction => TrickAction.Throw;

        public override void OnTrickStart()
        {
            SaberTrickModel.ChangeToTrickModel();
            SaberTrickModel.Rigidbody.isKinematic = false;

            SaberTrickModel.Rigidbody.velocity = MovementController.GetAverageVelocity() * 3 * _velocityMultiplier;
            _saberRotSpeed = MovementController.SaberSpeed * _velocityMultiplier;
            if (MovementController.AngularVelocity.x > 0) _saberRotSpeed *= 150;
            else _saberRotSpeed *= -150;
            SaberTrickModel.Rigidbody.angularVelocity = Vector3.zero;
            SaberTrickModel.Rigidbody.AddRelativeTorque(Vector3.right * _saberRotSpeed, ForceMode.Acceleration);
        }

        public override void OnTrickUpdate()
        {
        }

        public override void OnTrickEndRequested()
        {
            SaberTrickModel.Rigidbody.velocity = Vector3.zero;
            StartCoroutine(ReturnSaber(PluginConfig.Instance.ReturnSpeed));
        }

        public override void OnInit()
        {
            _controllerSnapThreshold = PluginConfig.Instance.ControllerSnapThreshold;
            _velocityMultiplier = PluginConfig.Instance.ThrowVelocity;
            _vrGameCore = GameObject.Find("VRGameCore");
        }

        public IEnumerator ReturnSaber(float speed)
        {
            Vector3 position = SaberTrickModel.TrickModel.transform.localPosition;
            float distance = Vector3.Distance(position, MovementController.ControllerPosition);
            while (distance > _controllerSnapThreshold)
            {
                distance = Vector3.Distance(position, MovementController.ControllerPosition);
                if (distance > 1f)
                {
                    position = Vector3.Lerp(position, MovementController.ControllerPosition,
                        Time.deltaTime * (speed * 0.8f));
                }
                else
                {
                    position = Vector3.MoveTowards(position, MovementController.ControllerPosition, Time.deltaTime * speed);
                    Vector3 rotation = Quaternion.RotateTowards(SaberTrickModel.TrickModel.transform.rotation,
                                                                MovementController.ControllerRotation, Time.deltaTime * speed).eulerAngles;
                    Vector3 worldRotation = _vrGameCore.transform.TransformDirection(rotation);
                    SaberTrickModel.Rigidbody.MoveRotation(Quaternion.Euler(worldRotation));
                }

                Vector3 worldPosition = _vrGameCore.transform.TransformPoint(position);
                SaberTrickModel.Rigidbody.MovePosition(worldPosition);
                yield return new WaitForEndOfFrame();
            }

            ThrowEnd();
        }

        private void ThrowEnd()
        {
            SaberTrickModel.Rigidbody.isKinematic = true;
            SaberTrickModel.ChangeToActualSaber();
            Reset();
        }
    }
}