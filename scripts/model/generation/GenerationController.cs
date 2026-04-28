using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using Godot;
using ProceduralFoliageGenerator.scripts.model;
using ProceduralFoliageGenerator.ViewModel;

namespace ProceduralFoliageGenerator.Model;

/// <summary>
/// Structure for storing all foliage rendering and generation related data.
/// </summary>
public record GenerationController
{
    public event EventHandler<string> BuildRequestedWhileNotReady;
    

    public GenerationCommandData CommandData { get; }
    public GenerationInfoData InfoData { get; }

    public String LastGeneratedConfig;

    public event EventHandler<string> ErrorOccured;
    
    public GenerationController()
    {
        CommandData = new GenerationCommandData();
        InfoData = new GenerationInfoData();

        CommandData.PathToSpeciesAttributesChanged += ((sender, args) =>
        {
            try
            {
                var content = File.ReadAllText(CommandData.PathToSpeciesAttributes);

                var list = PlantAttributeParser.Parse(content);
                if (list.Count > 0)
                    InfoData.PlantAttributes = list;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                ErrorOccured?.Invoke(sender, e.Message);
            }
        });

        CommandData.PathToSymbolSetChanged += ((sender, args) =>
        {
            try
            {
                var content =  File.ReadAllText(CommandData.PathToSymbolSet);
                
                var list = SymbolAttributesParser.Parse(content);
                if (list.Count > 0)
                    InfoData.SymbolAttributes = list;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                ErrorOccured?.Invoke(sender, e.Message);
            }
        });
    }

    public (bool,string[]) Execute()
    {
        string[] result = {};
        bool execute = CommandData.IsReady() && InfoData.IsWellFormed();
        
        if (execute)
        {
            GenerationDataCache.SaveFiles(CommandData);
            var (config,command )= GenerationCommandBuilder.Build(CommandData);
            result = command;
            LastGeneratedConfig = config;
        }
        else
        {
            ErrorOccured?.Invoke(this,"Execution requested while missing arguments");
        }
        return (execute,result);
    }
    
    public void Clear()
    {
        CommandData.Clear();
        InfoData.Clear();
    }
}