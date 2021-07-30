using Godot;

public class ProgressMeter : CanvasLayer
{

    public int MaxValue
    {
        get { return _maxValue; }
        set
        {
            this._maxValue = value;
            Update();
        }
    }

    public int CurrentValue
    {
        get { return _currentValue; }
        set
        {
            this._currentValue = value;
            Update();
        }
    }

    public string Title
    {
        get { return _title; }
        set
        {
            this._title = value;
            Update();
        }
    }

    private int _maxValue = 0;

    private int _currentValue = 0;

    private string _title = "";

    private Label label;

    private ProgressBar progressBar;

    
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        label = (Label) FindNode("Label");
        progressBar = (ProgressBar) FindNode("ProgressBar");
    }

    public void _Init(string title, int currentValue, int maxValue)
    {
        this._title = title;
        this._currentValue = currentValue;
        this._maxValue = maxValue;
        Update();
    }

    public void Update()
    {
        label.Text = $"{_title} {_currentValue}/{_maxValue}";
        progressBar.Value = _currentValue / _maxValue * 100;
        GD.Print((_currentValue / _maxValue) * 100);
    }

}
