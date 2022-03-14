// See https://aka.ms/new-console-template for more information
using KPatcher.Source;
using KPatcher.Source.Patcher;

Console.WriteLine("Hello, World!");

Patcher.Run("tslpatchdata/changes.ini", "gamedir", 0, false);
