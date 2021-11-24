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

    private float _panelWidth;

    private List<AnnouncementData> _announcementsData;
    private List<AnnouncementPanel> _createdPanels;
    private List<float> _panelsXPositions;

    private int _selectedColumnID = 0;
    private int _lastColumnID = 0;

    private float _buttonScrollSpeed = 20f;
    private const int Raws = 3;
    private int _maxColumns;
    public int LastColumnID
    {
        get => _lastColumnID;
        private set
        {

            if (_contentRect.anchoredPosition.x <= (_maxColumns-2) * (-_panelWidth))
            {
                StopAllCoroutines();
                StartCoroutine(MoveToPreviousColumn(_maxColumns - 2));
            }
            if (_contentRect.anchoredPosition.x > 0)
            {
                StartCoroutine(MoveToNextColumn(0));
            }

            if (value > _lastColumnID && value < _announcementsData.Count)
            {
                AddColumns(value);
                HideColumnsLeft(value);
            }
            else if (value < _lastColumnID && value > 0)
            {
                UnhideColumnsLeft(_lastColumnID, value);
                HideColumnsRight(value);
            }
            else return;

            _lastColumnID = value;
        }
    }
    private Vector2 _contentVector;

    public bool _isScrolling;
    private bool _isInit;
    private float _scrollVelocity;

    private void Awake()
    {
        _contentRect = GetComponent<RectTransform>();
        _scrollRect = GetComponentInParent<ScrollRect>();
    }
    public void OnRectValueChanged()
    {
        CheckSelectedColumnID();
        _scrollVelocity = Math.Abs(_scrollRect.velocity.x);
        ControlScrollInertia(_scrollVelocity);
    }
    private void CheckSelectedColumnID()
    {
        float nearestPos = float.MaxValue;
        int tempID = _selectedColumnID;

        for (int i = 0; i < _panelsXPositions?.Count; i++)
        {
            float distance = Math.Abs(_contentRect.anchoredPosition.x - _panelsXPositions[i]);
            if (distance <= nearestPos)
            {
                nearestPos = distance;
                tempID = i;
            }
        }

        LastColumnID = _selectedColumnID;
        _selectedColumnID = tempID;
    }

    private void ControlScrollInertia(float scrollVelocity)
    {
        if (!_isScrolling)
        {
            if (scrollVelocity < 400f || _contentRect.anchoredPosition.x >= _panelsXPositions[0] ||
            _contentRect.anchoredPosition.x <= _panelsXPositions[_panelsXPositions.Count - 1])
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
        _panelsXPositions = new List<float>();
        _createdPanels = new List<AnnouncementPanel>();

        _panelWidth = _annoncementPanelPrefab.GetComponent<RectTransform>().sizeDelta.x;


        _maxColumns = _announcementsData.Count / Raws;
        if (_announcementsData.Count % Raws != 0)
            _maxColumns += 1;

        Debug.Log(_maxColumns);

        if (_announcementsData?.Count > 0)
        {
            CreateColumns(3);
        }
        _isInit = true;
    }

    private void CreateColumns(int count)
    {
        if (_announcementsData?.Count > 0)
        {
            for (int i = 0; i < Raws * count; i++)
            {
                CreatePanel();
            }
        }
    }
    private void CreatePanel()
    {
        if (_createdPanels.Count < _announcementsData.Count)
        {
            GameObject itemPanel = Instantiate(_annoncementPanelPrefab, transform, false);
            AnnouncementPanel announcement = itemPanel.GetComponent<AnnouncementPanel>();
            announcement.UpdateView(_announcementsData[_createdPanels.Count]);
            _createdPanels.Add(announcement);

            if (_createdPanels.Count % (float)Raws == 0)
            {
                _panelsXPositions.Add(-_panelsXPositions.Count * _panelWidth);
            }
        }
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
        CheckSelectedColumnID();
        if (_selectedColumnID < _panelsXPositions.Count - 1)
        {
            StopAllCoroutines();

            StartCoroutine(MoveToNextColumn(_selectedColumnID + 1));
        }
    }

    private IEnumerator MoveToNextColumn(int id)
    {
        if (id < 0)
            id = 0;
        if (id > _panelsXPositions.Count - 1)
            id = _panelsXPositions.Count;

        _isScrolling = true;
        _scrollRect.inertia = false;
        while (true)
        {
            _contentVector.x = Mathf.SmoothStep(_contentRect.anchoredPosition.x, _panelsXPositions[id], _buttonScrollSpeed * Time.deltaTime);
            _contentRect.anchoredPosition = _contentVector;

            if (Math.Abs(_contentRect.anchoredPosition.x - _panelsXPositions[id]) <= 0.5f)
            {
                _isScrolling = false;
            }
            yield return null;
        }
    }

    public void MovePrevious()
    {
        CheckSelectedColumnID();

        if (_selectedColumnID > 0)
        {
            StopAllCoroutines();
            StartCoroutine(MoveToPreviousColumn(_selectedColumnID - 1));
        }
    }

    private IEnumerator MoveToPreviousColumn(int id)
    {

        if (id < 0)
            id = 0;
        if (id > _panelsXPositions.Count - 1)
            id = _panelsXPositions.Count;

        _isScrolling = true;
        _scrollRect.inertia = false;

        while (true)
        {
            _contentVector.x = Mathf.SmoothStep(_contentRect.anchoredPosition.x, _panelsXPositions[id], _buttonScrollSpeed * Time.deltaTime);
            _contentRect.anchoredPosition = _contentVector;

            if (Math.Abs(_contentRect.anchoredPosition.x - _panelsXPositions[id]) <= 0.5f)
            {
                _isScrolling = false;
            }
            yield return null;
        }
    }

    private void AddColumns(int newColumnId)
    {
        int extraColumns = 3;

        for (int i = newColumnId * Raws; i < (newColumnId + extraColumns) * Raws; i++)
        {
            if (_createdPanels.Count > i)
            {
                _createdPanels[i].SetContentVisible(true);
            }
            else
            {
                CreatePanel();
            }
        }
    }

    private void HideColumnsLeft(int newColumnId)
    {
        if (newColumnId < 2)
            return;
        int extraColumns = 2;
        for (int i = (newColumnId * Raws) - extraColumns * 2; i >= 0; i--)
        {
            _createdPanels[i].SetContentVisible(false);
        }
    }
    private void HideColumnsRight(int newColumnId)
    {
        if (newColumnId < 2)
            return;

        int extraColumns = 3;
        for (int i = (newColumnId + extraColumns) * Raws; i < _createdPanels.Count; i++)
        {

            _createdPanels[i]?.SetContentVisible(false);
        }
    }

    private void UnhideColumnsLeft(int lastColumnId, int newColumnId)
    {
        int extraColumns = 3;
        for (int i = 1 + (lastColumnId) * Raws; i >= (newColumnId - extraColumns) * Raws; i--)
        {
            if (i <= 0)
            {
                _createdPanels[0].SetContentVisible(true);
                break;
            }

            if (i > 0 && i < _createdPanels.Count)
            {
                _createdPanels[i].SetContentVisible(true);
            }
        }
    }
}
