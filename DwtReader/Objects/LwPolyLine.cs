//-----------------------------------------------------------------------------
// <copyright file="LwPolyLine.cs" company="SW Okolo IT">
// Copyright (c) SW Okolo IT. All rights reserved.
// </copyright>
// <summary>
// Содержит класс с данными оптимизированной мультилинии.
// </summary>
//-----------------------------------------------------------------------------

namespace DwtReader.Objects
{
    /// <summary>
    /// Данне оптимизированной мультилинии.
    /// </summary>
    public class LwPolyLine : Entity
    {
        /// <summary>
        /// Ширина.
        /// </summary>
        public double Width { get; set; } = default;

        /// <summary>
        /// Координата высоты (z).
        /// </summary>
        public double Elevation { get; set; } = default;

        /// <summary> 
        /// Толщина. 
        /// </summary>
        public double Thickness { get; set; } = default;

        /// <summary>
        /// Экструзия.
        /// </summary>
        public (double x, double y, double z) Extrusion { get; set; } = default;

        /// <summary> 
        /// Количество вершин. 
        /// </summary>
        public uint NumberVertex { get; set; } = default;

        /// <summary> 
        /// Количество выпуклостей. 
        /// </summary>
        public uint NumberBulges { get; set; } = default;

        /// <summary>
        /// Количество идентификаторов вершин. 
        /// </summary>
        public uint VertexIdCount { get; set; } = default;

        /// <summary> 
        /// Количество записей ширины.
        /// </summary>
        public uint NumberWidths { get; set; } = default;

        /// <summary>
        /// Список точек.
        /// </summary>
        public List<(double x, double y)> Points { get; set; } = new();

        /// <summary>
        /// Список выпуклостей.
        /// </summary>
        public List<double> Bulges { get; set; } = new();

        /// <summary>
        /// Список идентификаторов вершин.
        /// </summary>
        public List<uint> VertexId { get; set; } = new();

        /// <summary>
        /// Список высот.
        /// </summary>
        public List<(double start, double end)> Widths { get; set; } = new();
    }
}
