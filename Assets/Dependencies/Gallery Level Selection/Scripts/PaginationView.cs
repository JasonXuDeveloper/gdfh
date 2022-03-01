using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class PaginationView : MonoBehaviour
{
    public GalleryLevelSelectionManager galleryManager;
    public bool hasGalleryManager = true;
    public GameObject itemPrefab;
    public Transform itemsContainer;
    public Layout layout = Layout.Horizontal;
    public bool reverseOrder = false;
    public bool isItemsClickable = true;
    public float itemsWidth = 20;
    public float itemsHeight = 20;
    public float spaceBetweenItems = 5.0f;
    public int totalItemsCount = 10;
    public int visibleItems = 10;
    public bool refresh = true;
    public int firstSelectedIndex = 0;

    [HideInInspector] public PaginationItem[] items = new PaginationItem[0];
    [HideInInspector] public int gallerySelectedIndex = -1;
    [HideInInspector] public int localSelectedIndex = -1;

    private float mItemsWidth;
    private float mItemsHeight;
    private int mVisibleItems = 0;
    private int mGallertItemsCount = 0;
    private bool mReverseOrder = false;
    private int itemsState = 0;
    private PaginationItem.TextType itemsTextType = PaginationItem.TextType.None;
    private int minItemCount = 5;

    public enum Layout { Horizontal, Vertical }

    public virtual void Start()
    {
        if (Application.isPlaying)
        {
            refresh = true;
            Initialize();
        }
    }

    public void OnEditorForceRefresh()
    {
        refresh = true;
    }

    private void Initialize()
    {
        if (!itemPrefab)
        {
            Debug.LogError("You need to assign the items prefab of the PaginationView.");
            return;
        }

        mItemsWidth = itemsWidth;
        mItemsHeight = itemsHeight;
        SwapIfVertical(ref mItemsWidth, ref mItemsHeight);

        if (totalItemsCount < 0) totalItemsCount = 0;
        int galleryItemsCount = totalItemsCount;

        if (hasGalleryManager && galleryManager)
        {
            galleryItemsCount = galleryManager.items.Count;
        }
        else if(hasGalleryManager)
        {
            Debug.LogError("You need to assign the GalleryLevelSelection.");
        }

        visibleItems = Mathf.Clamp(visibleItems, 0, galleryItemsCount);
        if (visibleItems < minItemCount && visibleItems < galleryItemsCount) visibleItems = (galleryItemsCount < minItemCount) ? galleryItemsCount : minItemCount;

        int order = (reverseOrder) ? -1 : 1;

        if (mVisibleItems != visibleItems || refresh)
        {
            mVisibleItems = visibleItems;
            gallerySelectedIndex = -1;
            localSelectedIndex = -1;
            refresh = false;

            for (int i = 0; i < items.Length; i++)
            {
                if (items[i]) DestroyImmediate(items[i].gameObject);
            }

            items = new PaginationItem[visibleItems];

            itemsTextType = itemPrefab.GetComponent<PaginationItem>().textType;

            for (int i = 0; i < items.Length; i++)
            {
                int x = new int();
                x = i;
                GameObject obj = Instantiate(itemPrefab, Vector3.zero, Quaternion.identity, itemsContainer);
                if (isItemsClickable) obj.GetComponent<Button>().onClick.AddListener(delegate { OnPaginationItemsClick(x); });
                items[i] = obj.GetComponent<PaginationItem>();
                items[i].SetItemState(PaginationItem.ItemState.Normal);
                if (itemsTextType == PaginationItem.TextType.Numeric) items[i].SetText((i + 1) + "");
                else if (itemsTextType == PaginationItem.TextType.Title)
                {
                    if (hasGalleryManager && galleryManager)
                        items[i].SetText(galleryManager.items[i].levelName);
                    else
                        items[i].SetText("title");
                }
            }

        }

        Vector2 pos;
        for (int i = 0; i < items.Length; i++)
        {
            pos = new Vector2(order * i * (mItemsWidth + spaceBetweenItems), 0);
            SwapIfVertical(ref pos);
            items[i].transform.localPosition = pos;
        }

        pos = new Vector2(-order * (visibleItems - 1) * (mItemsWidth + spaceBetweenItems) / 2, 0);
        SwapIfVertical(ref pos);
        itemsContainer.localPosition = pos;

        if (hasGalleryManager && galleryManager)
            SetSelectedIndex(galleryManager.GetCurrentNearestItemIndex());
        else
        {
            firstSelectedIndex = Mathf.Clamp(firstSelectedIndex, 0, visibleItems - 1);
            SetSelectedIndex(firstSelectedIndex);
        }

    }

    public virtual void Update()
    {
#if UNITY_EDITOR
        if (!Application.isPlaying)
        {
            Initialize();
        }
#endif
    }

    public virtual void SetSelectedIndex(int value)
    {
        int galleryItemsCount = totalItemsCount;

        if (hasGalleryManager && galleryManager)
        {
            galleryItemsCount = galleryManager.items.Count;
        }

        if (value != gallerySelectedIndex || mGallertItemsCount != galleryItemsCount || mReverseOrder != reverseOrder)
        {

            if (items.Length > 0)
            {
                if (localSelectedIndex >= 0 && localSelectedIndex < items.Length)
                {
                    items[localSelectedIndex].SetItemState(PaginationItem.ItemState.Normal);
                }

                if (items.Length == galleryItemsCount)
                {
                    if (mGallertItemsCount != galleryItemsCount)
                    {
                        for (int i = 0; i < items.Length; i++) items[i].SetItemState(PaginationItem.ItemState.Normal);
                    }

                    localSelectedIndex = value;
                }
                else
                {
                    if (itemsState == 0)
                    {
                        if (value > items.Length - 3)
                        {
                            items[0].SetItemState(reverseOrder ? PaginationItem.ItemState.Last : PaginationItem.ItemState.First);
                            items[1].SetItemState(reverseOrder ? PaginationItem.ItemState.SecondToLast : PaginationItem.ItemState.Second);

                            if (value <= galleryItemsCount - 3)
                            {
                                items[items.Length - 2].SetItemState(reverseOrder ? PaginationItem.ItemState.Second : PaginationItem.ItemState.SecondToLast);
                                items[items.Length - 1].SetItemState(reverseOrder ? PaginationItem.ItemState.First : PaginationItem.ItemState.Last);

                                itemsState = 1;
                            }
                            else
                            {
                                items[items.Length - 2].SetItemState(PaginationItem.ItemState.Normal);
                                items[items.Length - 1].SetItemState(PaginationItem.ItemState.Normal);

                                itemsState = 2;
                            }
                        }
                        else
                        {
                            items[0].SetItemState(PaginationItem.ItemState.Normal);
                            items[1].SetItemState(PaginationItem.ItemState.Normal);
                            items[items.Length - 2].SetItemState(reverseOrder ? PaginationItem.ItemState.Second : PaginationItem.ItemState.SecondToLast);
                            items[items.Length - 1].SetItemState(reverseOrder ? PaginationItem.ItemState.First : PaginationItem.ItemState.Last);
                        }

                    }
                    else if (itemsState == 1)
                    {
                        if (value < 2)
                        {
                            items[0].SetItemState(PaginationItem.ItemState.Normal);
                            items[1].SetItemState(PaginationItem.ItemState.Normal);
                            items[items.Length - 2].SetItemState(reverseOrder ? PaginationItem.ItemState.Second : PaginationItem.ItemState.SecondToLast);
                            items[items.Length - 1].SetItemState(reverseOrder ? PaginationItem.ItemState.First : PaginationItem.ItemState.Last);

                            itemsState = 0;
                        }
                        else if (value >= galleryItemsCount - 2)
                        {
                            items[0].SetItemState(reverseOrder ? PaginationItem.ItemState.Last : PaginationItem.ItemState.First);
                            items[1].SetItemState(reverseOrder ? PaginationItem.ItemState.SecondToLast : PaginationItem.ItemState.Second);
                            items[items.Length - 2].SetItemState(PaginationItem.ItemState.Normal);
                            items[items.Length - 1].SetItemState(PaginationItem.ItemState.Normal);

                            itemsState = 2;
                        }
                        else
                        {
                            items[0].SetItemState(reverseOrder ? PaginationItem.ItemState.Last : PaginationItem.ItemState.First);
                            items[1].SetItemState(reverseOrder ? PaginationItem.ItemState.SecondToLast : PaginationItem.ItemState.Second);
                            items[items.Length - 2].SetItemState(reverseOrder ? PaginationItem.ItemState.Second : PaginationItem.ItemState.SecondToLast);
                            items[items.Length - 1].SetItemState(reverseOrder ? PaginationItem.ItemState.First : PaginationItem.ItemState.Last);
                        }
                    }
                    else if (itemsState == 2)
                    {
                        if (value < galleryItemsCount - (items.Length - 2))
                        {
                            items[items.Length - 2].SetItemState(reverseOrder ? PaginationItem.ItemState.Second : PaginationItem.ItemState.SecondToLast);
                            items[items.Length - 1].SetItemState(reverseOrder ? PaginationItem.ItemState.First : PaginationItem.ItemState.Last);

                            if (value >= 2)
                            {
                                items[0].SetItemState(reverseOrder ? PaginationItem.ItemState.Last : PaginationItem.ItemState.First);
                                items[1].SetItemState(reverseOrder ? PaginationItem.ItemState.SecondToLast : PaginationItem.ItemState.Second);

                                itemsState = 1;
                            }
                            else
                            {
                                items[0].SetItemState(PaginationItem.ItemState.Normal);
                                items[1].SetItemState(PaginationItem.ItemState.Normal);

                                itemsState = 0;
                            }
                        }
                        else
                        {
                            items[0].SetItemState(reverseOrder ? PaginationItem.ItemState.Last : PaginationItem.ItemState.First);
                            items[1].SetItemState(reverseOrder ? PaginationItem.ItemState.SecondToLast : PaginationItem.ItemState.Second);
                            items[items.Length - 2].SetItemState(PaginationItem.ItemState.Normal);
                            items[items.Length - 1].SetItemState(PaginationItem.ItemState.Normal);
                        }
                    }

                    if (itemsState == 0)
                    {
                        localSelectedIndex = value;

                        if (itemsTextType == PaginationItem.TextType.Numeric)
                        {
                            for (int i = 0; i < items.Length - 2; i++)
                            {
                                items[i].SetText((i + 1) + "");
                            }

                            items[items.Length - 2].SetText("...");
                            items[items.Length - 1].SetText(galleryItemsCount + "");
                        }
                        else if (itemsTextType == PaginationItem.TextType.Title)
                        {
                            if(hasGalleryManager && galleryManager)
                            {
                                for (int i = 0; i < items.Length - 2; i++)
                                {
                                    items[i].SetText(galleryManager.items[i].levelName);
                                }

                                items[items.Length - 2].SetText("...");
                                items[items.Length - 1].SetText(galleryManager.items[galleryItemsCount - 1].levelName);
                            }
                        }
                    }
                    else if (itemsState == 1)
                    {
                        localSelectedIndex += (value - gallerySelectedIndex);
                        localSelectedIndex = Mathf.Clamp(localSelectedIndex, 2, items.Length - 3);

                        if (itemsTextType == PaginationItem.TextType.Numeric)
                        {
                            int startNum = value - (localSelectedIndex - 3);
                            for (int i = 2; i < items.Length - 2; i++)
                            {
                                items[i].SetText((startNum) + "");
                                startNum++;
                            }

                            items[0].SetText(1 + "");
                            items[1].SetText("...");
                            items[items.Length - 2].SetText("...");
                            items[items.Length - 1].SetText(galleryItemsCount + "");
                        }
                        else if (itemsTextType == PaginationItem.TextType.Title)
                        {
                            if (hasGalleryManager && galleryManager)
                            {
                                int startNum = value - (localSelectedIndex - 3);
                                for (int i = 2; i < items.Length - 2; i++)
                                {
                                    items[i].SetText(galleryManager.items[startNum - 1].levelName);
                                    startNum++;
                                }

                                items[0].SetText(galleryManager.items[0].levelName);
                                items[1].SetText("...");
                                items[items.Length - 2].SetText("...");
                                items[items.Length - 1].SetText(galleryManager.items[galleryItemsCount - 1].levelName);
                            } 
                        }

                    }
                    else if (itemsState == 2)
                    {
                        localSelectedIndex = items.Length - (galleryItemsCount - value);

                        if (itemsTextType == PaginationItem.TextType.Numeric)
                        {
                            for (int i = items.Length - 1; i > 1; i--)
                            {
                                items[i].SetText((galleryItemsCount - (items.Length - 1 - i)) + "");
                            }

                            items[0].SetText(1 + "");
                            items[1].SetText("...");
                        }
                        else if (itemsTextType == PaginationItem.TextType.Title)
                        {
                            if (hasGalleryManager && galleryManager)
                            {
                                for (int i = items.Length - 1; i > 1; i--)
                                {
                                    items[i].SetText(galleryManager.items[galleryItemsCount - (items.Length - i)].levelName);
                                }

                                items[0].SetText(galleryManager.items[0].levelName);
                                items[1].SetText("...");
                            }
                        }
                    }
                }

                items[localSelectedIndex].SetItemState(PaginationItem.ItemState.Selected);
                items[localSelectedIndex].transform.SetSiblingIndex(items.Length - 1);
                gallerySelectedIndex = value;
            }

            mGallertItemsCount = galleryItemsCount;
            mReverseOrder = reverseOrder;
        }
    }

    private void OnPaginationItemsClick(int index)
    {
        int galleryItemsCount = totalItemsCount;

        if (hasGalleryManager && galleryManager)
        {
            galleryItemsCount = galleryManager.items.Count;
            
            if (items.Length == galleryItemsCount)
            {
                galleryManager.ShowItem(index, true);
            }
            else
            {
                if (index == 0)
                {
                    galleryManager.ShowItem(index, true);
                }
                else if (index == items.Length - 1)
                {
                    galleryManager.ShowItem(galleryItemsCount - 1, true);
                }
                else
                {
                    galleryManager.OnNavigationButtonsClick(index - localSelectedIndex);
                }
            }
        }
        else
        {
            if (items.Length == galleryItemsCount)
            {
                SetSelectedIndex(index);
            }
            else
            {
                if (index == 0)
                {
                    SetSelectedIndex(index);
                }
                else if (index == items.Length - 1)
                {
                    SetSelectedIndex(galleryItemsCount - 1);
                }
                else
                {
                    SetSelectedIndex(gallerySelectedIndex + (index - localSelectedIndex));
                }
            }

            Debug.Log("item " + gallerySelectedIndex + " clicked.");
        }
    }

    private void SwapIfVertical<T>(ref T var1, ref T var2)
    {
        if (layout == Layout.Vertical)
        {
            T temp = var1;
            var1 = var2;
            var2 = temp;
        }
    }

    private void SwapIfVertical(ref Vector2 var)
    {
        if (layout == Layout.Vertical)
        {
            float temp = var.x;
            var.x = var.y;
            var.y = temp;
        }
    }
}
