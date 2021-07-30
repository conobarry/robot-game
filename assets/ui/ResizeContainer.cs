using Godot;
using System;

public class ResizeContainer : GridContainer
{
    public event EventHandler<InputEventMouseResize> Resize;

    private bool isResizing = false;


    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        foreach (Panel panel in GetChildren())
        {
            if (panel.Name != "Center")
            {
                Godot.Collections.Array args = new Godot.Collections.Array(panel);

                panel.Connect("mouse_entered", this, nameof(OnPanelMouseEntered), args);
                panel.Connect("mouse_exited", this, nameof(OnPanelMouseExited), args);
                panel.Connect("gui_input", this, nameof(OnPanelGUIInput), args);
            }
        }
    }

    private OrdinalDirection GetDirection(Panel panel)
    {
        return Enum.Parse<OrdinalDirection>(panel.Name);
    }

    private void OnPanelMouseEntered(Panel panel)
    {
        OrdinalDirection direction = GetDirection(panel);
        CursorType cursorType;

        switch (direction)
        {
            case OrdinalDirection.North:
            case OrdinalDirection.South:
                cursorType = CursorType.ResizeNS;
                break;

            case OrdinalDirection.East:
            case OrdinalDirection.West:
                cursorType = CursorType.ResizeEW;
                break;

            case OrdinalDirection.NorthEast:
            case OrdinalDirection.SouthWest:
                cursorType = CursorType.ResizeNESW;
                break;

            case OrdinalDirection.NorthWest:
            case OrdinalDirection.SouthEast:
                cursorType = CursorType.ResizeNWSE;
                break;

            default:
                cursorType = CursorType.Default;
                break;
        }

        GetTree().GetCursor().SetCursor(cursorType);
    }
    
    private void OnPanelMouseExited(Panel panel)
    {
        GetTree().GetCursor().SetCursor(CursorType.Default);
    }

    private void OnPanelGUIInput(InputEvent @event, Panel panel)
    {
        if (@event is InputEventMouseButton)
        {
            InputEventMouseButton mouseEvent = (InputEventMouseButton) @event;

            if (mouseEvent.ButtonIndex == (int) ButtonList.Left)
            {
                if (mouseEvent.Pressed)
                {
                    isResizing = true;
                }
                else
                {
                    isResizing = false;
                }
            }
        }
        else if (@event is InputEventMouseMotion)
        {
            InputEventMouseMotion mouseMotion = (InputEventMouseMotion)@event;            

            if (isResizing)
            {
                InputEventMouseResize resizeEvent = new InputEventMouseResize(mouseMotion);
                resizeEvent.Direction = GetDirection(panel);

                OnResize(resizeEvent);
            }
            
        }

        base._GuiInput(@event);
    }

    protected virtual void OnResize(InputEventMouseResize args)
    {
        EventHandler<InputEventMouseResize> handler = Resize;
        if (handler != null)
        {
            handler(this, args);
        }
    }

}
