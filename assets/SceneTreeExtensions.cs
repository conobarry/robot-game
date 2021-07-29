using Godot;

public static class SceneTreeExtensions
{
    public static Cursor GetCursor(this SceneTree sceneTree)
    {
        return (Cursor) sceneTree.GetNodesInGroup("cursor")[0];
    }

}
