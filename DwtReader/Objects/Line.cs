//-----------------------------------------------------------------------------
// <copyright file="Line.cs" company="SW Okolo IT">
// Copyright (c) SW Okolo IT. All rights reserved.
// </copyright>
// <summary>
// Содержит класс с данными линии.
// </summary>
//-----------------------------------------------------------------------------

namespace DwtReader.Objects
{
    /// <summary>
    /// Данные линии.
    /// </summary>
    public class Line : Entity
    {
        /// <summary>
        /// Координата Z не существует?
        /// </summary>
        public bool ZIsNull { get; set; } = default;

        // Координаты начала.
        public double StartX { get; set; } = default;
        public double StartY { get; set; } = default;
        public double StartZ { get; set; } = default;

        // Координаты конца.
        public double EndX { get; set; } = default;
        public double EndY { get; set; } = default;
        public double EndZ { get; set; } = default;

        /// <summary> 
        /// Толщина. 
        /// </summary>
        public double Thickness { get; set; } = default;

        /// <summary>
        /// Экструзия.
        /// </summary>
        public (double x, double y, double z) Extrusion { get; set; } = default;
    }
}
