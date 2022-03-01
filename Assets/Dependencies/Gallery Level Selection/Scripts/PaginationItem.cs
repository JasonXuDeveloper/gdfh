using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PaginationItem : MonoBehaviour
{
    public TextType textType = TextType.None;
    public GameObject first, second, normal, selected, secondToLast, last;
    public Text txtFirst, txtSecond, textNormal, textSelected, txtSecondToLast, txtLast;

    public enum ItemState { First, Second, Normal, Selected, SecondToLast, Last }
    public enum TextType { None, Numeric, Title }

    private ItemState itemState = ItemState.Selected;

    public void SetItemState(ItemState state)
    {
        if (state == itemState) return;
        itemState = state;

        if (first) first.SetActive(false);
        if (second) second.SetActive(false);
        if (normal) normal.SetActive(false);
        if (selected) selected.SetActive(false);
        if (secondToLast) secondToLast.SetActive(false);
        if (last) last.SetActive(false);

        switch (state)
        {
            case ItemState.First:
                if (first) first.SetActive(true);
                break;
            case ItemState.Second:
                if (second) second.SetActive(true);
                break;
            case ItemState.Normal:
                if (normal) normal.SetActive(true);
                break;
            case ItemState.Selected:
                if (selected) selected.SetActive(true);
                break;
            case ItemState.SecondToLast:
                if (secondToLast) secondToLast.SetActive(true);
                break;
            case ItemState.Last:
                if (last) last.SetActive(true);
                break;
        }

    }

    public void SetText(string value)
    {
        if (txtFirst) txtFirst.text = value;
        if (txtSecond) txtSecond.text = value;
        if (textNormal) textNormal.text = value;
        if (textSelected) textSelected.text = value;
        if (txtSecondToLast) txtSecondToLast.text = value;
        if (txtLast) txtLast.text = value;
    }
    
}
