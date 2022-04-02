using System.Threading.Tasks;
using SiraUtil;
using SiraUtil.Logging;
using UnityEngine;
using SiraUtil.Sabers;
using SiraUtil.Tools;
using TrickSaber.Configuration;
using Zenject;
using Object = UnityEngine.Object;

namespace TrickSaber
{
    public class SaberTrickModel
    {
        public Rigidbody Rigidbody { get; private set; }
        public GameObject OriginalSaberModel { get; private set; }
        public GameObject TrickModel { get; private set; }

        private readonly PluginConfig _config;
        private readonly SiraSaberFactory _saberFactory;
        private readonly ColorManager _colorManager;
        private readonly SiraLog _logger;
        private SiraSaber _siraSaber;
        private Transform _saberTransform;

        private readonly bool _isMultiplayer;

        private SaberTrickModel(PluginConfig config, SiraSaberFactory saberFactory, ColorManager colorManager, [InjectOptional] MultiplayerPlayersManager multiplayerPlayersManager, SiraLog logger)
        {
            _config = config;
            _saberFactory = saberFactory;
            _colorManager = colorManager;
            _logger = logger;

            _isMultiplayer = multiplayerPlayersManager != null;
        }

        public async Task<bool> Init(Saber saber)
        {
            _siraSaber = _saberFactory.Spawn(saber.saberType);

            OriginalSaberModel = await GetSaberModel(saber);

            if (!OriginalSaberModel)
            {
                Object.DestroyImmediate(_siraSaber.gameObject);
                return false;
            }

            _siraSaber.SetColor(_colorManager.ColorForSaberType(saber.saberType));

            TrickModel = _siraSaber.gameObject;
            _saberTransform = _siraSaber.transform;

            TrickModel.name = $"TrickModel {saber.saberType}";
            AddRigidbody(TrickModel);

            TrickModel.SetActive(false);

            if (!_config.HitNotesDuringTrick || _isMultiplayer)
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
            //return saber.GetComponentInChildren<SaberModelController>()?.gameObject;
            SaberModelController smc = null;

            var timeout = 2000;
            var interval = 300;
            var time = 0;

            while (!smc)
            {
                smc = saber.GetComponentInChildren<SaberModelController>();

                if (smc) return smc.gameObject;

                if (time > timeout) return null;

                time += interval;
                await Task.Delay(interval);
            }

            return null;
        }
    }
}
