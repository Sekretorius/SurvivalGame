using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface Skill
{
    Sprite barSprite { get; set; }
    KeyCode keyCode { get; set; }
    bool isOnCooldown { get; }
    bool isActive { get; }
    void onInvoke();  
}
