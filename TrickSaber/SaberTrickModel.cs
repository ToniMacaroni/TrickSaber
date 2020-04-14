using UnityEngine;

namespace TrickSaber
{
    public class SaberTrickModel
    {
        public GameObject OriginalSaberModel;
        public Rigidbody Rigidbody;
        public GameObject TrickModel;

        public SaberTrickModel(GameObject SaberModel)
        {
            OriginalSaberModel = SaberModel;
            TrickModel = Object.Instantiate(SaberModel);
            AddRigidbody();
            TrickModel.transform.SetParent(GameObject.Find("VRGameCore").transform);
            TrickModel.SetActive(false);
        }

        public void AddRigidbody()
        {
            Rigidbody = TrickModel.AddComponent<Rigidbody>();
            Rigidbody.useGravity = false;
            Rigidbody.isKinematic = true;
            Rigidbody.detectCollisions = false;
            Rigidbody.maxAngularVelocity = 800;
            Rigidbody.interpolation = RigidbodyInterpolation.Interpolate;
        }

        public void ChangeToTrickModel()
        {
            TrickModel.SetActive(true);
            TrickModel.transform.position = OriginalSaberModel.transform.position;
            TrickModel.transform.rotation = OriginalSaberModel.transform.rotation;
            OriginalSaberModel.SetActive(false);
        }

        public void ChangeToActualSaber()
        {
            OriginalSaberModel.SetActive(true);
            TrickModel.SetActive(false);
        }
    }
}