//-----------------------------------------------------------------------------
// <copyright file="BlockControl.cs" company="SW Okolo IT">
// Copyright (c) SW Okolo IT. All rights reserved.
// </copyright>
// <summary>
// Содержит класс с данными блока управления.
// </summary>
//-----------------------------------------------------------------------------

using DwtReader.Objects.Componets;

namespace DwtReader.Objects.Controls
{
    /// <summary>
    /// Данные блока управления.
    /// </summary>
    public class BlockControl : Entity
    {
        /// <summary>
        /// Количество сущностей.
        /// </summary>
        public uint NumberEntries { get; set; } = default;

        /// <summary>
        /// Список дескрипторов вложенных сущностей.
        /// </summary>
        public List<ObjectHandle> Handles { get; set; } = new();
    }
}
