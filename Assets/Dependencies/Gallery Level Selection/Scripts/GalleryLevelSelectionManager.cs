using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class GalleryLevelSelectionManager : MonoBehaviour
{
    public List<GalleryLevelView> items = new List<GalleryLevelView>(0);
    public Transform galleryChild, itemsContainer;
    public bool hasScaleTransition = false;
    public float minScaleDownValue = 0.8f;
    public bool linearScale = false;
    public bool hasDisplacementTransition = false;
    public DisplacementType displacementType = DisplacementType.Normal;
    public float maxDisplacementPos = 0;
    public float itemsWidth = 300;
    public float itemsHeight = 300;
    public float spaceBetweenItems = 20.0f;
    public float normalAnimateSpeed = 30f;
    public ScrollAxis scrollAxis = ScrollAxis.Horizontal;
    public bool reverseOrder = false;
    public int activeItems = 5;
    public int start_level = 0;
    public bool hasRotationOverZ = false;
    public bool linearRotation = false;
    public float maxRotationZ = 45;
    public bool infiniteLoop = false;
    public bool activeEditorAutoManaging = true;
    public bool hasColorTransition = false;
    public bool linearColor = false;
    public Color transitionColorStart = Color.white;
    public Color transitionColorEnd = Color.white;
    public HorizontalAlignment horizontalAlignment = HorizontalAlignment.Center;
    public VerticalAlignment verticalAlignment = VerticalAlignment.Center;
    public IndicatorType indicatorType = IndicatorType.None;
    public Slider slider;
    public Scrollbar scrollbar;
    public PaginationView pagination;
    public LevelTitleFormat levelTitleFormat;
    public Text txtLevelTitle;
    public int autoGenerateItemsCount = 0;
    public bool autoGenerateAddImageUI = true;
    public bool autoGenerateAddText = true;
    public bool freeSwipeColliderSize = false;
    public bool navigateToClickedItems = true;
    public bool snapItems = true;
    public Action<int, bool> OnItemsClickedEvent;
    public float moveThreshold = 0.05f;

    private Collider2D[] itemsCollider;
    private float[] itemsMappedPostion;
    private int[] activeItemsArray = new int[0];
    private float mItemsWidth;
    private float mItemsHeight;
    private Collider2D galleryCollider;
    private Collider2D sliderCollider;
    private Collider2D itemClickedDown;
    private bool isTouchOnGallery = false;
    private bool isTouchedOnSlider = false;
    private bool isScrollAnimating = false;
    private int mActiveItems;
    private int clickedLevelIndex;
    private float scrollValueOnMouseDown;
    private float scrollStep;
    private float endOfItemsScrollPosition;
    private Coroutine animateScrollCoroutine;
    private Vector2 galleryChildPosOnMouseDown;
    private Vector3 touchPosOnMouseDown;
    private Vector3 touchPosWorldOnMouseDown;
    private float itemsAlignmentPosition = 0;
    private bool isOverlayCanvas = false;
    private float circularDisplacementY;
    private float circularDisplacementR;

    public enum IndicatorType { None, Slider, Scrollbar, Pagination }
    public enum ScrollAxis { Horizontal, Vertical }
    public enum LevelTitleFormat { None, Number, NumberAndCount, LevelName }
    public enum HorizontalAlignment { Center, Left, Right }
    public enum VerticalAlignment { Center, Top, Bottom }
    public enum DisplacementType { Normal, Linear, Circular }

    public virtual void Start()
    {
        if (Application.isPlaying)
        {
            Initialize();
            Canvas canvas = GetTopmostCanvas(gameObject);
            isOverlayCanvas = false;
            if (canvas && canvas.renderMode == RenderMode.ScreenSpaceOverlay) isOverlayCanvas = true;
        }
    }

    public virtual void Update()
    {
#if UNITY_EDITOR
        if (!Application.isPlaying && activeEditorAutoManaging)
        {
            Initialize();
        }
#endif
        if (items.Count == 0) return;

        if (Input.GetMouseButtonDown(0))
        {
            OnMouseButtonDown();
        }
        else if (Input.GetMouseButton(0))
        {
            OnMouseButtonMove();
        }
        else if (Input.GetMouseButtonUp(0))
        {
            OnMouseButtonUp();
        }
    }

    public virtual void OnItemsClicked(int index, bool isCenter)
    {

        Debug.Log("Item " + index + " clicked");

        if (!isCenter && navigateToClickedItems)
        {
            if (animateScrollCoroutine != null)
            {
                isScrollAnimating = false;
                StopCoroutine(animateScrollCoroutine);
            }

            animateScrollCoroutine = StartCoroutine(AnimateScroll(index));
        }

        if (OnItemsClickedEvent != null) OnItemsClickedEvent(index, isCenter);

        if (index == items.Count) index = 0;
        if (index < 0) index = items.Count - index;
    }

    public virtual void ShowItem(int number, bool animate)
    {
        if (animate)
        {
            if (infiniteLoop || (number >= 0 && number < items.Count))
            {
                if (animateScrollCoroutine != null)
                {
                    isScrollAnimating = false;
                    StopCoroutine(animateScrollCoroutine);
                }
                animateScrollCoroutine = StartCoroutine(AnimateScroll(number));
            }
        }
        else
        {
            setHorizontalNormalizedPosition((number * scrollStep));
            UpdateGalleryStateOnIndicator();
            SnapScrollIndex();
        }
    }

    public virtual void OnNavigationButtonsClick(int value)
    {
        if (value == 0) return;

        int centerIndex = GetCenterItemIndex();

        int target = centerIndex + value;

        ShowItem(target, true);
    }

    public int GetCurrentNearestItemIndex()
    {
        return GetInfiniteLoopIndex(GetCenterItemIndex(true));
    }

    public int GetCenterFixedItemIndex()
    {
        if (!isScrollAnimating && !isTouchOnGallery && !isTouchedOnSlider) return GetCurrentNearestItemIndex();
        else return -1;
    }

    public virtual void UpdateGalleryStateOnIndicator()
    {
        if (indicatorType == IndicatorType.Slider && slider)
        {
            //slider.SetValueWithoutNotify(GetGalleryProgress());
            slider.value = GetGalleryProgress();
        }
        else if (indicatorType == IndicatorType.Scrollbar && scrollbar)
        {
            //scrollbar.SetValueWithoutNotify(GetGalleryProgress());
            scrollbar.value = GetGalleryProgress();
        }
        else if (indicatorType == IndicatorType.Pagination && pagination)
        {
            pagination.SetSelectedIndex(GetInfiniteLoopIndex(GetCenterItemIndex(true)));
        }
    }

    public void OnSliderValueChanged()
    {
        setHorizontalNormalizedPosition(slider.value);

        if (!isScrollAnimating)
            OnGalleryPositionChange();
    }

    public void OnScrollbarValueChanged()
    {
        setHorizontalNormalizedPosition(scrollbar.value);

        if (!isScrollAnimating)
            OnGalleryPositionChange();
    }

    public void AutoGenerateItems()
    {
        if (autoGenerateItemsCount <= 0) return;

        List<GalleryLevelView> temp = new List<GalleryLevelView>(items.Count + autoGenerateItemsCount);

        for (int i = 0; i < items.Count; i++)
        {
            temp[i] = items[i];
        }

        for (int i = 0; i < autoGenerateItemsCount; i++)
        {
            GameObject obj = new GameObject("item " + (items.Count + 1 + i));
            obj.transform.SetParent(itemsContainer.transform);
            obj.AddComponent<RectTransform>();
            GalleryLevelView levelViewItem = obj.AddComponent<GalleryLevelView>();
            BoxCollider2D boxCollider = obj.AddComponent<BoxCollider2D>();
            SetRectSize(obj.GetComponent<RectTransform>(), itemsWidth, itemsHeight);
            boxCollider.size = new Vector2(itemsWidth, itemsHeight);
            levelViewItem.manager = this;
            levelViewItem.levelName = "item " + (items.Count + 1 + i);

            if (autoGenerateAddImageUI)
            {
                GameObject img = new GameObject("image");
                img.transform.SetParent(obj.transform);
                Image image = img.AddComponent<Image>();
                SetRectSize(img.GetComponent<RectTransform>(), itemsWidth, itemsHeight);
                levelViewItem.image = image;
            }

            if (autoGenerateAddText)
            {
                GameObject txt = new GameObject("txt");
                txt.transform.SetParent(obj.transform);
                Text textComp = txt.AddComponent<Text>();
                textComp.text = (items.Count + 1 + i) + "";
                textComp.fontSize = 24;
                textComp.alignment = TextAnchor.MiddleCenter;
                textComp.color = Color.black;
                txt.transform.localPosition = Vector3.zero;
                levelViewItem.text = textComp;
            }

            temp[items.Count + i] = levelViewItem;
        }

        items = temp;
        autoGenerateItemsCount = 0;
    }

#if UNITY_EDITOR
    public void ClearAllItems()
    {
        List<GalleryLevelView> levels = items;
        Undo.RecordObject(this, "delete");

        items = new List<GalleryLevelView>(0);

        for (int i = 0; i < levels.Count; i++)
        {
            GameObject obj = levels[i].gameObject;
            Undo.DestroyObjectImmediate(obj);
        }
    }
#endif
    private void Initialize()
    {
        itemsWidth = Mathf.Abs(itemsWidth);
        itemsHeight = Mathf.Abs(itemsHeight);
        mItemsWidth = itemsWidth;
        mItemsHeight = itemsHeight;
        SwapIfVertical(ref mItemsWidth, ref mItemsHeight);

        if (items.Count == 0) return;
        mActiveItems = Mathf.Clamp(activeItems, 4, items.Count);
        if (mActiveItems % 2 == 1) mActiveItems -= 1;
        if (mActiveItems < 4) mActiveItems = 4;

        if (infiniteLoop && items.Count < 4)
        {
            infiniteLoop = false;
            Debug.LogError("Can't support loop with less than 4 items.");
        }

        if (!hasScaleTransition) minScaleDownValue = 1;
        if (!hasDisplacementTransition) maxDisplacementPos = 0;
        minScaleDownValue = Mathf.Clamp(minScaleDownValue, 0, 1);

        if (items.Count > 1)
        {
            scrollStep = (float)1 / (items.Count - 1);
        }
        else scrollStep = 1;

        if (displacementType == DisplacementType.Circular)
        {
            CalculateCircularDisplacement();
        }

        galleryCollider = GetComponent<Collider2D>();
        SetGallerySwipeCollider();

        if (indicatorType == IndicatorType.Slider && slider)
        {
            sliderCollider = slider.GetComponent<Collider2D>();
            if (!sliderCollider) Debug.LogError("The slider has no Collider!!!");
        }
        else if (indicatorType == IndicatorType.Scrollbar && scrollbar)
        {
            sliderCollider = scrollbar.GetComponent<Collider2D>();
            if (!sliderCollider) Debug.LogError("The scrollbar has no Collider!!!");
        }
        else if (indicatorType == IndicatorType.Pagination && pagination)
        {
            pagination.hasGalleryManager = true;
            pagination.galleryManager = this;
        }

        itemsAlignmentPosition = 0;

        Vector2 pivot = new Vector2(0.5f, 0.5f);
        if (scrollAxis == ScrollAxis.Horizontal)
        {
            if (verticalAlignment == VerticalAlignment.Top) pivot.y = 1;
            else if (verticalAlignment == VerticalAlignment.Bottom) pivot.y = 0;

            itemsAlignmentPosition = (pivot.y - 0.5f) * itemsHeight;
        }
        else if (scrollAxis == ScrollAxis.Vertical)
        {
            if (horizontalAlignment == HorizontalAlignment.Right) pivot.x = 1;
            else if (horizontalAlignment == HorizontalAlignment.Left) pivot.x = 0;

            itemsAlignmentPosition = (pivot.x - 0.5f) * itemsWidth;
        }

        float itemsCnt = (float)items.Count;
        itemsCollider = new Collider2D[items.Count];
        for (int i = 0; i < items.Count; i++)
        {
            itemsCollider[i] = items[i].GetComponent<Collider2D>();

            if (!itemsCollider[i]) itemsCollider[i] = items[i].gameObject.AddComponent<BoxCollider2D>();

            itemsCollider[i].GetComponent<BoxCollider2D>().size = new Vector2(itemsWidth, itemsHeight);
            itemsCollider[i].GetComponent<BoxCollider2D>().offset = new Vector2((0.5f - pivot.x) * itemsWidth, (0.5f - pivot.y) * itemsHeight);

            items[i].GetComponent<GalleryLevelView>().index = i;
            RectTransform itemRect = items[i].GetComponent<RectTransform>();
            if (itemRect) items[i].GetComponent<RectTransform>().pivot = pivot;
            else items[i].gameObject.AddComponent<RectTransform>();
            items[i].transform.localScale = new Vector3(minScaleDownValue, minScaleDownValue, 1);
            items[i].transform.eulerAngles = new Vector3(0, 0, 0);
            items[i].gameObject.SetActive(false);
        }

        CalculateItemsMappedPosition();

        endOfItemsScrollPosition = (mItemsWidth + spaceBetweenItems) * (items.Count);

        start_level = Mathf.Clamp(start_level, 0, items.Count - 1);
        ShowItem(start_level, false);
    }

    private float GetGalleryProgress()
    {
        float scroll = horizontalNormalizedPosition;
        if (!infiniteLoop || (scroll >= 0 && scroll <= 1))
        {
            return Mathf.Clamp(scroll, 0, 1.0f);
        }
        else
        {
            int centerIndex = (int)(scroll / scrollStep);
            if ((Mathf.Abs(scroll) - (centerIndex * scrollStep) >= ((centerIndex + 1) * scrollStep) - Mathf.Abs(scroll)))
            {
                return Mathf.Abs(1 - Mathf.Clamp(scroll, 0, 1.0f));
            }
            else
            {
                return Mathf.Clamp(scroll, 0, 1.0f);
            }
        }
    }

    private void OnMouseButtonDown()
    {
        Vector2 touchPosWorld;
        if (isOverlayCanvas) touchPosWorld = Input.mousePosition;
        else touchPosWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 touchPosScreen = Input.mousePosition;

        itemClickedDown = null;
        clickedLevelIndex = -1;
        Collider2D[] hit = Physics2D.OverlapPointAll(touchPosWorld);

        int centerIndex = GetCenterItemIndex();
        int start = centerIndex - mActiveItems / 2;
        int end = start + mActiveItems;

        if (!infiniteLoop || items.Count < mActiveItems)
        {
            start = Mathf.Clamp(start, 0, items.Count - 1);
            end = Mathf.Clamp(end, 0, items.Count - 1);
        }

        isTouchOnGallery = false;
        isTouchedOnSlider = false;
        int distToCenter = 1000;

        foreach (Collider2D h in hit)
        {

            if (h == galleryCollider)
            {
                isTouchOnGallery = true;
                scrollValueOnMouseDown = horizontalNormalizedPosition;
                touchPosOnMouseDown = Input.mousePosition;
                touchPosWorldOnMouseDown = touchPosWorld;
                galleryChildPosOnMouseDown = galleryChild.transform.localPosition;
                if (animateScrollCoroutine != null)
                {
                    StopCoroutine(animateScrollCoroutine);
                }

                continue;
            }

            if ((indicatorType == IndicatorType.Slider || indicatorType == IndicatorType.Scrollbar) && h == sliderCollider)
            {
                isTouchedOnSlider = true;
                continue;
            }

            for (int i = start; i <= end; i++)
            {
                int tempI = GetInfiniteLoopIndex(i);

                if (h == itemsCollider[tempI])
                {
                    if (Mathf.Abs(centerIndex - i) < distToCenter)
                    {
                        distToCenter = Mathf.Abs(centerIndex - i);
                        itemClickedDown = itemsCollider[tempI];
                        clickedLevelIndex = i;
                    }
                }
            }

        }

        if ((indicatorType == IndicatorType.Slider || indicatorType == IndicatorType.Scrollbar) && sliderCollider == Physics2D.OverlapPoint(touchPosScreen))
        {
            isTouchedOnSlider = true;
        }
    }

    private void OnMouseButtonMove()
    {
        if (isTouchOnGallery && !isTouchedOnSlider)
        {
            Vector2 touchPos = Input.mousePosition;
            Vector2 touchPosWorld;
            if (isOverlayCanvas) touchPosWorld = Input.mousePosition;
            else touchPosWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            if (Vector2.Distance(touchPosWorld, touchPosWorldOnMouseDown) >= moveThreshold)
            {
                float xPos = galleryChildPosOnMouseDown.x + (touchPos.x - touchPosOnMouseDown.x);
                if (scrollAxis == ScrollAxis.Vertical) xPos = galleryChildPosOnMouseDown.y + (touchPos.y - touchPosOnMouseDown.y);

                if (!infiniteLoop)
                {
                    if (reverseOrder)
                        xPos = Mathf.Clamp(xPos, -(mItemsWidth + spaceBetweenItems), endOfItemsScrollPosition);
                    else
                        xPos = Mathf.Clamp(xPos, -endOfItemsScrollPosition, mItemsWidth + spaceBetweenItems);
                }

                Vector2 pos = new Vector2(xPos, itemsAlignmentPosition);
                SwapIfVertical(ref pos);
                galleryChild.transform.localPosition = pos;
                OnGalleryPositionChange();
            }
        }
    }

    private void OnMouseButtonUp()
    {
        Vector2 touchPosWorld;
        if (isOverlayCanvas) touchPosWorld = Input.mousePosition;
        else touchPosWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (isTouchedOnSlider)
        {
            isTouchedOnSlider = false;
            if (snapItems) SnapScrollIndex();
        }
        else if (isTouchOnGallery)
        {
            isTouchOnGallery = false;
            float scroll = horizontalNormalizedPosition;
            if (scroll >= 0 && scroll <= 1 && scrollValueOnMouseDown != scroll)
            {
                if (snapItems) SnapScrollIndex();
            }
            else if (itemClickedDown != null && Vector2.Distance(touchPosWorld, touchPosWorldOnMouseDown) <= moveThreshold)
            {
                if (!isScrollAnimating)
                {
                    if (GetInfiniteLoopIndex(clickedLevelIndex) == GetCenterItemIndex())
                    {
                        OnItemsClicked(GetInfiniteLoopIndex(clickedLevelIndex), true);
                        return;
                    }
                }

                OnItemsClicked(clickedLevelIndex, false);
            }
            else
            {
                if (!snapItems && infiniteLoop)
                    return;

                SnapScrollIndex();
            }
        }
    }

    private void SnapScrollIndex()
    {
        int centerIndex = GetCenterItemIndex();

        if (infiniteLoop)
        {
            float scroll = horizontalNormalizedPosition;
            if (scroll < 0 && (Mathf.Abs(scroll) - (centerIndex * scrollStep) >= ((centerIndex + 1) * scrollStep) - Mathf.Abs(scroll)))
            {
                centerIndex = -1;
            }
        }

        if (animateScrollCoroutine != null)
        {
            StopCoroutine(animateScrollCoroutine);
            isScrollAnimating = false;
        }

        animateScrollCoroutine = StartCoroutine(AnimateScroll(centerIndex));
    }

    private IEnumerator AnimateScroll(int index)
    {
        isScrollAnimating = true;
        float pos = index * scrollStep;
        float dist = pos - horizontalNormalizedPosition;
        if (infiniteLoop) pos = GetInfiniteLoopIndex(index) * scrollStep;

        int frame = (int)normalAnimateSpeed;
        float step = 0;
        if (Mathf.Abs(dist) <= scrollStep / 2)
        {
            frame /= 2;
        }

        step = dist / frame;

        for (int i = 0; i < frame; i++)
        {
            setHorizontalNormalizedPosition(horizontalNormalizedPosition + step);
            UpdateGalleryStateOnIndicator();
            OnGalleryPositionChange();
            yield return null;
        }

        setHorizontalNormalizedPosition(pos);
        OnGalleryPositionChange();
        UpdateGalleryStateOnIndicator();

        isScrollAnimating = false;
    }

    private void OnGalleryPositionChange()
    {
        if (items.Count == 0) return;

        float scroll = horizontalNormalizedPosition;

        if (infiniteLoop)
        {
            if ((int)(scroll / scrollStep) + 1 > items.Count)
            {
                setHorizontalNormalizedPosition(0);
                itemsContainer.localPosition = new Vector2(0, 0);
                galleryChildPosOnMouseDown = galleryChild.transform.localPosition;
                touchPosOnMouseDown = Input.mousePosition;
                scroll = 0;
            }
            else if ((int)(scroll / scrollStep) < 0)
            {
                setHorizontalNormalizedPosition(1);
                galleryChildPosOnMouseDown = galleryChild.transform.localPosition;
                touchPosOnMouseDown = Input.mousePosition;
                scroll = 1;
            }
        }
        else
            scroll = Mathf.Clamp(horizontalNormalizedPosition, 0, 1);

        int centerIndex = GetCenterItemIndex();

        int start = centerIndex - mActiveItems / 2;
        int end = start + mActiveItems;

        if (!infiniteLoop || items.Count < mActiveItems)
        {
            start = Mathf.Clamp(start, 0, items.Count - 1);
            end = Mathf.Clamp(end, 0, items.Count - 1);
        }
        else if (infiniteLoop && end - start == items.Count)
        {
            start += 1;
        }

        bool showTextNumber = false;
        Color color = Color.black;
        if (levelTitleFormat != LevelTitleFormat.None)
        {
            color = txtLevelTitle.color;
            UpdateTextIndicator(scroll, centerIndex);
        }

        float totalRange = 1;
        if (items.Count > 1)
        {
            totalRange = (mActiveItems / 2) * scrollStep;
        }

        float[] itemsScale = new float[end - start + 1];
        float[] itemsProgress = new float[end - start + 1];
        float[] itemsLinearProgress = new float[end - start + 1];
        float[] itemsScaledWidth = new float[end - start + 1];

        for (int i = start; i <= end; i++)
        {
            int tempI = GetInfiniteLoopIndex(i);

            float scale = Mathf.Abs(scroll - (i * scrollStep));
            float progress = scale / totalRange;
            float linearProgress = Mathf.Abs(scroll - (i * scrollStep)) / scrollStep;
            if (linearProgress > 1) linearProgress = 1;

            if (linearScale)
            {
                scale = minScaleDownValue;
                if (linearProgress < 1)
                {
                    scale = 1 - ((1 - minScaleDownValue) * linearProgress);
                }
            }
            else
            {
                scale = minScaleDownValue + ((1 - progress) * (1 - minScaleDownValue));
            }

            scale = Mathf.Clamp(scale, minScaleDownValue, 1.0f);
            itemsProgress[i - start] = progress;
            itemsLinearProgress[i - start] = linearProgress;
            itemsScale[i - start] = scale;
            itemsScaledWidth[i - start] = scale * mItemsWidth;
            items[tempI].transform.localScale = new Vector3(scale, scale, 1);

            if (levelTitleFormat != LevelTitleFormat.None)
            {
                if (Mathf.Abs(scroll - (i * scrollStep)) < scrollStep / 3)
                {
                    showTextNumber = true;
                    color.a = 1 - (Mathf.Abs(scroll - (i * scrollStep)) / scrollStep) * 3;
                }
                else if (!showTextNumber)
                {
                    color.a = 0;
                }
            }
        }

        if (items.Count == 1)
            color.a = 1;

        if (levelTitleFormat != LevelTitleFormat.None)
        {
            txtLevelTitle.color = color;
        }

        Vector2[] itemsPos = new Vector2[end - start + 1];
        float firstItemPos = 0;
        int order = (reverseOrder) ? -1 : 1;

        if (!infiniteLoop || start >= 0)
        {
            if (start > 0)
            {
                firstItemPos = ((start - 1) * (((mItemsWidth) * minScaleDownValue) + spaceBetweenItems)) + (((mItemsWidth / 2) * minScaleDownValue) + spaceBetweenItems) + ((mItemsWidth / 2) * itemsScale[0]);
            }

            itemsPos[0].x = firstItemPos * order;

            for (int i = 1; i <= end - start; i++)
            {
                itemsPos[i].x = itemsPos[i - 1].x + (itemsScaledWidth[i - 1] / 2 + (mItemsWidth / 2) * itemsScale[i] + spaceBetweenItems) * order;
            }
        }
        else
        {
            if (Mathf.Abs(start) > mActiveItems / 2)
            {
                firstItemPos = ((start - 1) * (((mItemsWidth) * minScaleDownValue) + spaceBetweenItems)) + (((mItemsWidth / 2) * minScaleDownValue) + spaceBetweenItems) + ((mItemsWidth / 2) * itemsScale[Mathf.Abs(start)]);
            }

            itemsPos[Mathf.Abs(start)].x = firstItemPos * order;

            for (int i = Mathf.Abs(start) + 1; i <= end - start; i++)
            {
                itemsPos[i].x = itemsPos[i - 1].x + (itemsScaledWidth[i - 1] / 2 + (mItemsWidth / 2) * itemsScale[i] + spaceBetweenItems) * order;
            }

            for (int i = Mathf.Abs(start) - 1; i >= 0; i--)
            {
                itemsPos[i].x = itemsPos[i + 1].x - (itemsScaledWidth[i + 1] / 2 + (mItemsWidth / 2) * itemsScale[i] + spaceBetweenItems) * order;
            }
        }

        HideNotActiveItems(start, end);

        activeItemsArray = new int[end - start + 1];

        int siblingIndex = items.Count - (end - start + 1);
        centerIndex = GetCenterItemIndex(true);

        Vector2 containerPos = MapItemsPostion(scroll, centerIndex, order);
        float centerXPos = ((scrollAxis == ScrollAxis.Horizontal) ? galleryChild.transform.localPosition.x + containerPos.x : galleryChild.transform.localPosition.y + containerPos.y);

        for (int i = start; i <= end; i++)
        {
            int tempI = GetInfiniteLoopIndex(i);

            if (displacementType == DisplacementType.Circular)
            {
                float y = GetYPosOfCircularDisplacement(itemsPos[i - start].x + centerXPos);
                itemsPos[i - start].y = y;
            }
            else
            {
                itemsPos[i - start].y = (displacementType == DisplacementType.Linear ? itemsLinearProgress[i - start] : itemsProgress[i - start]) * maxDisplacementPos;
            }

            SwapIfVertical(ref itemsPos[i - start]);
            items[tempI].transform.localPosition = itemsPos[i - start];
            items[tempI].UpdateProgress(itemsProgress[i - start], itemsLinearProgress[i - start]);
            if (hasRotationOverZ)
                items[tempI].transform.eulerAngles = new Vector3(0, 0, (linearRotation ? itemsLinearProgress[i - start] : itemsProgress[i - start]) * maxRotationZ * Mathf.Sign(scroll - (i * scrollStep)));

            if (spaceBetweenItems < 0)
            {
                if (i == centerIndex)
                {
                    items[tempI].transform.SetSiblingIndex(items.Count - 1);
                    siblingIndex = items.Count - 2;
                }
                else if (i < centerIndex)
                {
                    items[tempI].transform.SetSiblingIndex(siblingIndex);
                    siblingIndex++;
                }
                else if (i > centerIndex)
                {
                    items[tempI].transform.SetSiblingIndex(siblingIndex);
                    siblingIndex--;
                }
            }

            if (!items[tempI].gameObject.activeInHierarchy)
                items[tempI].gameObject.SetActive(true);
            activeItemsArray[i - start] = tempI;
        }

        UpdateGalleryStateOnIndicator();
    }

    private void UpdateTextIndicator(float scroll, int centerIndex)
    {
        if (scroll < 0 && (Mathf.Abs(scroll) - (centerIndex * scrollStep) >= ((centerIndex + 1) * scrollStep) - Mathf.Abs(scroll)))
        {
            if (levelTitleFormat == LevelTitleFormat.NumberAndCount)
                txtLevelTitle.text = items.Count + "/" + items.Count;
            if (levelTitleFormat == LevelTitleFormat.Number)
                txtLevelTitle.text = items.Count + "";
            if (levelTitleFormat == LevelTitleFormat.LevelName)
                txtLevelTitle.text = items[items.Count - 1].levelName;
        }
        else
        {
            if (levelTitleFormat == LevelTitleFormat.NumberAndCount)
                txtLevelTitle.text = ((centerIndex % items.Count) + 1) + "/" + items.Count;
            if (levelTitleFormat == LevelTitleFormat.Number)
                txtLevelTitle.text = ((centerIndex % items.Count) + 1) + "";
            if (levelTitleFormat == LevelTitleFormat.LevelName)
                txtLevelTitle.text = items[(centerIndex % items.Count)].levelName;
        }
    }

    private void HideNotActiveItems(int start, int end)
    {
        foreach (int i in activeItemsArray)
        {
            bool notInActives = true;
            for (int j = start; j <= end; j++)
            {
                if (GetInfiniteLoopIndex(j) == i)
                {
                    notInActives = false;
                    break;
                }
            }

            if (notInActives && i < items.Count)
                items[i].gameObject.SetActive(false);
        }
    }

    private Vector2 MapItemsPostion(float scroll, int centerIndex, int order)
    {
        Vector2 pos1;
        if (items.Count <= 1) pos1 = Vector2.zero;
        else
        {
            centerIndex = (int)(scroll / scrollStep) + 1;
            if (!infiniteLoop && centerIndex >= items.Count) centerIndex = items.Count - 1;
            float previousPos = itemsMappedPostion[centerIndex - 1];

            if (!infiniteLoop || (centerIndex >= 0 && centerIndex < items.Count - 1))
            {
                pos1 = new Vector2((itemsMappedPostion[centerIndex] - itemsMappedPostion[centerIndex - 1]) * (1 - (Mathf.Abs(scroll - ((centerIndex) * scrollStep)) / scrollStep)) + previousPos, 0);
            }
            else
            {
                pos1 = new Vector2((mItemsWidth * (1 - minScaleDownValue)) * (1 - (Mathf.Abs(scroll - (centerIndex * scrollStep)) / scrollStep)) + previousPos, 0);
            }
            pos1.x = pos1.x * order;
            SwapIfVertical(ref pos1);
        }

        itemsContainer.localPosition = pos1;
        return pos1;
    }

    private int GetInfiniteLoopIndex(int index)
    {
        int temp = index;
        if (infiniteLoop)
        {
            if (index < 0)
                temp = items.Count + index;
            else if (index >= items.Count)
                temp = index - items.Count;
        }

        return temp;
    }

    private void CalculateItemsMappedPosition()
    {
        itemsMappedPostion = new float[items.Count];

        float totalRange = (mActiveItems / 2) * scrollStep;
        float scale = scrollStep;
        float s = scale / totalRange;
        scale = 1 - (minScaleDownValue + ((1 - s) * (1 - minScaleDownValue)));

        float prePos = 0;
        if (linearScale) prePos = -(1 - minScaleDownValue) * (mItemsWidth / 2);

        for (int i = 1; i < items.Count; i++)
        {
            float pos = 0;
            if (i <= mActiveItems / 2)
            {
                if (items.Count > 1)
                    if (linearScale)
                    {
                        pos = (1 - minScaleDownValue) * mItemsWidth;
                    }
                    else
                    {
                        pos = (scale * (mItemsWidth / 2)) * Mathf.Clamp((2 * i - 1), 0, mActiveItems);
                    }
            }
            else pos = mItemsWidth * (1 - minScaleDownValue);

            itemsMappedPostion[i] = pos + prePos;
            prePos += pos;
        }
    }

    private int GetCenterItemIndex(bool getReal = false)
    {
        float scroll = horizontalNormalizedPosition;
        if (!infiniteLoop) scroll = Mathf.Clamp(horizontalNormalizedPosition, 0, 1);

        int index = (int)(scroll / scrollStep);

        if (scroll >= 0)
        {
            if (scroll - (index * scrollStep) >= ((index + 1) * scrollStep) - scroll)
            {
                index += 1;
            }
        }
        else if (getReal)
        {
            if (Mathf.Abs(scroll) - (index * scrollStep) >= ((index + 1) * scrollStep) - Mathf.Abs(scroll))
            {
                index -= 1;
            }
        }

        if (scrollStep == 0) index = 0;

        return index;
    }

    private void SetGallerySwipeCollider()
    {
        if (!freeSwipeColliderSize)
        {
            Vector2 colliderSize;

            if (!hasDisplacementTransition || (scrollAxis == ScrollAxis.Vertical && horizontalAlignment == HorizontalAlignment.Center) || (scrollAxis == ScrollAxis.Horizontal && verticalAlignment == VerticalAlignment.Center))
            {
                colliderSize = new Vector2((mActiveItems + 1) * (mItemsWidth + spaceBetweenItems),
                    mItemsHeight / 2 + Mathf.Abs(maxDisplacementPos) + ((mItemsHeight / 2) * minScaleDownValue));
            }
            else if ((scrollAxis == ScrollAxis.Vertical && horizontalAlignment == HorizontalAlignment.Left) || (scrollAxis == ScrollAxis.Horizontal && verticalAlignment == VerticalAlignment.Bottom))
            {
                if (maxDisplacementPos > 0)
                {
                    colliderSize = new Vector2((mActiveItems + 1) * (mItemsWidth + spaceBetweenItems),
                        maxDisplacementPos + (mItemsHeight * minScaleDownValue));
                }
                else
                {
                    colliderSize = new Vector2((mActiveItems + 1) * (mItemsWidth + spaceBetweenItems),
                        Mathf.Abs(maxDisplacementPos) + mItemsHeight);
                }
            }
            else
            {
                if (maxDisplacementPos > 0)
                {
                    colliderSize = new Vector2((mActiveItems + 1) * (mItemsWidth + spaceBetweenItems),
                        maxDisplacementPos + mItemsHeight);
                }
                else
                {
                    colliderSize = new Vector2((mActiveItems + 1) * (mItemsWidth + spaceBetweenItems),
                        Mathf.Abs(maxDisplacementPos) + (mItemsHeight * minScaleDownValue));
                }
            }

            if (colliderSize.y < mItemsHeight) colliderSize.y = mItemsHeight;
            Vector2 colliderOffset = new Vector2(0, ((colliderSize.y - mItemsHeight) / 2) * Mathf.Sign(maxDisplacementPos));
            SwapIfVertical(ref colliderSize);
            SwapIfVertical(ref colliderOffset);
            GetComponent<BoxCollider2D>().size = colliderSize;
            GetComponent<BoxCollider2D>().offset = colliderOffset;
        }
    }

    private void CalculateCircularDisplacement()
    {
        if (maxDisplacementPos == 0)
        {
            return;
        }

        float totalRange = 1;
        if (items.Count > 1)
        {
            totalRange = (mActiveItems / 2) * scrollStep;
        }

        float[] itemsScale = new float[mActiveItems / 2 + 1];
        float[] itemsScaledWidth = new float[mActiveItems / 2 + 1];

        for (int i = 0; i <= mActiveItems / 2; i++)
        {
            float scale = Mathf.Abs(0 - (i * scrollStep));
            float progress = scale / totalRange;
            float linearProgress = Mathf.Abs(0 - (i * scrollStep)) / scrollStep;
            if (linearProgress > 1) linearProgress = 1;

            if (linearScale)
            {
                scale = minScaleDownValue;
                if (linearProgress < 1)
                {
                    scale = 1 - ((1 - minScaleDownValue) * linearProgress);
                }
            }
            else
            {
                scale = minScaleDownValue + ((1 - progress) * (1 - minScaleDownValue));
            }

            scale = Mathf.Clamp(scale, minScaleDownValue, 1.0f);

            itemsScale[i] = scale;
            itemsScaledWidth[i] = scale * mItemsWidth;
        }

        float[] itemsPos = new float[mActiveItems / 2 + 1];
        itemsPos[0] = 0;

        for (int i = 1; i <= mActiveItems / 2; i++)
        {
            itemsPos[i] = itemsPos[i - 1] + (itemsScaledWidth[i - 1] / 2 + (mItemsWidth / 2) * itemsScale[i] + spaceBetweenItems);
        }

        float x2 = itemsPos[mActiveItems / 2];
        if (Mathf.Abs(maxDisplacementPos) > Mathf.Abs(x2)) maxDisplacementPos = Mathf.Abs(x2) * Mathf.Sign(maxDisplacementPos);
        float y2 = maxDisplacementPos;
        float x3 = -x2, y3 = y2;

        float A = (x2 * y3) - (x3 * y2);
        float C = ((x2 * x2 + y2 * y2) * x3) + ((x3 * x3 + y3 * y3) * (-x2));

        circularDisplacementY = -(C / (2 * A));
        circularDisplacementR = Mathf.Abs(circularDisplacementY);
        circularDisplacementR = Mathf.Clamp(circularDisplacementR, Mathf.Abs(x2), circularDisplacementR);
    }

    private float GetYPosOfCircularDisplacement(float x)
    {
        if (maxDisplacementPos == 0) return 0;
        float b = -2 * circularDisplacementY;
        float d1 = (b * b) - 4 * (x * x);
        if (d1 < 0) d1 = 0;
        d1 = Mathf.Sqrt(d1);
        float r1 = (-b + d1) / 2;
        float r2 = (-b - d1) / 2;

        return (Mathf.Abs(r1) < Mathf.Abs(r2)) ? r1 : r2;
    }

    private float horizontalNormalizedPosition
    {
        get
        {
            if (items.Count > 1)
            {
                int order = reverseOrder ? -1 : 1;
                float value = ((scrollAxis == ScrollAxis.Horizontal) ? galleryChild.transform.localPosition.x : galleryChild.transform.localPosition.y) / -((mItemsWidth + spaceBetweenItems) * (items.Count - 1));
                return value * order;
            }
            else
                return 0;
        }
    }

    private void setHorizontalNormalizedPosition(float value)
    {
        int order = reverseOrder ? -1 : 1;
        Vector2 pos = new Vector2(-((mItemsWidth + spaceBetweenItems) * (items.Count - 1) * value) * order, itemsAlignmentPosition);
        if (items.Count <= 1) pos.x = 0;
        SwapIfVertical(ref pos);
        galleryChild.transform.localPosition = pos;
    }

    private void SetRectSize(RectTransform rect, float newWidth, float newHeight)
    {
        Vector2 newSize = new Vector2(newWidth, newHeight);
        Vector2 oldSize = rect.rect.size;
        Vector2 deltaSize = newSize - oldSize;
        rect.offsetMin = rect.offsetMin - new Vector2(deltaSize.x * rect.pivot.x, deltaSize.y * rect.pivot.y);
        rect.offsetMax = rect.offsetMax + new Vector2(deltaSize.x * (1f - rect.pivot.x), deltaSize.y * (1f - rect.pivot.y));
    }

    private void SwapIfVertical<T>(ref T var1, ref T var2)
    {
        if (scrollAxis == ScrollAxis.Vertical)
        {
            T temp = var1;
            var1 = var2;
            var2 = temp;
        }
    }

    private void SwapIfVertical(ref Vector2 var)
    {
        if (scrollAxis == ScrollAxis.Vertical)
        {
            float temp = var.x;
            var.x = var.y;
            var.y = temp;
        }
    }

    private Canvas GetTopmostCanvas(GameObject component)
    {
        Canvas[] parentCanvases = component.GetComponentsInParent<Canvas>();
        if (parentCanvases != null && parentCanvases.Length > 0)
        {
            return parentCanvases[parentCanvases.Length - 1];
        }
        return null;
    }

}
