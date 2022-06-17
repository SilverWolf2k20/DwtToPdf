//-----------------------------------------------------------------------------
// <copyright file="MirrorFlag.cs" company="SW Okolo IT">
// Copyright (c) SW Okolo IT. All rights reserved.
// </copyright>
// <summary>
// Содержит перечисление типов отражения.
// </summary>
//-----------------------------------------------------------------------------

namespace DwtReader.Objects.Componets.Text
{
    /// <summary>
    /// Типы отражения.
    /// </summary>
    public enum MirrorFlag : byte
    {
        /// <summary>
        /// Нет отражения.
        /// </summary>
        None = 0,

        /// <summary>
        /// Слева направо.
        /// </summary>
        Backward = 2,

        /// <summary>
        /// Сверху вниз.
        /// </summary>
        UpsideDown  = 4
    }
}
