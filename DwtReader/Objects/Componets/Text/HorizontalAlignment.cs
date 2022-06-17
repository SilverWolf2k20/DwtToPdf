//-----------------------------------------------------------------------------
// <copyright file="HorizontalAlignment.cs" company="SW Okolo IT">
// Copyright (c) SW Okolo IT. All rights reserved.
// </copyright>
// <summary>
// Содержит перечисление типов горизонтального выравнивания.
// </summary>
//-----------------------------------------------------------------------------

namespace DwtReader.Objects.Componets.Text
{
    /// <summary>
    /// Типы горизонтального выравнивания.
    /// </summary>
    public enum HorizontalAlignment : byte
    {
        /// <summary>
        /// По левому краю (По умолчанию).
        /// </summary>
        Left = 0,

        /// <summary>
        /// По центру.
        /// </summary>
        Center = 1,

        /// <summary>
        /// По правому краю.
        /// </summary>
        Right = 2,

        /// <summary>
        /// Выравнено.
        /// </summary>
        Aligned = 3,

        /// <summary>
        /// Середина.
        /// </summary>
        Middle = 4,

        /// <summary>
        /// Широкий.
        /// </summary>
        Fit = 5
    }
}
