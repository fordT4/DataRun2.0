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
    [PostProcess(typeof(D2RainsFastPERenderer), PostProcessEvent.AfterStack, "AkilliMum/SRP/D2WeatherEffects/Post/D2RainsFastPE")]
    public sealed class D2RainsFastPE : PostProcessEffectSettings
    {
        public FloatParameter CameraSpeedMultiplier = new FloatParameter { value = 1f };
        public ColorParameter Color = new ColorParameter { value = new Color(1f, 1f, 1f, 1f) };
        public TextureParameter Noise = new TextureParameter { value = null };
        public FloatParameter Density = new FloatParameter { value = 1.4f };
        public FloatParameter Speed = new FloatParameter { value = 0.1f };
        public FloatParameter Exposure = new FloatParameter { value = 5f };
        public FloatParameter Direction = new FloatParameter { value = -0.32f };

        //public bool DarkMode = false;
        //public float DarkMultiplier = 1f;

        //public Shader Shader;
        //private Material _material;
    }

    public sealed class D2RainsFastPERenderer : PostProcessEffectRenderer<D2RainsFastPE>
    {
        private bool _firstRun = false;
        private Vector3 _firstPosition;
        private Vector3 _difference;

        public override void Render(PostProcessRenderContext context)
        {
            var sheet = context.propertySheets.Get(Shader.Find("Hidden/AkilliMum/SRP/D2WeatherEffects/Post/D2RainsFast"));
            //var sheet = context.propertySheets.Get(settings.Shader);

            if (!_firstRun)
            {
                _firstRun = true;
                _firstPosition = Camera.main.transform.position;
            }
            _difference = Camera.main.transform.position - _firstPosition;

            sheet.properties.SetColor("_Color", settings.Color);
            sheet.properties.SetTexture("_NoiseTex", settings.Noise);
            sheet.properties.SetFloat("_Density", settings.Density);
            sheet.properties.SetFloat("_Speed", settings.Speed);
            sheet.properties.SetFloat("_Exposure", settings.Exposure);
            sheet.properties.SetFloat("_Direction", settings.Direction);
            sheet.properties.SetFloat("_CameraSpeedMultiplier", settings.CameraSpeedMultiplier);
            sheet.properties.SetFloat("_UVChangeX", _difference.x);
            sheet.properties.SetFloat("_UVChangeY", _difference.y);

            context.command.BlitFullscreenTriangle(context.source, context.destination, sheet, 0);
        }
    }

}