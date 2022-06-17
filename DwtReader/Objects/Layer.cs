//-----------------------------------------------------------------------------
// <copyright file="Layer.cs" company="SW Okolo IT">
// Copyright (c) SW Okolo IT. All rights reserved.
// </copyright>
// <summary>
// Содержит класс с данными слоя.
// </summary>
//-----------------------------------------------------------------------------

namespace DwtReader.Objects
{
    /// <summary>
    /// Данные слоя.
    /// </summary>
    public class Layer : Entity
    {
        /// <summary>
        /// Имя слоя.
        /// </summary>
        public string Name { get; set; } = string.Empty;
    }
}
