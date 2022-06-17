//-----------------------------------------------------------------------------
// <copyright file="TextAlignment.cs" company="SW Okolo IT">
// Copyright (c) SW Okolo IT. All rights reserved.
// </copyright>
// <summary>
// Содержит класс для опредления координат вставки текста.
// </summary>
//-----------------------------------------------------------------------------

using DwtReader.Objects;
using DwtReader.Objects.Componets.Text;

namespace DwtDrawer.Components
{
    /// <summary>
    /// Определитель координат вставки.
    /// </summary>
    internal class TextAlignment
    {
        /// <summary>
        /// Получить координаты вставки.
        /// </summary>
        /// <param name="text">Объект текста</param>
        /// <returns>Координаты вставки (x,y)</returns>
        public (double x, double y) GetInsertionCoordinates(Text text)
        {
            if (text.VerticalAlignment == VerticalAlignment.Top)    // Выравнивание по верху.
                return GetInsertionCoordinatesWhenAligningTop(text);
            if (text.VerticalAlignment == VerticalAlignment.Middle) // Выравнивание по середине.
                return GetInsertionCoordinatesWhenAligningMiddle(text);
            if (text.VerticalAlignment == VerticalAlignment.Bottom) // Выравнивание по низу.
                return GetInsertionCoordinatesWhenAligningBottom(text);

            // Выравнивание по базовой линии.
            return GetInsertionCoordinatesWhenAligningBaseLine(text); 
        }

        /// <summary>
        /// Получить координаты вставки при выранивании по верху.
        /// </summary>
        /// <param name="text">Объект текста</param>
        /// <returns>Координаты вставки (x,y)</returns>
        private (double x, double y) GetInsertionCoordinatesWhenAligningTop(Text text)
        {
            double y = default;
            double x = default;

            if (text.HorizontalAlignment == HorizontalAlignment.Left) {   // Вверх лево. 
                x = GetInsertionCoordinatesXLeft(text);
                y = GetInsertionCoordinatesYTop(text);
                return (x, y);
            }
            if (text.HorizontalAlignment == HorizontalAlignment.Center) { // Вверх по центру.
                x = GetInsertionCoordinatesXCenter(text);
                y = GetInsertionCoordinatesYTop(text);
                return (x, y);
            }

            // Вверх вправо.
            x = GetInsertionCoordinatesXRight(text);
            y = GetInsertionCoordinatesYTop(text);
            return (x, y);
        }

        /// <summary>
        /// Получить координаты вставки при выранивании по середине.
        /// </summary>
        /// <param name="text">Объект текста</param>
        /// <returns>Координаты вставки (x,y)</returns>
        private (double x, double y) GetInsertionCoordinatesWhenAligningMiddle(Text text)
        {
            double y = default;
            double x = default;

            if (text.HorizontalAlignment == HorizontalAlignment.Left) {   // Середина лево.
                x = GetInsertionCoordinatesXLeft(text);
                y = GetInsertionCoordinatesYMiddle(text);
                return (x, y);
            }
            if (text.HorizontalAlignment == HorizontalAlignment.Center) { // Середина по центру.
                x = GetInsertionCoordinatesXCenter(text);
                y = GetInsertionCoordinatesYMiddle(text);
                return (x, y);
            }

            // Середина вправо.
            x = GetInsertionCoordinatesXRight(text);
            y = GetInsertionCoordinatesYMiddle(text);
            return (x, y);
        }

        /// <summary>
        /// Получить координаты вставки при выранивании по низу.
        /// </summary>
        /// <param name="text">Объект текста</param>
        /// <returns>Координаты вставки (x,y)</returns>
        private (double x, double y) GetInsertionCoordinatesWhenAligningBottom(Text text)
        {
            double y = default;
            double x = default;

            if (text.HorizontalAlignment == HorizontalAlignment.Left) {   // Низ лево.
                x = GetInsertionCoordinatesXLeft(text);
                y = GetInsertionCoordinatesYBottom(text);
                return (x, y);
            }
            if (text.HorizontalAlignment == HorizontalAlignment.Center) { // Низ по центру.
                x = GetInsertionCoordinatesXCenter(text);
                y = GetInsertionCoordinatesYBottom(text);
                return (x, y);
            }

            // Низ вправо.
            x = GetInsertionCoordinatesXRight(text);
            y = GetInsertionCoordinatesYBottom(text);
            return (x, y);
        }

