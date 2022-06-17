//-----------------------------------------------------------------------------
// <copyright file="AttributeDefine.cs" company="SW Okolo IT">
// Copyright (c) SW Okolo IT. All rights reserved.
// </copyright>
// <summary>
// Содержит класс с данными определенного атрибута.
// </summary>
//-----------------------------------------------------------------------------

namespace DwtReader.Objects
{
    /// <summary>
    /// Данные определенного атрибута.
    /// </summary>
    public class AttributeDefine : Attribute
    {
        /// <summary>
        /// Название атрибута.
        /// </summary>
        public string Prompt { get; set; } = string.Empty;
    }
}
