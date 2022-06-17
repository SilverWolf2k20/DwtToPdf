//-----------------------------------------------------------------------------
// <copyright file="Block.cs" company="SW Okolo IT">
// Copyright (c) SW Okolo IT. All rights reserved.
// </copyright>
// <summary>
// Содержит класс с данными блока.
// </summary>
//-----------------------------------------------------------------------------

namespace DwtReader.Objects
{
    /// <summary>
    /// Данные блока.
    /// </summary>
    public class Block : Entity
    {
        /// <summary>
        /// Имя блока.
        /// </summary>
        public string Name { get; set; } = string.Empty;
    }
}
