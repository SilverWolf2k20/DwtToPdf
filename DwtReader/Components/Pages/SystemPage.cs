//-----------------------------------------------------------------------------
// <copyright file="SystemPage.cs" company="SW Okolo IT">
// Copyright (c) SW Okolo IT. All rights reserved.
// </copyright>
// <summary>
// Содержит класс хранения данных системной страницы раздела.
// </summary>
//-----------------------------------------------------------------------------

namespace DwtReader.Components.Pages
{
    /// <summary> 
    /// Системная страница раздела. 
    /// </summary>
    public class SystemPage
    {
        /// <summary> 
        /// Дискриптор родителя. 
        /// </summary>
        public uint Parent { get; set; }
        /// <summary> 
        /// Лево? 
        /// </summary>
        public uint Left { get; set; }
        /// <summary> 
        /// Право? 
        /// </summary>
        public uint Right { get; set; }
    }
}
