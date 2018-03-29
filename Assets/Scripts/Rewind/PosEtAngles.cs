
using UnityEngine;

public class PositionPlus {

    public Vector3 position;
    public Vector2 direction;
    public bool click;

    public PositionPlus(Vector3 _position,Vector2 _direction,bool _click)
    {
        position = _position;
        direction=_direction;
        click = _click;
    }
}
