//-----------------------------------------------------------------------------
// <copyright file="VerticalAlignment.cs" company="SW Okolo IT">
// Copyright (c) SW Okolo IT. All rights reserved.
// </copyright>
// <summary>
// Содержит перечисление типов вертикального выравнивания.
// </summary>
//-----------------------------------------------------------------------------

namespace DwtReader.Objects.Componets.Text
{
    /// <summary>
    /// Типы вертикального выравнивания.
    /// </summary>
    public enum VerticalAlignment : byte
    {
        /// <summary>
        /// По базовой линии.
        /// </summary>
        Baseline = 0,

        /// <summary>
        /// По низу.
        /// </summary>
        Bottom = 1,

        /// <summary>
        /// Середина.
        /// </summary>
        Middle = 2,

        /// <summary>
        /// По верху.
        /// </summary>
        Top = 3
    }
}
