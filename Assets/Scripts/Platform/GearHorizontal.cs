using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GearHorizontal : GearAbstract
{
    public enum Direction { CW, CCW }
    
    public Direction direction;

    protected override void SetGearType()
    {
        if (direction == Direction.CW)
            this.gearType = GearType.HOR_CW;
        else if (direction == Direction.CCW)
            this.gearType = GearType.HOR_CCW;
    }
}
