//-----------------------------------------------------------------------------
// <copyright file="LineAndPlotType.cs" company="SW Okolo IT">
// Copyright (c) SW Okolo IT. All rights reserved.
// </copyright>
// <summary>
// Содержит перечисление типов печати или линии.
// </summary>
//-----------------------------------------------------------------------------

namespace DwtReader.Objects.Componets
{
    /// <summary>
    /// Типы печати или линии.
    /// </summary>
    public enum LineAndPlotType
    {
        /// <summary>
        /// Как в слое.
        /// </summary>
        ByLayer,

        /// <summary>
        /// Как в блоке.
        /// </summary>
        ByBlock,

        /// <summary>
        /// Глобальное.
        /// </summary>
        Global,

        /// <summary>
        /// Определен в разделе дискрипторов.
        /// </summary>
        InHandlesSection
    }
}
