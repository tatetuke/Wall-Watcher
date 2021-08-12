using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

//https://qiita.com/r-ngtm/items/934493e1e2c364f7019e
//https://edom18.hateblo.jp/entry/2020/11/02/080719
public class GrabRendererPass : ScriptableRenderPass
{
    private const string NAME = nameof(GrabRendererPass);

    private Shader _shader = null;
    private RenderTargetIdentifier _currentTarget = default;

    private int _grabPassTextureID = 0;

    public GrabRendererPass(Shader shader, RenderPassEvent passEvent)
    {
        renderPassEvent = passEvent;
        _shader = shader;

        _grabPassTextureID = Shader.PropertyToID("_GrabPassTexture");
    }

    public void SetRenderTarget(RenderTargetIdentifier target)
    {
        _currentTarget = target;
    }

    public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
    {
        if (_shader == null) return;
        // カメラの設定などにまつわる情報を取得する
        ref CameraData camData = ref renderingData.cameraData;
        // シーンビューだとおかしくなっていたので分岐を入れています。
        if (camData.isSceneViewCamera) return;

        var cameraData = renderingData.cameraData;
        var w = cameraData.camera.scaledPixelWidth;
        var h = cameraData.camera.scaledPixelHeight;

        // CommandBufferをプールから取得する
        CommandBuffer buf = CommandBufferPool.Get(NAME);

        buf.GetTemporaryRT(_grabPassTextureID, w, h, 0, FilterMode.Point, RenderTextureFormat.Default);
        Material material = new Material(_shader);
        buf.Blit(_currentTarget, _grabPassTextureID, material);

        // 最後に、これら一連の流れを記述したCommandBufferを実行します。
        context.ExecuteCommandBuffer(buf);
        CommandBufferPool.Release(buf);
    }
}