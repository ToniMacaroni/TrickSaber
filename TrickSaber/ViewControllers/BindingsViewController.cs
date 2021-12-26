using System;
using System.Collections.Generic;
using System.Linq;
using BeatSaberMarkupLanguage.Attributes;
using BeatSaberMarkupLanguage.ViewControllers;
using TrickSaber.Configuration;
using Zenject;

namespace TrickSaber.ViewControllers
{
    internal class BindingsViewController : BSMLResourceViewController
    {
        [UIValue("TrickActionEnum-list")]
        public List<object> TrickActionList = Enum.GetNames(typeof(TrickAction)).ToList<object>();

        public override string ResourceName => "TrickSaber.Views.BindingsView.bsml";

        [Inject] private readonly PluginConfig _config = null;
        [Inject] private readonly TrickSaberPlugin _pluginInfo = null;

        [UIValue("TriggerAction-value")]
        public string TriggerAction
        {
            get => _config.TriggerAction.ToString();
            set
            {
                _config.TriggerAction = value.GetEnumValue<TrickAction>();
                CheckMultiBinding();
            }
        }

        [UIValue("GripAction-value")]
        public string GripAction
        {
            get => _config.GripAction.ToString();
            set
            {
                _config.GripAction = value.GetEnumValue<TrickAction>();
                CheckMultiBinding();
            }
        }

        [UIValue("ThumbAction-value")]
        public string ThumbAction
        {
            get => _config.ThumbstickAction.ToString();
            set
            {
                _config.ThumbstickAction = value.GetEnumValue<TrickAction>();
                CheckMultiBinding();
            }
        }

        [UIValue("ReverseTrigger-value")]
        public bool ReverseTrigger
        {
            get => _config.ReverseTrigger;
            set => _config.ReverseTrigger = value;
        }

        [UIValue("ReverseGrip-value")]
        public bool ReverseGrip
        {
            get => _config.ReverseGrip;
            set => _config.ReverseGrip = value;
        }

        [UIValue("ReverseThumbstick-value")]
        public bool ReverseThumbstick
        {
            get => _config.ReverseThumbstick;
            set => _config.ReverseThumbstick = value;
        }

        [UIValue("ShowIndexText")] public bool ShowIndexText => _pluginInfo.IsKnucklesController;

        [UIValue("ContactInfo")] public string ContactInfo => "My Discord : Toni Macaroni#8970";

        [UIValue("Version")] public string Version => _pluginInfo.Version.ToString();

        [UIValue("NewerVersionAvailable")] public bool NewerVersionAvailable => !_pluginInfo.IsNewestVersion;

        [UIValue("NewerVersionText")] public string NewerVersionText => "Newer version available on Github (" + _pluginInfo.RemoteVersion + ")";

        private bool _multiBindingTextActive;

        [UIComponent("MultiBindingTextActive")]
        public bool MultiBindingTextActive
        {
            get => _multiBindingTextActive;
            set
            {
                _multiBindingTextActive = value;
                NotifyPropertyChanged();
            }
        }

        void CheckMultiBinding()
        {
            List<string> boundActions = new List<string>();
            bool isMultiBinding = false;

            if (TriggerAction!="None" && boundActions.Contains(TriggerAction)) isMultiBinding = true;
            else boundActions.Add(TriggerAction);

            if (GripAction != "None" && boundActions.Contains(GripAction)) isMultiBinding = true;
            else boundActions.Add(GripAction);

            if (ThumbAction != "None" && boundActions.Contains(ThumbAction)) isMultiBinding = true;
            else boundActions.Add(ThumbAction);

            MultiBindingTextActive = isMultiBinding;
        }
    }
}