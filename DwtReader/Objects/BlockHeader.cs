//-----------------------------------------------------------------------------
// <copyright file="BlockHeader.cs" company="SW Okolo IT">
// Copyright (c) SW Okolo IT. All rights reserved.
// </copyright>
// <summary>
// Содержит класс с данными заголовка блока.
// </summary>
//-----------------------------------------------------------------------------

using DwtReader.Objects.Componets;

namespace DwtReader.Objects
{
    /// <summary>
    /// Данные заголовка блока.
    /// </summary>
    public class BlockHeader : Entity
    {
        /// <summary>
        /// Имя блока.
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Флаг.
        /// </summary>
        public int Flag { get; set; } = default;

        /// <summary>
        /// Количество сущностей.
        /// </summary>
        public uint ObjectCount { get; set; } = default;
        public double X { get; set; } = default;
        public double Y { get; set; } = default;
        public double Z { get; set; } = default;

        /// <summary>
        /// Список дескрипторов.
        /// </summary>
        public List<ObjectHandle> Handles { get; set; } = new();
    }
}
