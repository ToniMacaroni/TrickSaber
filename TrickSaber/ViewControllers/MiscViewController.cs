using System;
using System.Collections.Generic;
using System.Linq;
using BeatSaberMarkupLanguage.Attributes;
using BeatSaberMarkupLanguage.Components;
using BeatSaberMarkupLanguage.ViewControllers;
using HMUI;
using TrickSaber.Configuration;
using TrickSaber.InputHandling;
using UnityEngine;
using Zenject;

namespace TrickSaber.ViewControllers
{
    internal class MiscViewController : BSMLResourceViewController
    {
        public override string ResourceName => "TrickSaber.Views.MiscView.bsml";

        [UIValue("SpinDirEnum-list")]
        public List<object> SpinDirectionsList = Enum.GetNames(typeof(SpinDir)).ToList<object>();

        [UIValue("DirEnum-list")]
        public List<object> ThumbstickDirectionsList = Enum.GetNames(typeof(ThumstickDir)).ToList<object>();

        [UIComponent("scrollable")] private readonly Transform _scrollable = null;

        [Inject] private readonly PluginConfig _config = null;

        [UIValue("ThumbDir-value")]
        public string ThumbstickDir
        {
            get => _config.ThumstickDirection.ToString();
            set => _config.ThumstickDirection = value.GetEnumValue<ThumstickDir>();
        }

        [UIValue("IsSpeedVelocityDependent-value")]
        public bool IsSpeedVelocityDependent
        {
            get => _config.IsSpeedVelocityDependent;
            set => _config.IsSpeedVelocityDependent = value;
        }

        [UIValue("SpinSpeed-value")]
        public float SpinSpeed
        {
            get => _config.SpinSpeed;
            set => _config.SpinSpeed = value;
        }

        [UIValue("SpinDir-value")]
        public string SpinDir
        {
            get => _config.SpinDirection.ToString();
            set => _config.SpinDirection = value.GetEnumValue<SpinDir>();
        }

        [UIValue("ThrowVelocity-value")]
        public float ThrowVelocity
        {
            get => _config.ThrowVelocity;
            set => _config.ThrowVelocity = value;
        }

        [UIValue("ReturnSpeed-value")]
        public float ReturnSpeed
        {
            get => _config.ReturnSpeed;
            set => _config.ReturnSpeed = value;
        }

        [UIValue("SlowmoDuringThrow-value")]
        public bool SlowmoDuringThrow
        {
            get => _config.SlowmoDuringThrow;
            set => _config.SlowmoDuringThrow = value;
        }

        [UIValue("DisableIfNotesOnScreen-value")]
        public bool DisableIfNotesOnScreen
        {
            get => _config.DisableIfNotesOnScreen;
            set => _config.DisableIfNotesOnScreen = value;
        }

        [UIValue("EnableCutting-value")]
        public bool EnableCutting
        {
            get => _config.EnableCuttingDuringTrick;
            set => _config.EnableCuttingDuringTrick = value;
        }

        [UIAction("#post-parse")]
        public void Setup()
        {
            //align the scrollable, so it uses the whole screen
            if (_scrollable && _scrollable.name == "BSMLScrollableSettingsContainer")
            {
                var rect = _scrollable as RectTransform;
                rect.anchoredPosition = new Vector2(0, 0);
                rect.sizeDelta = new Vector2(-4, -4);
            }
        }
    }
}