using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopScrollView : MonoBehaviour
{
    [SerializeField] private GameObject _annoncementPanelPrefab;

    private ScrollRect _scrollRect;
    private RectTransform _contentRect;

    private float _offsetVertical;
    private float _offsetHorizontal;

    private List<AnnouncementData> _announcementsData;
    private List<GameObject> _createdPanels;
    private List<Vector2> _panelsPositions;

    private int _selectedPanelID;
    private Vector2 _contentVector;

    private bool _isScrolling;
    private bool _isInit;

    private void Awake()
    {
        _contentRect = GetComponent<RectTransform>();
        _scrollRect = GetComponentInParent<ScrollRect>();
    }
    private void FixedUpdate()
    {
        if (_isInit == false) return;

        float scrollVelocity = Math.Abs(_scrollRect.velocity.x);

        ControlScrollInertia(scrollVelocity);

        if (_isScrolling || scrollVelocity > 400) return;

        CheckSelectedPanelID();
        _contentVector.x = Mathf.SmoothStep(_contentRect.anchoredPosition.x, _panelsPositions[_selectedPanelID].x + _offsetHorizontal / 2f, 10f * Time.fixedDeltaTime);
        _contentRect.anchoredPosition = _contentVector;
    }

    private void CheckSelectedPanelID()
    {
        float nearestPos = float.MaxValue;

        for (int i = 0; i < _panelsPositions?.Count; i++)
        {
            float distance = Math.Abs(_contentRect.anchoredPosition.x - _offsetHorizontal / 2f - _panelsPositions[i].x);
            if (distance < nearestPos)
            {
                nearestPos = distance;
                _selectedPanelID = i;
            }
        }
    }

    private void ControlScrollInertia(float scrollVelocity)
    {
        if (!_isScrolling)
        {
            if (scrollVelocity < 400f || _contentRect.anchoredPosition.x - _offsetHorizontal / 2f >= _panelsPositions[0].x ||
            _contentRect.anchoredPosition.x - _offsetHorizontal / 2f <= _panelsPositions[_panelsPositions.Count - 1].x)
            {
                _scrollRect.inertia = false;
            }
        }
    }
    public void OpenShopList()
    {
        if (_isInit)
        {
            return;
        }
        else
        {
            CreateShopList();
        }
    }
    private void CreateShopList()
    {
        _announcementsData = Shop.Instance.AnnouncementsData;
        _createdPanels = new List<GameObject>();
        _panelsPositions = new List<Vector2>();

        _offsetHorizontal = _annoncementPanelPrefab.GetComponent<RectTransform>().sizeDelta.x;
        _offsetVertical = _annoncementPanelPrefab.GetComponent<RectTransform>().sizeDelta.y;

        if (_announcementsData?.Count > 0)
        {
            _createdPanels.Clear();

            int row = 0;
            int column = 0;
            int newColumn = 0;

            for (int i = 0; i < _announcementsData.Count; i++)
            {
                GameObject itemPanel = Instantiate(_annoncementPanelPrefab, transform, false);
                AnnouncementPanel announcement = itemPanel.GetComponent<AnnouncementPanel>();
                announcement.UpdateView(_announcementsData[i]);
                _createdPanels.Add(itemPanel);

                float offsetY = 0;

                switch (row)
                {
                    case 0:
                        offsetY = _offsetVertical / 2f;
                        row++;
                        break;

                    case 1:
                        offsetY = _offsetVertical + _offsetVertical / 2f;
                        row++;
                        break;

                    case 2:
                        offsetY = _offsetVertical * 2 + _offsetVertical / 2f;
                        row = 0;
                        newColumn++;
                        break;
                }

                itemPanel.transform.localPosition = new Vector2(column * _offsetHorizontal + _offsetHorizontal / 2, -offsetY);
                _panelsPositions.Add(-itemPanel.transform.localPosition);
                column = newColumn;
            }
        }

        _isInit = true;
    }

    public void Scrolling(bool scroll)
    {
        _isScrolling = scroll;
        if (scroll)
        {
            StopAllCoroutines();
            _scrollRect.inertia = true;
        }
    }

    public void MoveNext()
    {
        CheckSelectedPanelID();
        int panelsStep = 3;
        if (_selectedPanelID < _createdPanels.Count - panelsStep)
        {
            StopAllCoroutines();
            _scrollRect.inertia = false;
            _isScrolling = true;
            StartCoroutine(MoveToNextColumn(_selectedPanelID + panelsStep));
        }
    }

    private IEnumerator MoveToNextColumn(int id)
    {
        while (true)
        {
            _contentVector.x = Mathf.SmoothStep(_contentRect.anchoredPosition.x, _panelsPositions[id].x + _offsetHorizontal / 2f, 15f * Time.deltaTime);
            _contentRect.anchoredPosition = _contentVector;

            if (Math.Abs(_contentRect.anchoredPosition.x - (_panelsPositions[id].x + _offsetHorizontal / 2f)) <= 1f)
            {
                _isScrolling = false;
            }
            yield return null;
        }

    }

    public void MovePrevious()
    {
        CheckSelectedPanelID();

        if (_selectedPanelID > 2)
        {
            StopAllCoroutines();
            _scrollRect.inertia = false;
            _isScrolling = true;
            StartCoroutine(MoveToPreviousColumn(_selectedPanelID - 1));
        }
    }

    private IEnumerator MoveToPreviousColumn(int id)
    {
        while (true)
        {
            _contentVector.x = Mathf.SmoothStep(_contentRect.anchoredPosition.x, _panelsPositions[id].x + _offsetHorizontal / 2f, 15f * Time.deltaTime);
            _contentRect.anchoredPosition = _contentVector;

            if (Math.Abs(_contentRect.anchoredPosition.x - (_panelsPositions[id].x + _offsetHorizontal / 2f)) <= 1f)
            {
                _isScrolling = false;
            }
            yield return null;
        }
    }
}
