//-----------------------------------------------------------------------------
// <copyright file="PositoinAdjuster.cs" company="SW Okolo IT">
// Copyright (c) SW Okolo IT. All rights reserved.
// </copyright>
// <summary>
// Содержит класс для определения настрок для листа.
// </summary>
//-----------------------------------------------------------------------------

using DwtDrawer.Data;

using DwtReader.Objects;
using DwtReader.Objects.Componets;

namespace DwtDrawer.Components
{
    /// <summary>
    /// Класс для создания настроек листа.
    /// </summary>
    public class PositoinAdjuster
    {
        public PdfSetting Setting { get; private set; } = new PdfSetting();
        public bool NotFound { get; private set; } = default;

        /// <summary>
        /// Создает экземпляр класса PositoinAdjuster.
        /// </summary>
        public PositoinAdjuster()
        {
            Setting.OffsetX = int.MaxValue;
            Setting.OffsetY = int.MaxValue;
        }

        /// <summary>
        /// Подбирает формат и размер листа для чертежа.
        /// </summary>
        /// <param name="objects">Объекты чертежа.</param>
        /// <returns>True - если калибровка прошла успешно, иначе - false</returns>
        public bool Calibrate(List<(ObjectType type, Entity entity)> objects, uint id)
        {
            NotFound = true;

            if (objects is null || objects.Count == 0)
                return false;

            int maxX = -int.MaxValue;
            int maxY = -int.MaxValue;

            Setting.OffsetX = int.MaxValue;
            Setting.OffsetY = int.MaxValue;

            foreach (var entity in objects) {
                // Чтение сдвига по мультилиниям
                if (entity.type == ObjectType.LwPolyLine) {
                    var polyline = entity.entity as LwPolyLine;

                    if(polyline is null || polyline.Owner.reference != id)
                        continue;

                    NotFound = false;
                    for (int i = 0; i < polyline.NumberVertex - 1; ++i) {
                        Setting.OffsetX = Math.Min(Setting.OffsetX, (int)polyline.Points[i].x);
                        Setting.OffsetY = Math.Min(Setting.OffsetY, (int)polyline.Points[i].y);

                        maxX = Math.Max(maxX, (int)polyline.Points[i].x);
                        maxY = Math.Max(maxY, (int)polyline.Points[i].y);
                    }
                }
                // Чтение сдвига по линиям
                if (entity.type == ObjectType.Line) {
                    var line = entity.entity as Line;

                    if (line is null || line.Owner.reference != id)
                        continue;

                    NotFound = false;
                    Setting.OffsetX = Math.Min(Setting.OffsetX, (int)line.StartX);
                    Setting.OffsetY = Math.Min(Setting.OffsetY, (int)line.StartY);
                    Setting.OffsetX = Math.Min(Setting.OffsetX, (int)line.EndX);
                    Setting.OffsetY = Math.Min(Setting.OffsetY, (int)line.EndY);

                    maxX = Math.Max(maxX, (int)line.StartX);
                    maxY = Math.Max(maxY, (int)line.StartY);
                    maxX = Math.Max(maxX, (int)line.EndX);
                    maxY = Math.Max(maxY, (int)line.EndY);
                }
            }
            Setting.Width  = Math.Abs(maxX - Setting.OffsetX);
            Setting.Height = Math.Abs(maxY - Setting.OffsetY);

            DitermineSheetFormat(Setting.Width, Setting.Height);
            DitermineSheetOrientation(Setting.Width, Setting.Height);
            return true;
        }

        /// <summary>
        /// Определяет формат листа.
        /// </summary>
        /// <param name="width">Ширина чертежа</param>
        /// <param name="heigth">Высота чертежа</param>
        private void DitermineSheetFormat(int width, int heigth)
        {
            if (width <= 105 && heigth <= 148)
                Setting.Format = SheetFormat.A6;
            else if (width <= 148 && heigth <= 210)
                Setting.Format = SheetFormat.A5;
            else if (width <= 210 && heigth <= 297)
                Setting.Format = SheetFormat.A4;
            else if (width <= 297 && heigth <= 420)
                Setting.Format = SheetFormat.A3;
            else if (width <= 420 && heigth <= 594)
                Setting.Format = SheetFormat.A2;
            else if (width <= 594 && heigth <= 841)
                Setting.Format = SheetFormat.A1;
            else
                Setting.Format = SheetFormat.A0;
        }

        /// <summary>
        /// Определяет ориентацию листа.
        /// </summary>
        /// <param name="width">Ширина чертежа</param>
        /// <param name="heigth">Высота чертежа</param>
        private void DitermineSheetOrientation(int width, int heigth)
        {
            if (width > heigth)
                Setting.Orientation = SheetOrientation.LandScape;
            else
                Setting.Orientation = SheetOrientation.Bookstore;
        }
    }
}
