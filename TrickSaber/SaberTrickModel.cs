using System.Threading.Tasks;
using UnityEngine;
using SiraUtil.Sabers;
using TrickSaber.Configuration;

namespace TrickSaber
{
    public class SaberTrickModel
    {
        public static readonly string BasicSaberModelName = "BasicSaberModel(Clone)";

        public Rigidbody Rigidbody { get; private set; }
        public GameObject OriginalSaberModel { get; private set; }
        public GameObject TrickModel { get; private set; }

        private readonly PluginConfig _config;
        private readonly SiraSaber.Factory _saberFactory;
        private SiraSaber _siraSaber;
        private Transform _saberTransform;

        private SaberTrickModel(PluginConfig config, SiraSaber.Factory saberFactory)
        {
            _config = config;
            _saberFactory = saberFactory;
        }

        public async Task<bool> Init(Saber saber)
        {
            OriginalSaberModel = await GetSaberModel(saber);

            if (!OriginalSaberModel)
            {
                return false;
            }

            _siraSaber = _saberFactory.Create();
            _siraSaber.ChangeType(saber.saberType);

            TrickModel = _siraSaber.gameObject;
            _saberTransform = _siraSaber.transform;

            TrickModel.name = $"TrickModel {saber.saberType}";
            AddRigidbody(TrickModel);

            _siraSaber.gameObject.SetActive(false);

            if (!_config.HitNotesDuringTrick)
            {
                _siraSaber.Saber.enabled = false;
            }

            return true;
        }

        public void AddRigidbody(GameObject saberObject)
        {
            Rigidbody = saberObject.AddComponent<Rigidbody>();
            Rigidbody.useGravity = false;
            Rigidbody.isKinematic = true;
            Rigidbody.detectCollisions = false;
            Rigidbody.maxAngularVelocity = 800;
            Rigidbody.interpolation = RigidbodyInterpolation.Interpolate;
        }

        public void ChangeToTrickModel()
        {
            TrickModel.SetActive(true);
            _saberTransform.position = OriginalSaberModel.transform.position;
            _saberTransform.rotation = OriginalSaberModel.transform.rotation;
            OriginalSaberModel.SetActive(false);
        }

        public void ChangeToActualSaber()
        {
            OriginalSaberModel.SetActive(true);
            TrickModel.SetActive(false);
        }

        private async Task<GameObject> GetSaberModel(Saber saber)
        {
            Transform result = null;

            var timeout = 2000;
            var interval = 300;
            var time = 0;

            while (result == null)
            {
                result = saber.transform.Find("SF Saber"); // Saber Factory
                if (!result) result = saber.transform.Find(BasicSaberModelName); // Default Saber

                if (time > timeout) return null;

                time += interval;
                await Task.Delay(interval);
            }

            return result.gameObject;
        }
    }
}
