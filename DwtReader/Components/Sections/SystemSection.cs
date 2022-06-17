//-----------------------------------------------------------------------------
// <copyright file="SystemSection.cs" company="SW Okolo IT">
// Copyright (c) SW Okolo IT. All rights reserved.
// </copyright>
// <summary>
// Содержит класс для хранения данных системного раздела.
// </summary>
//-----------------------------------------------------------------------------

using DwtReader.Components.Pages;

namespace DwtReader.Components.Sections
{
    /// <summary> 
    /// Системный раздел. 
    /// </summary>
    public class SystemSection
    {
        /// <summary>
        /// Номер страницы раздела, начинается с 1, номера страниц уникальны
        /// для каждого файла.
        /// </summary>
        public int Number { get; set; } = default;

        /// <summary>
        /// Размер секции.
        /// </summary>
        public uint Size { get; set; } = default;

        /// <summary>
        /// Адрес секции.
        /// </summary>
        public uint Address { get; set; } = default;

        /// <summary>
        /// Страницы раздела данных.
        /// </summary>
        public SystemPage Page { get; set; } = new();
    }
}