        /// <summary>
        /// Получить координаты вставки при выранивании по высоте базовой линии.
        /// </summary>
        /// <param name="text">Объект текста</param>
        /// <returns>Координаты вставки (x,y)</returns>
        private (double x, double y) GetInsertionCoordinatesWhenAligningBaseLine(Text text)
        {
            double y = default;
            double x = default;

            if (text.HorizontalAlignment == HorizontalAlignment.Left) {    // Влево
                x = GetInsertionCoordinatesXLeft(text);
                y = GetInsertionCoordinatesYBaseLine(text);
                return (x, y);
            }
            if (text.HorizontalAlignment == HorizontalAlignment.Center) {  // Центр.
                x = GetInsertionCoordinatesXCenter(text);
                y = GetInsertionCoordinatesYBaseLine(text);
                return (x, y);
            }
            if (text.HorizontalAlignment == HorizontalAlignment.Right) {   // Вправо.
                x = GetInsertionCoordinatesXRight(text);
                y = GetInsertionCoordinatesYBaseLine(text);
                return (x, y);
            }
            if (text.HorizontalAlignment == HorizontalAlignment.Aligned) { // Выравненный.
                x = GetInsertionCoordinatesXAligned(text);
                y = GetInsertionCoordinatesYBaseLine(text);
                return (x, y);
            }
            if (text.HorizontalAlignment == HorizontalAlignment.Middle) {  // Середина.
                x = GetInsertionCoordinatesXMiddle(text);
                y = GetInsertionCoordinatesYMiddle(text);
                return (x, y);
            }

            // По ширине.
            x = GetInsertionCoordinatesXFit(text);
            y = GetInsertionCoordinatesYBaseLine(text);
            return (x, y);
        }

        #region HorizontalAlignment
        /// <summary>
        /// Выравнивание текста по левому краю.
        /// </summary>
        /// <param name="text">Объект текста</param>
        /// <returns>Координаты вставки (x,y)</returns>
        private double GetInsertionCoordinatesXLeft(Text text)
            =>  text.Alignment.x;

        /// <summary>
        /// Выравнивание текста по центру.
        /// </summary>
        /// <param name="text">Объект текста</param>
        /// <returns>Координаты вставки (x,y)</returns>
        private double GetInsertionCoordinatesXCenter(Text text)
            => text.Alignment.x;// - text.Value.Length * text.Height / 2;

        /// <summary>
        /// Выравнивание текста по правому краю.
        /// </summary>
        /// <param name="text">Объект текста</param>
        /// <returns>Координаты вставки (x,y)</returns>
        private double GetInsertionCoordinatesXRight(Text text)
            => text.Alignment.x - text.Value.Length * text.Height;

        /// <summary>
        /// Выравнивание текста по ширине.
        /// </summary>
        /// <param name="text">Объект текста</param>
        /// <returns>Координаты вставки (x,y)</returns>
        private double GetInsertionCoordinatesXAligned(Text text)
            => text.Alignment.x;

        /// <summary>
        /// Выравнивание текста по середине.
        /// </summary>
        /// <param name="text">Объект текста</param>
        /// <returns>Координаты вставки (x,y)</returns>
        private double GetInsertionCoordinatesXMiddle(Text text)
            => GetInsertionCoordinatesXCenter(text);

        /// <summary>
        /// Выравнивание текста по заполнению.
        /// </summary>
        /// <param name="text">Объект текста</param>
        /// <returns>Координаты вставки (x,y)</returns>
        private double GetInsertionCoordinatesXFit(Text text)
            => text.Alignment.x;
        #endregion

        #region VerticalAlignment
        /// <summary>
        /// Выравнивание текста по верху.
        /// </summary>
        /// <param name="text">Объект текста</param>
        /// <returns>Координаты вставки (x,y)</returns>
        private double GetInsertionCoordinatesYTop(Text text)
            => text.Alignment.y - text.Height;

        /// <summary>
        /// Выравнивание текста по середине.
        /// </summary>
        /// <param name="text">Объект текста</param>
        /// <returns>Координаты вставки (x,y)</returns>
        private double GetInsertionCoordinatesYMiddle(Text text)
            => text.Alignment.y - text.Height / 2;

        /// <summary>
        /// Выравнивание текста по низу.
        /// </summary>
        /// <param name="text">Объект текста</param>
        /// <returns>Координаты вставки (x,y)</returns>
        private double GetInsertionCoordinatesYBottom(Text text)
            => text.Alignment.y;

        /// <summary>
        /// Выравнивание текста по высоте базовой линии.
        /// </summary>
        /// <param name="text">Объект текста</param>
        /// <returns>Координаты вставки (x,y)</returns>
        private double GetInsertionCoordinatesYBaseLine(Text text)
            => text.Alignment.y;
        #endregion
    }
}
