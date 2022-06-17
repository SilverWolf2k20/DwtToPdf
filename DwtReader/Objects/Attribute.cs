//-----------------------------------------------------------------------------
// <copyright file="Attribute.cs" company="SW Okolo IT">
// Copyright (c) SW Okolo IT. All rights reserved.
// </copyright>
// <summary>
// Содержит класс с данными атрибута.
// </summary>
//-----------------------------------------------------------------------------

namespace DwtReader.Objects
{
    /// <summary>
    /// Данные атрибута.
    /// </summary>
    public class Attribute : Text
    {
        /// <summary>
        /// Версия атрибута.
        /// </summary>
        public byte Version { get; set; } = default;
    }
}
