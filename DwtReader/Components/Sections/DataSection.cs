//-----------------------------------------------------------------------------
// <copyright file="DataSection.cs" company="SW Okolo IT">
// Copyright (c) SW Okolo IT. All rights reserved.
// </copyright>
// <summary>
// Содержит класс для хранения данных раздела с данными.
// </summary>
//-----------------------------------------------------------------------------

using DwtReader.Components.Pages;

namespace DwtReader.Components.Sections
{
    /// <summary>
    /// Раздел с данными.
    /// </summary>
    public class DataSection
    {
        /// <summary>
        /// Размер раздела.
        /// </summary>
        public ulong Size { get; set; } = default;

        /// <summary>
        /// Количество страниц.
        /// </summary>
        public uint PageCount { get; set; } = default;

        /// <summary>
        /// Максимальный распакованный размер страницы раздела этого типа.
        /// </summary>
        /// <remarks>Обычно 0x7400.</remarks>
        public uint MaxSize { get; set; } = default;

        /// <summary>
        /// Сжатый (1 = нет, 2 = да).
        /// </summary>
        /// <remarks>Обычно 2.</remarks>
        public uint IsCompressed { get; set; } = default;

        /// <summary>
        /// Идентификатор раздела (начинается с 0). Первая секция 
        /// имеет номер 0, последующие секции нумеруются в порядке 
        /// убывания от (количество секций – 1) до 1.
        /// </summary>
        public uint Id { get; set; } = default;

        /// <summary>
        /// Зашифровано (0 = нет, 1 = да, 2 = неизвестно).
        /// </summary>
        public uint IsEncrypted { get; set; } = default;

        /// <summary>
        /// Имя секции.
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Страницы раздела.
        /// </summary>
        public Dictionary<uint, DataPage> Pages { get; set; } = new();

        /// <summary>
        /// Конвертация в перечисление.
        /// </summary>
        public TypeSection ToEnum() => Name switch {
            "AcDb:Header"           => TypeSection.Header,
            "AcDb:Classes"          => TypeSection.Classes,
            "AcDb:SummaryInfo"      => TypeSection.SumaryInfo,
            "AcDb:Preview"          => TypeSection.Preview,
            "AcDb:VBAProject"       => TypeSection.VbaProject,
            "AcDb:AppInfo"          => TypeSection.AppInfo,
            "AcDb:FileDepList"      => TypeSection.FileDepList,
            "AcDb:RevHistory"       => TypeSection.RevHistory,
            "AcDb:Security"         => TypeSection.Security,
            "AcDb:AcDbObjects"      => TypeSection.Objects,
            "AcDb:ObjFreeSpace"     => TypeSection.ObjFreeSpace,
            "AcDb:Template"         => TypeSection.Template,
            "AcDb:Handles"          => TypeSection.Handles,
            "AcDb:AcDsPrototype_1b" => TypeSection.Prototype,
            "AcDb:AuxHeader"        => TypeSection.AuxHeader,
            "AcDb:Signature"        => TypeSection.Signature,
            "AcDb:AppInfoHistory"   => TypeSection.AppInfoHistory,
            _ => TypeSection.Unknowns,
        };
    }
}
