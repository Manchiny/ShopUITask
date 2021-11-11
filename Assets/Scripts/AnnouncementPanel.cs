using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class AnnouncementPanel : MonoBehaviour
{
    private AnnouncementData _announcementData;

    [SerializeField] private TextMeshProUGUI _itemName;
    [SerializeField] private TextMeshProUGUI _itemCount;
    [SerializeField] private TextMeshProUGUI _totalPrice;
    [SerializeField] private Image _itemIcon;

    [SerializeField] private TextMeshProUGUI _userName;
    [SerializeField] private TextMeshProUGUI _userLevel;
    [SerializeField] private RawImage _userAvatar;


    public void UpdateView(AnnouncementData data)
    {
        _announcementData = data;

        StartCoroutine(LoadImageFromURL());
        StartCoroutine(LoadIconFromResources(data.itemIconName));

        _itemName.text = data.itemName;
        _itemCount.text = $"x{data.count}";
        _totalPrice.text = $"{data.itemCoast * data.count}";

        _userName.text = data.userName;
        _userLevel.text = data.userLevel.ToString();  
    }

    private IEnumerator LoadIconFromResources(string fileName)
    {
        ResourceRequest loadAsync = Resources.LoadAsync(fileName, typeof(Sprite));

         yield return loadAsync;

        _itemIcon.sprite = loadAsync.asset as Sprite;

        yield break;
    }
    private IEnumerator LoadImageFromURL()
    {
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(_announcementData.userAvatarURL);
        request.SendWebRequest();

        while (request.isDone == false)
        {
            yield return new WaitForEndOfFrame();
        }

        Texture2D texture = DownloadHandlerTexture.GetContent(request); 
        if(texture != null)
        {
            _userAvatar.texture = texture;
        }

        yield break;
    }
}