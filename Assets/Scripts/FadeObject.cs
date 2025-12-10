using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class FadeObject : MonoBehaviour
{
    private Material _material;
    private Color _originalColor;

    void Awake()
    {
        _material = GetComponent<Renderer>().material;
        _originalColor = _material.color;
        // Ensure the material is set to transparent mode
        SetMaterialTransparent();
    }

    public void SetAlpha(float alpha)
    {
        Color c = _material.color;
        c.a = alpha;
        _material.color = c;
    }

    private void SetMaterialTransparent()
    {
        if (_material.HasProperty("_Mode"))
            _material.SetFloat("_Mode", 3); // 3 = Transparent
        _material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
        _material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
        _material.SetInt("_ZWrite", 0);
        _material.DisableKeyword("_ALPHATEST_ON");
        _material.EnableKeyword("_ALPHABLEND_ON");
        _material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
        _material.renderQueue = 3000;
    }
}