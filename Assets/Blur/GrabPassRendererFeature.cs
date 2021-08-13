using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
public class GrabPassRendererFeature : ScriptableRendererFeature
{
    [SerializeField] private Shader _shader = null;
    [SerializeField] private RenderPassEvent _renderPassEvent = RenderPassEvent.AfterRenderingOpaques;

    private GrabRendererPass _grabPass = null;

    public override void Create()
    {
        Debug.Log("Create GrabPass Renderer Feature.");

        if (_grabPass == null)
        {
            _grabPass = new GrabRendererPass(_shader, _renderPassEvent);
        }
    }

    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        Debug.Log("pass add");
        _grabPass.SetRenderTarget(renderer.cameraColorTarget);
        renderer.EnqueuePass(_grabPass);
    }
}