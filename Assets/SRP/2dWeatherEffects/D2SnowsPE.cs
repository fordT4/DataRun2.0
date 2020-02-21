//#define DEBUG_RENDER

using UnityEngine;
using System.Collections;
using System.Linq;
using System.Runtime.InteropServices;
using System;
using UnityEngine.Rendering.PostProcessing;

namespace AkilliMum.SRP.D2WeatherEffects
{
    [Serializable]
    [PostProcess(typeof(D2SnowsPERenderer), PostProcessEvent.AfterStack, "AkilliMum/SRP/D2WeatherEffects/Post/D2SnowsPE")]
    public sealed class D2SnowsPE : PostProcessEffectSettings
    {
        public FloatParameter CameraSpeedMultiplier = new FloatParameter { value = 1f };
        public ColorParameter Color = new ColorParameter { value = new Color(1f, 1f, 1f, 1f) };
        [Range(1, 50)]
        public FloatParameter ParticleMultiplier = new FloatParameter { value = 10.0f };
        public FloatParameter Speed = new FloatParameter { value = 4.0f };
        public FloatParameter Direction = new FloatParameter { value = 0.2f };
        public FloatParameter Luminance = new FloatParameter { value = 1f };
        [Range(0.01f, 50)]
        public FloatParameter Zoom = new FloatParameter { value = 1.2f };
        public BoolParameter DarkMode = new BoolParameter { value = false };
        [Range(0f, 0.1f)]
        public FloatParameter LuminanceAdder = new FloatParameter { value = 0.002f };

    }

    public sealed class D2SnowsPERenderer : PostProcessEffectRenderer<D2SnowsPE>
    {
        private bool _firstRun = false;
        private Vector3 _firstPosition;
        private Vector3 _difference;

        public override void Render(PostProcessRenderContext context)
        {
            var sheet = context.propertySheets.Get(Shader.Find("Hidden/AkilliMum/SRP/D2WeatherEffects/Post/D2Snows"));

            if (!_firstRun)
            {
                _firstRun = true;
                _firstPosition = Camera.main.transform.position;
            }
            _difference = Camera.main.transform.position - _firstPosition;

            sheet.properties.SetColor("_Color", settings.Color);
            sheet.properties.SetFloat("_Speed", settings.Speed);
            sheet.properties.SetFloat("_Direction", settings.Direction);
            sheet.properties.SetFloat("_Zoom", settings.Zoom);
            sheet.properties.SetFloat("_DarkMode", settings.DarkMode == true ? 1 : 0);
            sheet.properties.SetFloat("_DarkMultiplier", settings.Luminance);
            sheet.properties.SetFloat("_Multiplier", settings.ParticleMultiplier);
            sheet.properties.SetFloat("_LuminanceAdd", settings.LuminanceAdder);
            sheet.properties.SetFloat("_CameraSpeedMultiplier", settings.CameraSpeedMultiplier);
            sheet.properties.SetFloat("_UVChangeX", _difference.x);
            sheet.properties.SetFloat("_UVChangeY", _difference.y);

            context.command.BlitFullscreenTriangle(context.source, context.destination, sheet, 0);
        }
    }

}