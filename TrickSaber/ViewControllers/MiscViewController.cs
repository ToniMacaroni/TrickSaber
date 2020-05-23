using System;
using System.Collections.Generic;
using System.Linq;
using BeatSaberMarkupLanguage.Attributes;
using BeatSaberMarkupLanguage.Components;
using BeatSaberMarkupLanguage.ViewControllers;
using HMUI;
using UnityEngine;

namespace TrickSaber.ViewControllers
{
    internal class MiscViewController : BSMLResourceViewController
    {
        [UIComponent("scrollable")] private Transform _scrollable;

        [UIValue("SpinDirEnum-list")]
        public List<object> SpinDirectionsList = Enum.GetNames(typeof(SpinDir)).ToList<object>();

        [UIValue("DirEnum-list")]
        public List<object> ThumbstickDirectionsList = Enum.GetNames(typeof(ThumstickDir)).ToList<object>();

        public override string ResourceName => "TrickSaber.Views.MiscView.bsml";

        [UIValue("ThumbDir-value")]
        public string ThumbstickDir
        {
            get => PluginConfig.Instance.ThumstickDirection;
            set => PluginConfig.Instance.ThumstickDirection = value;
        }

        [UIValue("IsSpeedVelocityDependent-value")]
        public bool IsSpeedVelocityDependent
        {
            get => PluginConfig.Instance.IsSpeedVelocityDependent;
            set => PluginConfig.Instance.IsSpeedVelocityDependent = value;
        }

        [UIValue("SpinSpeed-value")]
        public float SpinSpeed
        {
            get => PluginConfig.Instance.SpinSpeed;
            set => PluginConfig.Instance.SpinSpeed = value;
        }

        [UIValue("SpinDir-value")]
        public string SpinDir
        {
            get => PluginConfig.Instance.SpinDirection;
            set => PluginConfig.Instance.SpinDirection = value;
        }

        [UIValue("ThrowVelocity-value")]
        public float ThrowVelocity
        {
            get => PluginConfig.Instance.ThrowVelocity;
            set => PluginConfig.Instance.ThrowVelocity = value;
        }

        [UIValue("ReturnSpeed-value")]
        public float ReturnSpeed
        {
            get => PluginConfig.Instance.ReturnSpeed;
            set => PluginConfig.Instance.ReturnSpeed = value;
        }

        [UIValue("SlowmoDuringThrow-value")]
        public bool SlowmoDuringThrow
        {
            get => PluginConfig.Instance.SlowmoDuringThrow;
            set => PluginConfig.Instance.SlowmoDuringThrow = value;
        }

        [UIValue("DisableIfNotesOnScreen-value")]
        public bool DisableIfNotesOnScreen
        {
            get => PluginConfig.Instance.DisableIfNotesOnScreen;
            set => PluginConfig.Instance.DisableIfNotesOnScreen = value;
        }

        [UIValue("EnableCutting-value")]
        public bool EnableCutting
        {
            get => PluginConfig.Instance.EnableCuttingDuringTrick;
            set => PluginConfig.Instance.EnableCuttingDuringTrick = value;
        }

        [UIAction("#post-parse")]
        public void Setup()
        {
            //_scrollable = BSMLScrollableSettingsContainer
            if (_scrollable)
            {
                var rect = _scrollable as RectTransform;
                rect.anchoredPosition = new Vector2(0, 0);
                rect.sizeDelta = new Vector2(-4, -4);
            }
        }
    }
}