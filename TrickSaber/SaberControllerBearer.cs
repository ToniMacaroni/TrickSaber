using IPA.Utilities;

namespace TrickSaber
{
    internal class SaberControllerBearer
    {
        private SaberControllerPackage _left;
        private SaberControllerPackage _right;

        private SaberControllerBearer(SaberManager saberManager, PlayerVRControllersManager playerVrControllersManager)
        {
            _left = new SaberControllerPackage(saberManager.leftSaber,
                playerVrControllersManager.GetField<VRController, PlayerVRControllersManager>("_leftHandVRController"));

            _right = new SaberControllerPackage(saberManager.rightSaber,
                playerVrControllersManager.GetField<VRController, PlayerVRControllersManager>("_rightHandVRController"));
        }

        public SaberControllerPackage this[SaberType saberType] => saberType == SaberType.SaberA ? _left : _right;

        internal struct SaberControllerPackage
        {
            public Saber Saber;
            public VRController VRController;

            public SaberControllerPackage(Saber saber, VRController vrController)
            {
                Saber = saber;
                VRController = vrController;
            }
        }
    }
}