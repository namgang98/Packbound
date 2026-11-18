using UnityEngine;

public class SellBackpackPanel : DungeonBackpackPanel
{
    [SerializeField, Min(0f)] float topPadding = 8f;
    [SerializeField, Min(0f)] float bottomPadding = 8f;
    [SerializeField, Min(0f)] float sidePadding = 8f;

    protected override float GetRowTop()
    {
        RectTransform area = (RectTransform)transform;
        return area.rect.height * 0.5f - topPadding;
    }

    protected override void Layout(bool hasPouch)
    {
        ResetScale();
        base.Layout(hasPouch);

        float scale = GetFitScale(hasPouch);

        if(scale >= 1f)
            return;

        float top = GetRowTop();
        ScaleFromTop(BagSlotRect, top, scale);
        ScaleFromTop(PouchSlotRect, top, scale);
        ScaleFromTop((RectTransform)bagView.transform, top, scale);
        ScaleFromTop((RectTransform)pouchView.transform, top, scale);
    }

    private float GetFitScale(bool hasPouch)
    {
        Rect area = ((RectTransform)transform).rect;
        float availableWidth = area.width - sidePadding * 2f;
        float availableHeight = area.height - topPadding - bottomPadding;

        float contentWidth = SlotColumnWidth * 2f + bagView.PixelSize.x + pouchSpacing;
        float contentHeight = Mathf.Max(bagView.PixelSize.y, BagSlotRect.rect.height);

        if(hasPouch)
        {
            contentWidth += pouchView.PixelSize.x;
            contentHeight = Mathf.Max(contentHeight, pouchView.PixelSize.y);
        }

        if(contentWidth <= 0f || contentHeight <= 0f)
            return 1f;

        return Mathf.Min(1f, availableWidth / contentWidth, availableHeight / contentHeight);
    }

    private void ScaleFromTop(RectTransform target, float top, float scale)
    {
        Vector2 position = target.anchoredPosition;
        target.anchoredPosition = new Vector2(position.x * scale, top + (position.y - top) * scale);
        target.localScale = Vector3.one * scale;
    }

    private void ResetScale()
    {
        BagSlotRect.localScale = Vector3.one;
        PouchSlotRect.localScale = Vector3.one;
        bagView.transform.localScale = Vector3.one;
        pouchView.transform.localScale = Vector3.one;
    }
}