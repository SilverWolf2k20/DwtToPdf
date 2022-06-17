//-----------------------------------------------------------------------------
// <copyright file="MaterialType.cs" company="SW Okolo IT">
// Copyright (c) SW Okolo IT. All rights reserved.
// </copyright>
// <summary>
// Содержит перечисление типов материала.
// </summary>
//-----------------------------------------------------------------------------

namespace DwtReader.Objects.Componets
{
    public enum MaterialType
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
        /// Непрерывный.
        /// </summary>
        Continous,

        /// <summary>
        /// Определен в разделе дискрипторов.
        /// </summary>
        InHandlesSection
    }
}
