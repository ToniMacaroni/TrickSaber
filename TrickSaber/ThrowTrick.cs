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
        private float _saberRotSpeed;
        private float _controllerSnapThreshold = 0.3f;
        private float _velocityMultiplier = 1;

        public override void OnTrickStart()
        {
            SaberTrickManager._collider.enabled = false;
            SaberTrickManager.Rigidbody.isKinematic = false;
            SaberTrickManager.Rigidbody.velocity = SaberTrickManager._velocity * 2 * _velocityMultiplier;
            _saberRotSpeed = SaberTrickManager.SaberSpeed * 200 * _velocityMultiplier;
            SaberTrickManager.Controller.enabled = false;
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
            float distance = Vector3.Distance(position, SaberTrickManager._controllerPosition);
            while (distance > _controllerSnapThreshold)
            {
                distance = Vector3.Distance(position, SaberTrickManager._controllerPosition);
                if (distance > 1f)
                {
                    position = Vector3.Lerp(position, SaberTrickManager._controllerPosition, Time.deltaTime * (speed * 0.8f));
                }
                else
                {
                    position = Vector3.MoveTowards(position, SaberTrickManager._controllerPosition, Time.deltaTime * speed);
                }
                SaberTrickManager.Rigidbody.MovePosition(position);
                yield return new WaitForEndOfFrame();
            }
            ThrowEnd();
        }

        private void ThrowEnd()
        {
            SaberTrickManager.Rigidbody.isKinematic = true;
            SaberTrickManager._collider.enabled = true;
            SaberTrickManager.Controller.enabled = true;
            Reset();
        }
    }
}
