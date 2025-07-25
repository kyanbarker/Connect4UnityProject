using System.Linq;
using TMPro;
using UnityEngine;

public class Main : MonoBehaviour
{
    public static Main Instance { get; private set; }

    /// <summary>
    /// The current Color
    /// </summary>
    public Color Color { get; set; }

    /// <summary>
    /// The name of Color
    /// </summary>
    public string ColorName
    {
        get { return Color == Color.red ? "Red" : "Yellow"; }
    }

    /// <summary>
    /// FloorAt[x]: The lowest y coordinate not occupied by a token at a coordinate x
    /// </summary>
    public int[] FloorAt { get; private set; }

    /// <summary>
    /// True if there is a connect 4 on the board, else false
    /// </summary>
    public bool IsConnect4
    {
        get
        {
            Vector2[] paths =
            {
                new Vector2(0, 1),
                new Vector2(1, 0),
                new Vector2(1, 1),
                new Vector2(1, -1)
            };
            return paths.Any(path => IsConnect4Path(path));
        }
    }

    /// <summary>
    /// The TextMeshPro to communicate messages with the player
    /// </summary>
    [field: SerializeField] public TextMeshPro Messenger { get; set; }

    /// <summary>
    /// The dynamically positioned Token previewing the outcome of Drop
    /// </summary>
    public Token PreviewToken { get; private set; }

    /// <summary>
    /// The x coordinate of PreviewToken
    /// </summary>
    public int PreviewTokenX
    {
        get
        {
            return (int)PreviewToken.transform.position.x;
        }
        set
        {
            int x = value;
            int y = FloorAt[x];
            int z = -1;
            PreviewToken.transform.localPosition = new Vector3(x, y, z);
        }
    }

    /// <summary>
    ///
    /// </summary>
    [field: SerializeField] public Transform TokenParent { get; set; }

    /// <summary>
    /// The GameObject to instantiate to create the board and PreviewToken
    /// </summary>
    [field: SerializeField] public GameObject TokenPrefab { get; private set; }

    /// <summary>
    /// The 2D list of Token components
    /// </summary>
    public Token[,] Tokens { get; private set; }

    /// <summary>
    /// Sets the color of the floor Token at x to Color.
    /// Checks for a Connect4.
    /// Invokes SwitchColor if there is no Connect4.
    /// Invokes OnGameEnd if there is a Connect4
    /// </summary>
    public void Drop(int x)
    {
        int y = FloorAt[x];
        if (y < 6)
        {
            Tokens[x, y].Color = Color;
            FloorAt[x]++;
            if (IsConnect4)
            {
                BroadcastMessage("OnGameEnd");
            }
            else
            {
                SwitchColor();
            }
        }
    }

    /// <summary>
    /// Selects and returns the colors of each Token along path beginning at pivot
    /// </summary>
    public Color[] GetColors(Vector2 pivot, Vector2 path)
    {
        Color[] colors = new Color[4];
        for (int i = 0; i < 4; i++)
        {
            Vector2 offset = path * i;
            Vector2 point = pivot + offset;
            colors[i] = Tokens[(int)point.x, (int)point.y].Color;
        }
        return colors;
    }

    private void Awake()
    {
        Instance = this;

        Tokens = new Token[7, 6]; // Each element is null
        FloorAt = new int[7]; // Each element is 0

        CreatePreviewToken();
        CreateBoard();

        SwitchColor();
    }

    private Vector3 ClampVector(Vector3 raw, Vector3 min, Vector3 max)
    {
        float x = Mathf.Clamp(raw.x, min.x, max.x);
        float y = Mathf.Clamp(raw.y, min.y, max.y);
        float z = Mathf.Clamp(raw.z, min.z, max.z);
        return new Vector3(x, y, z);
    }

    /// <summary>
    /// Instantiates 42 instances (7 rows and 6 columns) of TokenPrefab.
    /// Assigns each instance's Token component to Tokens
    /// </summary>
    private void CreateBoard()
    {
        for (int x = 0; x < 7; x++)
        {
            for (int y = 0; y < 6; y++)
            {
                GameObject tokenInstance = Instantiate(TokenPrefab, TokenParent);
                tokenInstance.transform.localPosition = new Vector2(x, y);
                Tokens[x, y] = tokenInstance.GetComponent<Token>();
            }
        }
    }

    /// <summary>
    /// Instantiates TokenPrefab and sets its Opacity to 50%.
    /// Assigns PreviewToken to the instance's Token component
    /// </summary>
    private void CreatePreviewToken()
    {
        PreviewToken = Instantiate(TokenPrefab, TokenParent).GetComponent<Token>();
        PreviewToken.Opacity = 0.5f;
        PreviewToken.Color = Color;
    }

    /// <summary>
    /// Is there a connect 4 along path?
    /// path's x and y coordinates are 0 or 1
    /// </summary>
    private bool IsConnect4Path(Vector2 path)
    {
        Vector2 extents = path * 3; // How far will our search extend?
        Vector2 boardMin = new Vector2(0, 0); // Min position of the board
        Vector2 boardMax = new Vector2(7, 6); // Max position of the board
        Vector2 searchMin = ClampVector(boardMin - extents, boardMin, boardMax); // Min position of our search
        Vector2 searchMax = ClampVector(boardMax - extents, boardMin, boardMax); // Max position of our search

        for (float x = searchMin.x; x < searchMax.x; x++)
        {
            for (float y = searchMin.y; y < searchMax.y; y++)
            {
                Vector2 pivot = new Vector2(x, y);
                Color[] colors = GetColors(pivot, path);
                bool isConnect4 = colors.All(color => color == Color);
                if (isConnect4)
                {
                    return true;
                }
            }
        }
        return false;
    }

    private void OnGameEnd()
    {
        Messenger.text = $"{ColorName} wins!";
    }

    /// <summary>
    /// Switches Color to the opposite of its current value.
    /// Updates the color of PreviewToken.
    /// Updates the color and text of Messenger
    /// </summary>
    private void SwitchColor()
    {
        Color = Color == Color.red ? Color.yellow : Color.red;
        PreviewToken.Color = Color;

        Messenger.color = Color;
        Messenger.text = $"{ColorName}'s turn";
    }
}