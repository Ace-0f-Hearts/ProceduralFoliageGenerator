using System;
using System.IO;
using System.Runtime.InteropServices.JavaScript;
using Godot;
using Godot.NativeInterop;
using Array = Godot.Collections.Array;
using FileAccess = Godot.FileAccess;

namespace ProceduralFoliageGenerator.ViewModel;


/// <summary>
/// Singleton class responsible for storing the input and output arguments of the generation.
/// The generation is executed on a different thread from the main.
/// </summary>
public partial class GenerationExecutor : Node
{
    
    

    private static GenerationExecutor _instance;
    private GodotThread _generatorThread;
    private Mutex _generatorMutex;

    /// <summary>
    /// TODO: Finalize generatorPath once its done
    /// </summary>
    private string _generatorPath = "/mnt/hobby-partition/Dev/ThesisWork/FoliageGen/build/debug/apps/App";
    private string[] _generatorArguments;

    private string _logPrefix = "genlog_";
    private string _loggingPath = "user://logs/generator/";
    private Array _outputLog;

    
    public event EventHandler GenerationSuccess;
    public event EventHandler GenerationFailure;
    
    
    /// <summary>
    /// Path to the executable foliage generator program.
    /// TODO: Finalize its path
    /// </summary>
    public string GeneratorPath
    {
        get { return _generatorPath; }
        private set
        {
            _generatorMutex.Lock();
            _generatorPath = value;
            _generatorMutex.Unlock();
        }
    }

    /// <summary>
    /// Arguments to be passed to the generator program.<br/>
    /// Requires:
    /// <list type="number">
    /// <item> Path to the input map file </item>
    /// <item> Path to the input species file </item>
    /// <item> Path to the output file </item>
    /// </list>
    /// </summary>
    public string[] GeneratorArguments
    {
        get { return _generatorArguments; }
        set
        {
            _generatorMutex.Lock();
            _generatorArguments = value;
            _generatorMutex.Unlock();
        }
    }

    /// <summary>
    /// An array holding the standard output and error passed to the terminal during execution of the generator program.
    /// Not used for persistent storage of the outputs, rather just as an intermediate solution until the values are logged and cleared.
    /// </summary>
    public Array OutputLog
    {
        get { return _outputLog; }
        private set {
            _generatorMutex.Lock();
            _outputLog = value;
            _generatorMutex.Unlock();
        }
    }
    
    /// <summary>
    /// Determines whether the standard output and error are logged after the execution of the procedural generator program.
    /// </summary>
    public bool SaveLogs { get; set; } = true;
    
    /// <summary>
    /// The error code returned by the generator program.
    /// 0 if no error was present, otherwise some kind of error occured.
    /// </summary>
    public int ErrorCode {get; private set;}
    
    /// <summary>
    /// Single instance of the <c>GenerationExecutor</c> class.
    /// </summary>
    static public GenerationExecutor Instance
    {
        get
        {
            if (_instance is null)
            {
                _instance = new GenerationExecutor();
            }
            return _instance;
        }
        private set 
        {
            _instance = value;
        }
    }
    
    /// <summary>
    /// Private constructor of the <c>GenerationExecutor</c> class.
    /// </summary>
    private GenerationExecutor()
    {
        _generatorMutex = new Mutex();
        _generatorThread = new GodotThread();
        OutputLog = new Array();

        var dir = DirAccess.Open("user://");
        dir.MakeDirRecursive(_loggingPath);

    }

    /// <summary>
    /// Locks the mutex associated with the execution thread of the generator. Used inside the execution thread.
    /// </summary>
    private void _ExecuteThread()
    {
        GD.Print("Generator thread started");
        _generatorMutex.Lock();
        ErrorCode = OS.Execute(GeneratorPath, GeneratorArguments, OutputLog, true);
        CallDeferred(MethodName._FinishThread);
        _generatorMutex.Unlock(); //TODO: Double check how the unlocking should work here

    }

    /// <summary>
    /// Handles the non-blocking wait on the execution of the generator program and calls logging.
    /// </summary>
    private void _FinishThread()
    {
        _generatorThread.WaitToFinish();
        GD.Print($"Generation Finished\nOutput Log:{OutputLog}\nError Code:{ErrorCode}");

        if (ErrorCode != 0)
        {
            GenerationFailure?.Invoke(this, EventArgs.Empty);
        }
        else
        {
            GenerationSuccess?.Invoke(this, EventArgs.Empty);
        }
        
        _generatorMutex.Lock();
        LogOutput();
        ClearArguments();
        ClearLogs();
        _generatorMutex.Unlock();
    }   
    
    /// <summary>
    /// Starts the thread for executing the generation.
    /// </summary>
    public void ExecuteGeneration()
    {
        Callable callableExec = new Callable(this,MethodName._ExecuteThread);
        
        var err = _generatorThread.Start(callableExec);
        GD.Print(err); 
    }

    /// <summary>
    /// Simple logging function for storing the contents resulting from the execution of the generator.
    /// TODO: Later on, if other logging tasks are necessary, make a global/centralized logging object
    /// </summary>
    private void LogOutput()
    {
        if (SaveLogs)
        {
            var time = Time.GetDatetimeStringFromSystem();
            var path = Path.Combine(_loggingPath, _logPrefix + time + ".log");
            var logFile = FileAccess.Open(path,FileAccess.ModeFlags.Write);
            
            GD.Print(path);
            if (logFile is not null)
            {
                foreach (string item in OutputLog)
                {
                    logFile.StoreLine(item);
                }
                logFile.Close();
            }
            else
            {
                GD.Print($"Error: {FileAccess.GetOpenError()}");
            }
        }
    }

    private void ClearArguments()
    {
        _generatorArguments = null;
    }

    private void ClearLogs()
    {
        OutputLog.Clear();
    }
    
    public override void _ExitTree()
    {
        _generatorThread.WaitToFinish();
        base._ExitTree();
    }
    
}