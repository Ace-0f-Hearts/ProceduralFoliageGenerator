using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using Godot;
using ProceduralFoliageGenerator.ViewModel;

namespace ProceduralFoliageGenerator.Model;

/// <summary>
/// Structure for storing all foliage rendering and generation related data.
/// </summary>
public record GenerationController
{
    public event EventHandler PlantAttributesSet;
    public event EventHandler<string> BuildRequestedWhileNotReady;

    public GenerationCommandData CommandData { get; } = new();
    public GenerationInfoData InfoData { get; } = new();

    public String LastGeneratedConfig;
    
    
    public GenerationController()
    {
        
    }

    public string[] Execute()
    {
        string[] result = {};        
        
        if (CommandData.IsReady())
        {
            GenerationDataCache.SaveFiles(CommandData);
            var (config,command )= GenerationCommandBuilder.Build(CommandData);
            result = command;
            LastGeneratedConfig = config;
        }
        else
        {
            
        }
        return result;
    }
    
    public void Clear()
    {
        CommandData.Clear();
        InfoData.Clear();
    }
}