using UnityEngine;
using UnityEngine.UIElements;

public class DragAndDrop
{
    public static VisualElement dragingElement;
    public static void DragStart(VisualElement _element)
    {
        dragingElement = _element;
        dragingElement.CaptureMouse();
    }
    public static void Dragging(MouseMoveEvent _evt)
    {
        if (dragingElement != null)
        {
            // Update the position of the dragged element based on the cursor position and the drag offset
            Vector2 newPosition = _evt.mousePosition - Constants.DRAG_OFFSET;// _dragOffset;
            dragingElement.style.left = newPosition.x;
            dragingElement.style.top = newPosition.y;
            _evt.StopPropagation();
        }
    }
    public static void ReleaseDrag()
    {
        dragingElement.ReleaseMouse();
        dragingElement = null;
    }
}