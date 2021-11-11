using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] GameObject _shopMenuPanel;
    [SerializeField] GameObject _mainMenuPanel;
    [SerializeField] ShopScrollView _shopScrollView;

    private void Start()
    {
        _shopMenuPanel.SetActive(false);
    }
    public void OpenShop()
    {
        _mainMenuPanel.SetActive(false);
        _shopMenuPanel.SetActive(true);
        _shopScrollView.OpenShopList();
    }

    public void CloseShop()
    {
        _shopMenuPanel.SetActive(false);
        _mainMenuPanel.SetActive(true);
    }
}
