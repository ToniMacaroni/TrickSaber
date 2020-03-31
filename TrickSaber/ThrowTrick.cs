using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace TrickSaber
{
    public class ThrowTrick : Trick
    {
        public MovementController MovementController;

        private float _saberRotSpeed;
        private float _controllerSnapThreshold = 0.3f;
        private float _velocityMultiplier = 1;

        public override void OnTrickStart()
        {
            SaberTrickManager.Collider.enabled = false;
            SaberTrickManager.Rigidbody.isKinematic = false;
            SaberTrickManager.Rigidbody.velocity = MovementController.Velocity * 3 * _velocityMultiplier;
            _saberRotSpeed = MovementController.SaberSpeed * _velocityMultiplier;
            if (MovementController.AngularVelocity.x > 0) _saberRotSpeed *= 150;
            else _saberRotSpeed *= -150;
            SaberTrickManager.Controller.enabled = false;
            SaberTrickManager.Rigidbody.angularVelocity = Vector3.zero;
            SaberTrickManager.Rigidbody.AddRelativeTorque(Vector3.right * _saberRotSpeed, ForceMode.Acceleration);
        }

        public override void OnTrickUpdate()
        {
        }

        public override void OnTrickEndRequested()
        {
            SaberTrickManager.Rigidbody.velocity = Vector3.zero;
            StartCoroutine(ReturnSaber(PluginConfig.Instance.ReturnSpeed));
        }

        public override void OnConstruct()
        {
            _controllerSnapThreshold = PluginConfig.Instance.ControllerSnapThreshold;
            _velocityMultiplier = PluginConfig.Instance.ThrowVelocity;
        }

        public IEnumerator ReturnSaber(float speed)
        {
            //float velocitySpeed = speed * (1+_throwSpeed);
            Vector3 position = SaberTrickManager.Saber.transform.localPosition;
            float distance = Vector3.Distance(position, MovementController.ControllerPosition);
            while (distance > _controllerSnapThreshold)
            {
                distance = Vector3.Distance(position, MovementController.ControllerPosition);
                if (distance > 1f)
                {
                    position = Vector3.Lerp(position, MovementController.ControllerPosition, Time.deltaTime * (speed * 0.8f));
                }
                else
                {
                    position = Vector3.MoveTowards(position, MovementController.ControllerPosition, Time.deltaTime * speed);
                }
                SaberTrickManager.Rigidbody.MovePosition(position);
                yield return new WaitForEndOfFrame();
            }
            ThrowEnd();
        }

        private void ThrowEnd()
        {
            SaberTrickManager.Rigidbody.isKinematic = true;
            SaberTrickManager.Collider.enabled = true;
            SaberTrickManager.Controller.enabled = true;
            Reset();
        }
    }
}
