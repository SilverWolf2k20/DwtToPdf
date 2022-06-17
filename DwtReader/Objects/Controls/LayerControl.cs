//-----------------------------------------------------------------------------
// <copyright file="LayerControl.cs" company="SW Okolo IT">
// Copyright (c) SW Okolo IT. All rights reserved.
// </copyright>
// <summary>
// Содержит класс с данными слоя управления.
// </summary>
//-----------------------------------------------------------------------------

using DwtReader.Objects.Componets;

namespace DwtReader.Objects.Controls
{
    /// <summary>
    /// Данные слоя управления.
    /// </summary>
    public class LayerControl : Entity
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
