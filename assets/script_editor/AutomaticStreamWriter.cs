using System;
using System.IO;

public class StringWrittenEventArgs<T> : EventArgs
{
    public T Value
    {
        get;
        private set;
    }
    
    public StringWrittenEventArgs(T value)
    {
        this.Value = value;
    }
}

public class AutomaticStreamWriter : StreamWriter
{
    public event EventHandler<StringWrittenEventArgs<string>> StringWritten;
        
    public AutomaticStreamWriter(Stream s) : base(s)
    {
        
    }
    
    private void LaunchEvent(string text)
    {
        if (StringWritten != null)
        {
            StringWritten(this, new StringWrittenEventArgs<string>(text));
        }
    }
    
    public override void Write(string value)
    {
        base.Write(value);
        LaunchEvent(value);
    }
        
}