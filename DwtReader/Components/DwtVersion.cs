//-----------------------------------------------------------------------------
// <copyright file="DwtVersion.cs" company="SW Okolo IT">
// Copyright (c) SW Okolo IT. All rights reserved.
// </copyright>
// <summary>
// Содержит перечисление всех версий файлов формата DWT.
// </summary>
//-----------------------------------------------------------------------------

namespace DwtReader.Components
{
    /// <summary>
    /// Версии файла формата DWT.
    /// </summary>
    public enum DwtVersion
    {
        None,
        AC1014,
        AC1015, // R2000
        AC1018, // R2004
        AC1021, // R2007
        AC1024, // R2010
        AC1027, // R2013 <- необходимая версия
        AC1032, // R2018
    }
}
