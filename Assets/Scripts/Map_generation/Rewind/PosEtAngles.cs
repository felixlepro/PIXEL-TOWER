
using UnityEngine;

public class PositionPlus {

    public Vector3 position;
    public Vector2 direction;
    public bool click;
    public GameObject weapon;
    public GameObject ancienweapon;

    public PositionPlus(Vector3 _position,Vector2 _direction,bool _click, GameObject _ancienweapon, GameObject _weapon)
    {
        position = _position;
        direction=_direction;
        click = _click;
        weapon = _weapon;
        ancienweapon = _ancienweapon;
    }
}
