using Godot;

public class WindowResizeHandle : Control
{
    
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        
    }

    private enum ResizeRegion: int
    {
        None = 0,
        Top = 1,
        Right = 2,
        TopRight = 3,
        Left = 4,
        TopLeft = 5,
        Bottom = 6,
        BottomRight = 8,
        BottomLeft = 10,
    }

    private ResizeRegion IsCursorInResizeRegion()
    {
        ResizeRegion xRegion = ResizeRegion.None;
        ResizeRegion yRegion = ResizeRegion.None;

        Vector2 mousePos = GetViewport().GetMousePosition();

        float x = mousePos.x;
        float y = mousePos.y;

        float margin = 5f;

        float left = RectPosition.x;
        float right = RectPosition.x + RectSize.x;
        float top = RectPosition.y;
        float bottom = RectPosition.y + RectSize.y;

        if ( (x > left && x < right) && (y > top && y < bottom) )
        {
            switch (x)
            {
                case float n when (n < left + margin):
                    xRegion = ResizeRegion.Left;
                    break;

                case float n when (n > right - margin):
                    xRegion = ResizeRegion.Right;
                    break;
            }

            switch (y)
            {
                case float n when (n < top + margin):
                    yRegion = ResizeRegion.Top;
                    break;

                case float n when (n > bottom - margin):
                    yRegion = ResizeRegion.Bottom;
                    break;
            }            
        }


        // GD.Print((ResizeRegion)((int)xRegion + (int)yRegion));

        return (ResizeRegion)((int)xRegion + (int)yRegion);
    }

    // public override void _GuiInput(InputEvent @event)    
    // {
    //     GD.Print("mouse");
    //     if (@event is InputEventMouseButton)
    //     {
    //         InputEventMouseButton mouseEvent = (InputEventMouseButton)@event;

    //         if (mouseEvent.ButtonIndex == (int)ButtonList.Left)
    //         {

    //             if (mouseEvent.Pressed)
    //             {
    //                 // isDragActive = true;
    //             }
    //             else
    //             {
    //                 // isDragActive = false;
    //             }
    //         }
    //     }
    //     else if (@event is InputEventMouseMotion)
    //     {
    //         InputEventMouseMotion mouseMotion = (InputEventMouseMotion)@event;            

    //         GD.Print("move");
    //         // if (isDragActive)
    //         // {
    //         //     OnDrag(mouseMotion);
    //         // }

    //         ResizeRegion region = IsCursorInResizeRegion();

    //         if (region != ResizeRegion.None)
    //         {
    //             Resize(region, mouseMotion.Relative);
    //         }
            
    //     }

    //     base._GuiInput(@event);
    // }

    private void Resize(ResizeRegion region, Vector2 mouseRelative)
    {
        switch (region)
        {
            case ResizeRegion.Top:

                break;
        }
    }

}
