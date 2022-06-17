//-----------------------------------------------------------------------------
// <copyright file="DataPage.cs" company="SW Okolo IT">
// Copyright (c) SW Okolo IT. All rights reserved.
// </copyright>
// <summary>
// Содержит класс для хранения данных страницы раздела c данными.
// </summary>
//-----------------------------------------------------------------------------

namespace DwtReader.Components.Pages
{
    /// <summary> 
    /// Страница раздела c данными. 
    /// </summary>
    public class DataPage
    {
        /// 
        /// <summary> Номер страницы, начинается с 1. 
        /// </summary>
        public uint Number { get; set; }

        /// <summary>
        /// Размер данных для этой страницы (сжатый размер).
        /// </summary>
        public uint Size { get; set; }

        /// <summary> 
        /// Начальное смещение для этой страницы. 
        /// </summary>
        public ulong Offset { get; set; }
    }
}
