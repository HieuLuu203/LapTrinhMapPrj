using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class MovementController : MonoBehaviour
{
    private Rigidbody2D rb;
    private Vector2 direction = Vector2.down;
    public float speed = 5f;

    [Header("Input")]
    public KeyCode inputUp = KeyCode.W;
    public KeyCode inputDown = KeyCode.S;
    public KeyCode inputLeft = KeyCode.A;
    public KeyCode inputRight = KeyCode.D;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        string input = "";
        if (Input.GetKey(inputUp)) input += "W;";
        if (Input.GetKey(inputDown)) input += "S;";
        if (Input.GetKey(inputLeft)) input += "A;";
        if (Input.GetKey(inputRight)) input += "D;";

        if (!string.IsNullOrEmpty(input))
        {
            GameManager.Instance.SendPlayerInput(input, Time.deltaTime);
        }
    }
}