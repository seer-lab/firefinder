using System;
using System.IO;
using System.Linq;
using UnityEngine;
using System.Text;
using IronPython.Hosting;
using Microsoft.Scripting.Hosting;

public class Interpreter
{
    public ScriptScope Scope;

    private ScriptEngine Engine;

    private ScriptSource Source;

    private CompiledCode Compiled;

    public Interpreter()
    {
        Engine = Python.CreateEngine();
        Scope  = Engine.CreateScope();

        // Execute code containing predefined python functions
        Engine.ExecuteFile("Assets/PythonLibrary/lib.py", Scope);
    }

    public string Compile(string src, Microsoft.Scripting.SourceCodeKind CodeKind =
                                      Microsoft.Scripting.SourceCodeKind.Statements)
    {
        if(src == string.Empty) {
            Debug.Log("Code is empty");
            return string.Empty;
        }

        LoadRuntime();

        Source = Engine.CreateScriptSourceFromString(src, CodeKind);

        MemoryStream stream = new MemoryStream();
        //Set IO Ouput of execution
        Engine.Runtime.IO.SetOutput(stream, new StreamWriter(stream));

        Compiled  = Source.Compile();

        try {
            Compiled.Execute(Scope);
            return FormatOutput(ReadFromStream(stream));

        } catch(Exception ex) {
            return Engine.GetService<ExceptionOperations>().FormatException(ex);
        }
    }

    private string FormatOutput(string output)
    {
        return string.IsNullOrEmpty(output) ? string.Empty 
        :      string.Join("\n", output.Remove(output.Length-1)
                                       .Split('\n')
                                       .Reverse().ToArray());
    }

    private string ReadFromStream(MemoryStream ms) {

        int length = (int)ms.Length;
        Byte[] bytes = new Byte[ms.Length];
        ms.Seek(0, SeekOrigin.Begin);
        ms.Read(bytes, 0, length);

        return Encoding.GetEncoding("utf-8").GetString(bytes, 0, length);
    }

    private void LoadRuntime()
    {
        Engine.Runtime.LoadAssembly(typeof(GameObject).Assembly);
    }

}
