using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseFollowerItem : MonoBehaviour
{
    [SerializeField] Canvas canvas;
    [SerializeField] Camera cameraMain;
    [SerializeField] Ui_InvetoryItem invetoryItem;

    private void Awake()
    {
        canvas = transform.root.GetComponent<Canvas>();
        cameraMain = Camera.main;
        invetoryItem = GetComponentInChildren<Ui_InvetoryItem>();
    }
    public void SetData(Sprite sprite, int quantity)
    {
        invetoryItem.SetData(sprite, quantity);
    }
    private void Update()
    {
        Vector2 position;
        // chuyển đổi tọa độ của chuột trên màn hình sang tọa độ không gian
        // tham số rect: đối tượng rect mà bạn muốn chuyển đổi từ pos truyền vào sang pos của nó
        // tức là nó lấy rect làm mốc tọa độ
        // screenPoint: tọa độ trong không gian mành hình muốn chuyển đổi
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            (RectTransform)canvas.transform, Input.mousePosition, canvas.worldCamera, out position);
        transform.position = canvas.transform.TransformPoint(position); // chuyển từ tọa độ local sang global
    }
    public void Togge(bool val)
    {
        gameObject.SetActive(val); 
    }
}
