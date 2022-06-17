//-----------------------------------------------------------------------------
// <copyright file="Text.cs" company="SW Okolo IT">
// Copyright (c) SW Okolo IT. All rights reserved.
// </copyright>
// <summary>
// Содержит класс с данными текста.
// </summary>
//-----------------------------------------------------------------------------

using DwtReader.Objects.Componets.Text;

namespace DwtReader.Objects
{
    /// <summary>
    /// Данные текста.
    /// </summary>
    public class Text : Entity
    {
        /// <summary>
        /// Координата высоты (z).
        /// </summary>
        public double Elevation { get; set; } = default;

        /// <summary>
        /// Координаты вставки.
        /// </summary>
        public (double x, double y) Insertion { get; set; } = default;

        /// <summary>
        /// Координаты выравнивания.
        /// </summary>
        public (double x, double y) Alignment { get; set; } = default;

        /// <summary>
        /// Толщина.
        /// </summary>
        public double Thickness { get; set; } = default;

        /// <summary>
        /// Эктрузия.
        /// </summary>
        public (double x, double y, double z) Extrusion { get; set; } = default;

        /// <summary>
        /// Угол наклона.
        /// </summary>
        public double ObliqueAngle { get; set; } = default;

        /// <summary>
        /// Угол поворота.
        /// </summary>
        public double RotationAngle { get; set; } = default;

        /// <summary>
        /// Высота.
        /// </summary>
        public double Height { get; set; } = default;

        /// <summary>
        /// Коэффициент ширины.
        /// </summary>
        public double WidthFactor { get; set; } = default;

        /// <summary>
        /// Данные текста.
        /// </summary>
        public string Value { get; set; } = String.Empty;

        /// <summary>
        /// Отражение.
        /// </summary>
        public MirrorFlag Generation { get; set; } = MirrorFlag.None;

        /// <summary>
        /// Горизонтальное выравнивание.
        /// </summary>
        public HorizontalAlignment HorizontalAlignment { get; set; } = HorizontalAlignment.Left;

        /// <summary>
        /// Вертикальное выравнивание.
        /// </summary>
        public VerticalAlignment VerticalAlignment { get; set; } = VerticalAlignment.Baseline;
    }
}
