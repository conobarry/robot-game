using Godot;
using System.Collections.Generic;

interface ICodeObject<T> where T : Node
{
    string CodeName { get; }

    List<MeshInstance> HighlightMeshes { get; }

    Material HighlightMaterial { get; }

    void Highlight()
    {
        foreach (MeshInstance mesh in HighlightMeshes)
        {
            mesh.GetActiveMaterial(0).NextPass = HighlightMaterial;
        }
    }

    void RemoveHighlight()
    {
        foreach (MeshInstance mesh in HighlightMeshes)
        {
            mesh.GetActiveMaterial(0).NextPass = null;
        }
    }

    void OnMouseEnter()
    {
        Node node = (Node) this;
        Cursor cursor = (Cursor) node.GetTree().GetNodesInGroup("cursor")[0];
        cursor.TooltipUpper.Text = CodeName;

        Highlight();
    }

    void OnMouseExit()
    {
        Node node = (Node) this;
        Cursor cursor = (Cursor) node.GetTree().GetCursor();
        cursor.TooltipUpper.Text = "";

        RemoveHighlight();
    }

}