using System.Collections;
using System.ComponentModel;
using UnityEngine;

namespace TrickSaber.Tricks
{
    public class ThrowTrick : Trick
    {
        private float _controllerSnapThreshold = 0.3f;
        private float _saberRotSpeed;
        private float _velocityMultiplier = 1;

        public override TrickAction TrickAction => TrickAction.Throw;

        public override void OnTrickStart()
        {
            SaberTrickModel.ChangeToTrickModel();
            SaberTrickModel.Rigidbody.isKinematic = false;

            Vector3 finalVelocity = MovementController.GetAverageVelocity() * _velocityMultiplier;
            SaberTrickModel.Rigidbody.velocity = finalVelocity * 3;
            _saberRotSpeed = finalVelocity.magnitude;
            if (MovementController.AngularVelocity.x > 0) _saberRotSpeed *= 150;
            else _saberRotSpeed *= -150;
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
        }

        public IEnumerator ReturnSaber(float speed)
        {
            Vector3 position = SaberTrickModel.TrickModel.transform.position;
            var controllerPos = MovementController.ControllerPosition;
            float distance = Vector3.Distance(position, controllerPos);
            while (distance > _controllerSnapThreshold)
            {
                distance = Vector3.Distance(position, controllerPos);
                var direction = controllerPos - position;
                float force;
                if (distance < 1f) force = 10f;
                else force = speed * distance;
                force = Mathf.Clamp(force, 0, 200);
                SaberTrickModel.Rigidbody.velocity = direction.normalized * force;
                position = SaberTrickModel.TrickModel.transform.position;
                controllerPos = MovementController.ControllerPosition;
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