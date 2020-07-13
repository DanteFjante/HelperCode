using System.IO;
using DefaultNamespace;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Canvas), typeof(ToggleGroup))]
public class ImagePainter : MonoBehaviour
{
    public Color backgroundColor;
    public Color drawColor;

    [Range(1, 100)]
    public byte pencilSize;

    [Range(0, 1f)] 
    public float strength;


    [BoxGroup("Required")]
    [Required("Image is required for Image painter to be able to paint")]
    public Image _image;
    [BoxGroup("Required")]
    [Required("Camera is required to reduce calls to camera.main which causes lag")]
    public Camera camera;
    
    [FoldoutGroup("Optional")]
    [Tooltip("To save image to file")]
    public Button saveButton;
    
    [FoldoutGroup("Optional")]
    public InputField imageName;
    
    [FoldoutGroup("Optional")]
    [Tooltip("Toggle if you want to enumarate existing names to keep old copies \nIf off, old copies will be replaced")]
    public Toggle enumerateOnExistingName;
    
    [FoldoutGroup("Optional")]
    [Tooltip("To clear image space to alpha channel")]
    public Button clearImage;
    
    [FoldoutGroup("Optional")]
    [Tooltip("Toggle for if you want to be able to pane")]
    public Toggle canPane;
    
    [FoldoutGroup("Optional")]
    [Tooltip("Toggle for if you want to be able to draw a static collider where paint has been drawn")]
    public Toggle drawCollider;

    [FoldoutGroup("Optional")]
    [Tooltip("Toggle if you want to be able to scroll-zoom")]
    public Toggle canZoom;

    [FoldoutGroup("Optional")] 
    [Tooltip("Slider for if you want to be able to change draw size")]
    public Slider pencilSizeSlider;

    [FoldoutGroup("Optional")] 
    [Tooltip("Slider for if you want to be able to change drawOpacity")]
    public Slider drawOpacitySlider;
    
    [FoldoutGroup("Tools")]
    [Tooltip("Toggle for if you want to paint")]
    public Toggle paintTool;

    [FoldoutGroup("Tools")]
    [Tooltip("Toggle for if you want to use erasor")]
    public Toggle eraseTool;

    [FoldoutGroup("Tools")]
    [Tooltip("Toggle for if you want to use bucket")]
    public Toggle bucketTool;

    [FoldoutGroup("Default Values")] public string _imageName = "Image";
    [FoldoutGroup("Default Values")] public bool _imageEnumarate = true;
    [FoldoutGroup("Default Values")] public bool _canZoom = true;
    [FoldoutGroup("Default Values")] public bool _canPane = true;

    private Texture2D tex;

    private Vector3[] buffer;


    // Start is called before the first frame update
    void Start()
    {

        if(saveButton != null) saveButton.onClick.AddListener(() => SavePNG(GetImageName()));
        if(saveButton != null) saveButton.onClick.AddListener(() => SavePNG(GetImageName()));

        RectTransform rt = _image.rectTransform;
        Rect rect = rt.rect;
        rt.pivot = Vector2.one / 2;

        tex = new Texture2D((int) rect.width, (int) rect.height);
        Texture2DTools.FillTexture(backgroundColor, 1, ref tex);

        _image.sprite = Sprite.Create(tex, new Rect(0, 0, (int) rect.width, (int) rect.height),  Vector2.one / 2);
        _image.sprite.name = _imageName;

    }

    private void OnDestroy()
    {


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
        }
    }

    public static Vector2Int V3ToV2Int(Vector3 vector)
    {
        return new Vector2Int((int) vector.x, (int) vector.y);
    }
    
    public string GetImageName()
    {

        string imgName = _imageName;
        if (imageName && imageName.text.Length > 0)
            imgName = imageName.text;
            
        bool enumerate = _imageEnumarate;
        if (enumerateOnExistingName != null)
            enumerate = enumerateOnExistingName.isOn;

        if (!File.Exists("Assets/" + imgName + ".png") && enumerate)
            return _imageName;
        
        int nameNr = 0;

        while (File.Exists("Assets/" + imgName + nameNr + ".png"))
        {
            nameNr++;
        }

        return "Assets/" + _imageName + nameNr + ".png";

    }
    
    public void SavePNG(string name)
    {
        byte[] data = _image.sprite.texture.EncodeToPNG();
        var stream = File.OpenWrite("Assets/" + name + ".png");
        stream.Write(data, 0, data.Length);
        stream.Close();
    }

    public void ClearImage()
    {
        tex = new Texture2D(tex.width, tex.height);
        Texture2DTools.FillTexture(backgroundColor, 1, ref tex);
    }

    public bool CanPane()
    {
        bool
    }





}
