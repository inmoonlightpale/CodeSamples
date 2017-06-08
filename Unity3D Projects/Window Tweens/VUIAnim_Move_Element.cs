using UnityEngine;

public class VUIAnim_Move_Element : VUIAnim_Element
{

    [SerializeField]
    private bool DebugElement;


    public float MoveTime;

    private Vector3 OnPosition;
    private Vector3 OffPositionFrom;
    private Vector3 OffPositionTo;

    [SerializeField]
    private VUIAnim_MoveDirections MoveFrom, MoveTo;

    [SerializeField]
    private VUIAnim_MoveSpaces MoveSpaceUsed = VUIAnim_MoveSpaces.screen;

    public override void InitializeTrigger()
    {
        OnPosition = transform.localPosition;
        OffPositionFrom = OnPosition + GiveDirectionEndPosition(MoveFrom);
        OffPositionTo = OnPosition + GiveDirectionEndPosition(MoveTo);

        base.InitializeTrigger();
    }

    public override void TriggerOn()
    {
        if (DebugElement) Debug.Log("Element " + gameObject.name + " Triggering On at " + Time.time);
        transform.localPosition = OffPositionFrom;
        vTween.instance.TweenMoveObject(transform, OnPosition, MoveTime, true);
    }

    public override void TriggerOff()
    {
        if (DebugElement) Debug.Log("Element " + gameObject.name + " Triggering Off at " + Time.time);
        vTween.instance.TweenMoveObject(transform, OffPositionTo, MoveTime, true);
    }

    public override void SnapOn()
    {
        if (DebugElement) Debug.Log("Element " + gameObject.name + " Snapping On at " + Time.time);
        transform.localPosition = OnPosition;
    }

    public override void SnapOff()
    {
        if (DebugElement) Debug.Log("Element " + gameObject.name + " Snapping Off at " + Time.time);
        transform.localPosition = OffPositionFrom;
    }

    internal Vector3 GiveDirectionEndPosition(VUIAnim_MoveDirections dir)
    {
        //TODO: find more elegant way, don't like hard-coding 100 pixel/unit ratio
        float spacemultiplier = (MoveSpaceUsed == VUIAnim_MoveSpaces.screen) ? 1f : 0.01f;
        switch (dir)
        {
            case VUIAnim_MoveDirections.down:
                return new Vector3(0, -Screen.height * 1.1f, 0) * spacemultiplier;
            case VUIAnim_MoveDirections.left:
                return new Vector3(-Screen.width * 1.1f, 0, 0) * spacemultiplier;
            case VUIAnim_MoveDirections.right:
                return new Vector3(Screen.width * 1.1f, 0, 0) * spacemultiplier;
            case VUIAnim_MoveDirections.up:
                return new Vector3(0, Screen.height * 1.1f, 0) * spacemultiplier;

            default:
                return Vector3.zero;
        }
    }
}

public enum VUIAnim_MoveDirections
{
    none,
    right,
    left,
    up,
    down
}

public enum VUIAnim_MoveSpaces
{
    screen,
    world
}
