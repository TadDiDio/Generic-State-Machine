using UnityEngine;

public static class InputManager
{
    public static bool SprintKey()
    {
        return Input.GetKey(KeyCode.LeftShift);
    }
    public static Vector2 DirectionalInput()
    {
        return Vector2.right * Input.GetAxisRaw("Horizontal") + Vector2.up * Input.GetAxisRaw("Vertical");
    }
    public static bool GrappleKey()
    {
        return Input.GetKey(KeyCode.Q);
    }
}
