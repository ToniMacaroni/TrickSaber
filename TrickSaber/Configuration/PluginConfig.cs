
using System.Runtime.CompilerServices;
using IPA.Config.Stores;
using IPA.Config.Stores.Attributes;
using IPA.Config.Stores.Converters;

[assembly: InternalsVisibleTo(GeneratedStore.AssemblyVisibilityTarget)]

namespace TrickSaber.Configuration
{
    internal class PluginConfig
    {
        public bool TrickSaberEnabled { get; set; } = true;

        [UseConverter(typeof(EnumConverter<TrickAction>))]
        public TrickAction TriggerAction { get; set; } = TrickAction.Throw;

        [UseConverter(typeof(EnumConverter<TrickAction>))]
        public TrickAction GripAction { get; set; } = TrickAction.None;

        [UseConverter(typeof(EnumConverter<TrickAction>))]
        public TrickAction ThumbstickAction { get; set; } = TrickAction.Spin;

        public bool ReverseTrigger { get; set; } = false;
        public bool ReverseGrip { get; set; } = false;
        public bool ReverseThumbstick { get; set; } = false;

        [UseConverter(typeof(EnumConverter<ThumstickDir>))]
        public ThumstickDir ThumstickDirection { get; set; } = ThumstickDir.Horizontal;

        public float TriggerThreshold { get; set; } = 0.8f;
        public float GripThreshold { get; set; } = 0.8f;
        public float ThumbstickThreshold { get; set; } = 0.8f;

        public float ControllerSnapThreshold { get; set; } = 0.3f;


        public bool IsSpeedVelocityDependent { get; set; } = false;

        public float SpinSpeed { get; set; } = 1f;

        [UseConverter(typeof(EnumConverter<SpinDir>))]
        public SpinDir SpinDirection { get; set; } = SpinDir.Backward;

        public float ThrowVelocity { get; set; } = 1f;

        public float ReturnSpeed { get; set; } = 10f;

        public float ReturnSpinMultiplier { get; set; } = 1f;

        [DisablesScoring]
        public bool SlowmoDuringThrow { get; set; } = false;

        public float SlowmoAmount { get; set; } = 0.2f;

        public bool CompleteRotationMode { get; set; } = false;

        public bool DisableIfNotesOnScreen { get; set; } = false;

        [DisablesScoring(Reason = "Hit Em")]
        public bool HitNotesDuringTrick { get; set; } = false;

        //Advanced
        public int VelocityBufferSize { get; set; } = 5;

        public float SlowmoStepAmount { get; set; } = 0.02f;
    }
}