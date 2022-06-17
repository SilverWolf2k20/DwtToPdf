//-----------------------------------------------------------------------------
// <copyright file="Circle.cs" company="SW Okolo IT">
// Copyright (c) SW Okolo IT. All rights reserved.
// </copyright>
// <summary>
// Содержит класс с данными окружности.
// </summary>
//-----------------------------------------------------------------------------

namespace DwtReader.Objects
{
    /// <summary>
    /// Данные окружности.
    /// </summary>
    public class Circle : Entity
    {
        public double CenterX { get; set; } = default;
        public double CenterY { get; set; } = default;
        public double CenterZ { get; set; } = default;
        public double Radius { get; set; } = default;

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
