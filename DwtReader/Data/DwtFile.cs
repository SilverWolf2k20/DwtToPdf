//-----------------------------------------------------------------------------
// <copyright file="DwtFile.cs" company="SW Okolo IT">
// Copyright (c) SW Okolo IT. All rights reserved.
// </copyright>
// <summary>
// Содержит класс для хранения данных DWT файла.
// </summary>
//-----------------------------------------------------------------------------

using DwtReader.Components;
using DwtReader.Components.Sections;
using DwtReader.Objects;
using DwtReader.Objects.Componets;

namespace DwtReader.Data
{
    /// <summary>
    /// Данные DWT файла.
    /// </summary>
    public class DwtFile
    {
        public string FileName { get; set; } = string.Empty;

        // Мета данные.
        public DwtVersion Version       { get; set; } = DwtVersion.None;
        public byte MaintenanceVersion  { get; set; } = default;
        public uint PreviewImagePos     { get; set; } = default;
        public byte WriterVersion       { get; set; } = default;
        public byte ReleaseVersion      { get; set; } = default;
        public uint Codepage            { get; set; } = default;
        public uint SecurityFlag        { get; set; } = default;
        public uint SummaryInfoAddress  { get; set; } = default;
        public uint VBAProjectAddress   { get; set; } = default;

        // Зашифрованные мета данные.
        public uint SectionPageMapId        { get; set; } = default;
        public ulong SectionPageMapAddress  { get; set; } = default;
        public uint SectionMapId            { get; set; } = default;
        public uint SectionPageArraySize    { get; set; } = default;

        // Разделы
        public Dictionary<int, SystemSection> SystemSections     { get; set; } = new();
        public uint DataSectionsNumber                           { get; set; } = default;
        public Dictionary<TypeSection, DataSection> DataSections { get; set; } = new();

        // Объекты
        public List<(ObjectType type, Entity entity)> Objects { get; set; } = new();
    }
}
