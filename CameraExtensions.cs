using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CameraExtensions
{
    /// <summary>
    /// ��������ӳ�䵽ָ��Canvas��AnchoredPosition
    /// </summary>
    public static Vector3 WorldToCanvasPoint(this Camera camera, RectTransform canvas, Vector3 worldPosition)
    {
        Vector2 offset = new Vector2(canvas.sizeDelta.x * 0.5f, (float)canvas.sizeDelta.y * 0.5f);//viewport��ԭ�������ģ�canvasԭ�������½�
        Vector2 viewportPosition = camera.WorldToViewportPoint(worldPosition);
        Vector2 canvasPoint = new Vector2(viewportPosition.x * canvas.sizeDelta.x, viewportPosition.y * canvas.sizeDelta.y) - offset;

        return canvasPoint;
    }
}
