using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConwaysGameOfLife : MonoBehaviour
{
    public Texture input;

    public RenderTexture result;
    public ComputeShader computeShader;

    public Material material;

    public int width = 512;
    public int height = 512;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (height < 1 || width < 1) return;

        int kernel = computeShader.FindKernel("GameOfLife");

        computeShader.SetTexture(kernel, "Input", input);
        computeShader.SetFloat("Width", width);
        computeShader.SetFloat("Height", height);

        result = new RenderTexture(width, height, 24);
        result.wrapMode = TextureWrapMode.Repeat;
        result.enableRandomWrite = true;
        result.filterMode = FilterMode.Point;
        result.useMipMap = false;
        result.Create();

        computeShader.SetTexture(kernel, "Result", result);
        computeShader.Dispatch(kernel, width / 8, height / 8, 1);

        input = result; // set input of the next frame to the result
        material.mainTexture = input;
    }
}
