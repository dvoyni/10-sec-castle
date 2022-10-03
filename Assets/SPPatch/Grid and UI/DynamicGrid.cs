using System;
using UnityEngine;
using UnityEngine.UI;
[RequireComponent(typeof(GridLayoutGroup))]
[ExecuteInEditMode]
public class DynamicGrid : MonoBehaviour
{
    RectTransform _parent;
    GridLayoutGroup _grid;
    public int col = 1, row = 1;
    public float paddingL = 0, paddingR = 0, paddingT = 0, paddingB = 0;
    public float spacingX = 0, spacingY = 0;

    public int priority;

    private void OnEnable() {
        SetGrid();
    }

    void Update()
    {
    #if UNITY_EDITOR
        SetGrid();
    #endif
    }

    public void SetGrid()
    {
        _parent = gameObject.GetComponent<RectTransform>();
        _grid = gameObject.GetComponent<GridLayoutGroup>();
        var padding = _grid.padding;
        padding.left = (int)(paddingL / 100f * Screen.width);
        padding.right = (int)(paddingR / 100f * Screen.width);
        padding.top = (int)(paddingT / 100f * Screen.height);
        padding.bottom = (int)(paddingB / 100f * Screen.height);
        var rect = _parent.rect;
        var spacing = _grid.spacing;
        spacing = new Vector2(spacingX / 100 * (rect.width - padding.left - padding.right), spacingY / 100 * (rect.height - padding.top - padding.bottom));
        _grid.spacing = spacing;
        _grid.cellSize = new Vector2((rect.width - padding.left - padding.right - (spacing.x * (col - 1))) / col, (rect.height - padding.top - padding.bottom - (spacing.y * (row - 1))) / row);
    }

    public void Disable()
    {
        GetComponent<DynamicGrid>().enabled = false;
    }
}


