using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class BlockManager : MonoBehaviour
{
    [SerializeField] private ColorBlock m_ColorPrefab;

    void Start()
    {
        PanelManager.OnInitPanelCompleted()
            .Subscribe(_ =>
            {
                foreach (var gridTr in PanelManager.Instance.GridTransforms)
                {
                    var obj = Instantiate(m_ColorPrefab, gridTr);
                }
            }).AddTo(this);
    }
}
