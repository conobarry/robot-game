using Godot;

interface ICodeObject
{
    string CodeName { get; set; }

    void Highlight()
    {
        
    }

    void RemoveHighlight();
}