using UnityEngine;
using UnityEngine.UI;

public class UIPlayerProfileUnit : MonoBehaviour
{
    private PlayerProfile _currentProfile;

    [SerializeField]
    private bool LastSelectedProfile;

    [SerializeField]
    private Image PlayerPortrait, BlingImage;

    public void SetState(PlayerProfile player, bool LastSelected)
    {
        _currentProfile = player;
        LastSelectedProfile = LastSelected;
        SetGraphics();

    }

    void SetGraphics()
    {
        string portraitname = _currentProfile.PlayerPortrait;

        portraitname = portraitname.Replace(variables.ResourcesPlayerPortraits, "");

        Debug.Log("Tying to load " + variables.ResourcesPlayerPortraitsLarge + portraitname);

        Sprite s = Resources.Load<Sprite>(variables.ResourcesPlayerPortraitsLarge + portraitname);

        PlayerPortrait.sprite = s;
        BlingImage.enabled = LastSelectedProfile;
    }
}
