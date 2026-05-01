using System;
using System.IO;
using ProceduralFoliageGenerator.scripts.model;

namespace ProceduralFoliageGenerator.Model;

/// <summary>
///     Structure for storing all foliage rendering and generation related data.
/// </summary>
public record GenerationController
{
    public string LastGeneratedConfig { get; set; }

    public GenerationController()
    {
        CommandData = new GenerationCommandData();
        InfoData = new GenerationInfoData();

        CommandData.PathToSpeciesAttributesChanged += (sender, args) =>
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
        };

        CommandData.PathToSymbolSetChanged += (sender, args) =>
        {
            try
            {
                var content = File.ReadAllText(CommandData.PathToSymbolSet);

                var list = SymbolAttributesParser.Parse(content);
                if (list.Count > 0)
                    InfoData.SymbolAttributes = list;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                ErrorOccured?.Invoke(sender, e.Message);
            }
        };
    }


    public GenerationCommandData CommandData { get; }
    public GenerationInfoData InfoData { get; }
    public event EventHandler BuildRequestedWhileNotReady;

    public event EventHandler<string> ErrorOccured;

    public (bool, string[]) Execute()
    {
        string[] result = { };
        var execute = CommandData.IsReady() && InfoData.IsWellFormed();

        if (execute)
        {
            GenerationDataCache.SaveFiles(CommandData);
            var (config, command) = GenerationCommandBuilder.Build(CommandData);
            result = command;
            LastGeneratedConfig = config;
        }
        else
        {
            BuildRequestedWhileNotReady?.Invoke(this, EventArgs.Empty);
            ErrorOccured?.Invoke(this, "Execution requested while missing arguments");
        }

        return (execute, result);
    }

    public void Clear()
    {
        CommandData.Clear();
        InfoData.Clear();
    }
}