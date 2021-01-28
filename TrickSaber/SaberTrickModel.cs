using UnityEngine;
using IPA.Utilities;
using Zenject;

namespace TrickSaber
{
    public class SaberTrickModel
    {
        public static readonly string BasicSaberModelName = "BasicSaberModel(Clone)";

        public GameObject OriginalSaberModel;
        public Rigidbody Rigidbody;
        public GameObject TrickModel;

        public SaberTrickModel(GameObject SaberModel)
        {
            OriginalSaberModel = SaberModel;
            TrickModel = Object.Instantiate(SaberModel);
            AddRigidbody();
            TrickModel.transform.SetParent(GameObject.Find("VRGameCore").transform);
            if (SaberModel.name == BasicSaberModelName) FixBasicTrickSaber();
            TrickModel.SetActive(false);
        }

        public void FixBasicTrickSaber()
        {
            var originalGlow = OriginalSaberModel.GetComponentInChildren<SetSaberGlowColor>();
            var colorMgr = originalGlow.GetField<ColorManager, SetSaberGlowColor>("_colorManager");
            var saberType = originalGlow.GetField<SaberType, SetSaberGlowColor>("_saberType");

            var saberModelController = TrickModel.GetComponent<SaberModelController>();
            saberModelController.SetField("_colorManager", colorMgr);
            var glows = saberModelController.GetField<SetSaberGlowColor[], SaberModelController>("_setSaberGlowColors");
            foreach (var glow in glows)
            {
                glow.SetField("_colorManager", colorMgr);
            }
            var fakeGlows = saberModelController.GetField<SetSaberFakeGlowColor[], SaberModelController>(
                "_setSaberFakeGlowColors");
            foreach (var fakeGlow in fakeGlows)
            {
                fakeGlow.SetField("_colorManager", colorMgr);
            }

            //saberModelController.Init(TrickModel.transform.parent, saberModelController);
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
