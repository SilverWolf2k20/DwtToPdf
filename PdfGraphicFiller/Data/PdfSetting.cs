//-----------------------------------------------------------------------------
// <copyright file="PdfSetting.cs" company="SW Okolo IT">
// Copyright (c) SW Okolo IT. All rights reserved.
// </copyright>
// <summary>
// Содержит класс настроек для pdf.
// </summary>
//-----------------------------------------------------------------------------

namespace DwtDrawer.Data
{
    /// <summary>
    /// Настройки для pdf.
    /// </summary>
    public class PdfSetting
    {
        public PdfSetting() { }

        public PdfSetting(PdfSetting setting)
        {
            Orientation = setting.Orientation;
            Format  = setting.Format;
            OffsetX = setting.OffsetX;
            OffsetY = setting.OffsetY;
            Width   = setting.Width;
            Height  = setting.Height;
        }
        public SheetOrientation Orientation { get; set; } = SheetOrientation.Bookstore;
        public SheetFormat Format { get; set; } = SheetFormat.A4;
        public int OffsetX { get; set; } = default;
        public int OffsetY { get; set; } = default;
        public int Width   { get; set; } = default;
        public int Height  { get; set; } = default;
    }
}
