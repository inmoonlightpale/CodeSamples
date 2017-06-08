using UnityEngine;

public class VUIAnim_Receiver : MonoBehaviour
{
    [SerializeField]
    private VUIAnim_Element[] Elements;

    internal virtual void TriggerElements()
    {
        for (int i = 0; i < Elements.Length; i++)
        {
            Elements[i].TriggerElement();
        }
    }

}
