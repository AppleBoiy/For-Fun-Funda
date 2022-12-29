/*
 *-----------------------------------------------------------------
 * Class:          BlurController
 * Description:    Created by Unity, edited by VC.
 * Author:         VueCode
 * GitHub:         https://github.com/ivuecode/
 * -----------------------------------------------------------------
 */
using UnityEngine;

[ExecuteInEditMode]
public class BlurController : MonoBehaviour
{
    [Header("Blue Settings")]
    public int iterations = 3;                   // Blur iterations - larger number means more blur.
    public float blurSpread = 0.6f;              // Blur spread for each iteration. Lower values give better looking blur.
    private static Material _mMaterial;
    private Material Material { get { if (_mMaterial == null) { _mMaterial = new Material(blurShader) { hideFlags = HideFlags.DontSave }; } return _mMaterial; } }

    public Shader blurShader;            
    // The blur iteration shader just takes 4 texture samples and averages them.
    // By applying it repeatedly and spreading out sample locations
    // we get a Gaussian blur approximation.

    // Performs one blur iteration.
    private void FourTapCone(RenderTexture source, RenderTexture dest, int iteration)
    {
        var off = 0.5f + iteration * blurSpread;
        Graphics.BlitMultiTap(source, dest, Material,
                               new Vector2(-off, -off),
                               new Vector2(-off, off),
                               new Vector2(off, off),
                               new Vector2(off, -off));
    }

    // Do-samples the texture to a quarter resolution.
    private void DownSample4X(RenderTexture source, RenderTexture dest)
    {
        float off = 1.0f;
        Graphics.BlitMultiTap(source, dest, Material,
                               new Vector2(-off, -off),
                               new Vector2(-off, off),
                               new Vector2(off, off),
                               new Vector2(off, -off));
    }

    // Called by the camera to apply the image effect
    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        int rtW = source.width / 4;
        int rtH = source.height / 4;
        RenderTexture buffer = RenderTexture.GetTemporary(rtW, rtH, 0);

        // Copy source to the 4x4 smaller texture.
        DownSample4X(source, buffer);

        // Blur the small texture
        for (int i = 0; i < iterations; i++)
        {
            RenderTexture buffer2 = RenderTexture.GetTemporary(rtW, rtH, 0);
            FourTapCone(buffer, buffer2, i);
            RenderTexture.ReleaseTemporary(buffer);
            buffer = buffer2;
        }
        Graphics.Blit(buffer, destination);
        RenderTexture.ReleaseTemporary(buffer);
    }
}