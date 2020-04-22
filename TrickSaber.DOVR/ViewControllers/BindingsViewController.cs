using System;
using System.Collections.Generic;
using System.Linq;
using BeatSaberMarkupLanguage.Attributes;
using BeatSaberMarkupLanguage.ViewControllers;
using TrickSaber.DOVR;

namespace TrickSaber.ViewControllers
{
    internal class BindingsViewController : BSMLResourceViewController
    {
        [UIValue("TrickActionEnum-list")]
        public List<object> TrickActionList = Enum.GetNames(typeof(TrickAction)).ToList<object>();

        public override string ResourceName => "TrickSaber.DOVR.Views.BindingsView.bsml";

        [UIValue("TriggerAction-value")]
        public string UseTrigger
        {
            get => PluginConfig.Instance.TriggerAction;
            set => PluginConfig.Instance.TriggerAction = value;
        }

        [UIValue("GripAction-value")]
        public string GripAction
        {
            get => PluginConfig.Instance.GripAction;
            set => PluginConfig.Instance.GripAction = value;
        }

        [UIValue("ThumbAction-value")]
        public string ThumbAction
        {
            get => PluginConfig.Instance.ThumbstickAction;
            set => PluginConfig.Instance.ThumbstickAction = value;
        }

        [UIValue("BindingSupported")] public bool BindingSupported => TrickSaberPlugin.IsControllerSupported;

        [UIValue("ShowIndexText")] public bool ShowIndexText => !TrickSaberPlugin.IsControllerSupported;

        [UIValue("ContactInfo")] public string ContactInfo => "My Discord : Toni Macaroni#8970";

        [UIValue("Version")] public string Version => TrickSaberPlugin.Version.GetVersionString();

        [UIValue("NewerVersionAvailable")] public bool NewerVersionAvailable => !TrickSaberPlugin.IsNewestVersion;

        [UIValue("NewerVersionText")] public string NewerVersionText => "Newer version available on Github (" + TrickSaberPlugin.RemoteVersion.GetVersionString() + ")";
    }
}