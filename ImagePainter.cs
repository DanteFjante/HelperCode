using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using DefaultNamespace;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;

[RequireComponent(typeof(Image))]
public class ImagePainter : MonoBehaviour
{
    public Color defaultBackgroundColor;

    public Color drawColor;
    
    [Range(1, 100)]
    public byte pencilSize;

    [Range(0, 1f)] 
    public float strength;
    
    private Image _image;
    private Texture2D tex;

    private Vector3[] buffer;

    public Camera camera;
    
    // Start is called before the first frame update
    void Start()
    {
        _image = GetComponent<Image>();
        RectTransform rt = _image.rectTransform;
        Rect rect = rt.rect;
        rt.pivot = Vector2.one / 2;

        tex = new Texture2D((int) rect.width, (int) rect.height);
        Texture2DTools.FillTexture(defaultBackgroundColor, 1, ref tex);

        _image.sprite = Sprite.Create(tex, new Rect(0, 0, (int) rect.width, (int) rect.height),  Vector2.one / 2);
        _image.sprite.name = "Draw On Me";
        
        print("Creating texture");

        byte[] data = _image.sprite.texture.EncodeToPNG();
        if (!File.Exists("Assets/DrawOnMe.png"))
        {
            var stream = File.OpenWrite("Assets/DrawOnMe.png");
            stream.Write(data, 0, data.Length);
            stream.Close();
        }

    }

    private void OnDestroy()
    {
        byte[] data = _image.sprite.texture.EncodeToPNG();
        var stream = File.OpenWrite("Assets/DrawOnMe.png");
            stream.Write(data, 0, data.Length);
            stream.Close();
            
        print("Closing Texture");

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            
            Vector2 point = Input.mousePosition;
            Vector2 npoint;
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(_image.rectTransform, point, camera, out npoint))
            {
                npoint += _image.rectTransform.rect.size / 2;
                npoint.x -= pencilSize / 2f;
                npoint.y -= pencilSize / 2f;
                Texture2DTools.DrawOnTexture(drawColor, strength, pencilSize, ref tex, V3ToV2Int(npoint));
            }
            print("Screen point: " + point + "Local point" + npoint);
        }
    }

    public static Vector2Int V3ToV2Int(Vector3 vector)
    {
        return new Vector2Int((int) vector.x, (int) vector.y);
    }





}
