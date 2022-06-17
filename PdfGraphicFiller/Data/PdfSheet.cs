//-----------------------------------------------------------------------------
// <copyright file="PdfSheet.cs" company="SW Okolo IT">
// Copyright (c) SW Okolo IT. All rights reserved.
// </copyright>
// <summary>
// Содержит класс для хранения данных листа.
// </summary>
//-----------------------------------------------------------------------------

namespace DwtDrawer.Data
{
    /// <summary>
    /// Данные листа.
    /// </summary>
    public class PdfSheet
    {
        public string Name { get; set; } = string.Empty;
        public uint ID { get; set; } = default;
        public PdfSetting Setting { get; set; } = new();
    }
}
