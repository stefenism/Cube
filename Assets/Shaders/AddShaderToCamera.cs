using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddShaderToCamera : MonoBehaviour
{
    public Material postProcessingMat;

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        Graphics.Blit(source, destination,postProcessingMat);
    }
}
