using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class PanelManager : MonoBehaviour
{
    internal static PanelManager Instance { get; private set; }

    [SerializeField] private GameObject m_GridPrefab;
    [SerializeField] private IntReactiveProperty m_Row = new IntReactiveProperty(8);
    [SerializeField] private IntReactiveProperty m_Column = new IntReactiveProperty(8);

    public Transform[,] GridTransforms { get; private set; }
    private static Action _OnInitCompleted;

    void Awake() => Instance = this;

    void Start()
    {
        m_Row.Merge(m_Column).Subscribe(_ => InitPanel()).AddTo(this);
    }

    private void InitPanel()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        GridTransforms = new Transform[m_Row.Value, m_Column.Value];
        transform.position = new Vector2((m_Row.Value - 1) / 2f * -1, (m_Column.Value - 1) / 2f * -1);
        Camera.main.orthographicSize = m_Row.Value > m_Column.Value ? m_Row.Value : m_Column.Value / 2f >= m_Row.Value ? (m_Column.Value + 1) / 2f : m_Row.Value;

        for (int i = 0; i < m_Row.Value; i++)
        {
            for (int j = 0; j < m_Column.Value; j++)
            {
                var obj = Instantiate(m_GridPrefab, transform);
                obj.name = $"Grid[{ i },{ j }]";
                obj.transform.localPosition = new Vector2(i, j);
                GridTransforms[i, j] = obj.transform;
            }
        }

        _OnInitCompleted?.Invoke();
    }

    internal static IObservable<Unit> OnInitPanelCompleted() => Observable.FromEvent
    (
        _action => _OnInitCompleted += _action,
        _action => _OnInitCompleted -= _action
    );
}
