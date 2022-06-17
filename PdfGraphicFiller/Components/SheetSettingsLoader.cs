//-----------------------------------------------------------------------------
// <copyright file="SheetSettingsLoader.cs" company="SW Okolo IT">
// Copyright (c) SW Okolo IT. All rights reserved.
// </copyright>
// <summary>
// Содержит класс для загрузки настроек листов.
// </summary>
//-----------------------------------------------------------------------------

using System.Xml.Linq;

namespace DwtDrawer.Components
{
    /// <summary>
    /// Настройки листа.
    /// </summary>
    public class SheetSettings
    {
        public string? FormatName  { get; set; } = string.Empty;
        public string? PlotterType { get; set; } = string.Empty;
        public int Width  { get; set; } = default;
        public int Height { get; set; } = default;
    }

    /// <summary>
    /// Загрузчик настроек листов.
    /// </summary>
    internal class SheetSettingsLoader
    {
        /// <summary>
        /// Настройки листов.
        /// </summary>
        public List<SheetSettings> Settings { get; private set; } = new();

        /// <summary>
        /// Считывает данные из файла.
        /// </summary>
        /// <param name="filePath">Путь к файлу настроек</param>
        /// <returns>True - если чтение прошло успешно, иначе - false</returns>
        public bool LoadFromFile(string filePath)
        {
            try {
                var document = XDocument.Load(filePath);
                var sheet = document.Element("sheet");

                if (sheet is null)
                    return false;

                foreach (var format in sheet.Elements("format")) {
                    Settings.Add(new SheetSettings()
                    {
                        FormatName  = format?.Attribute("name")?.Value,
                        PlotterType = format?.Element("plotter")?.Value,
                        Width  = int.Parse(format.Element("width")?.Value),
                        Height = int.Parse(format.Element("height")?.Value)
                    });
                }
            }
            catch (Exception) {
                return false;
            }
            return true;
        }
    }
}
