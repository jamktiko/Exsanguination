using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMoveable
{
    float moveSpeed {  get; set; }

    void MoveBoss(Vector3 velocity);
}
