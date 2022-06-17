//-----------------------------------------------------------------------------
// <copyright file="Entity.cs" company="SW Okolo IT">
// Copyright (c) SW Okolo IT. All rights reserved.
// </copyright>
// <summary>
// Содержит класс с данными сущности.
// </summary>
//-----------------------------------------------------------------------------

using DwtReader.Objects.Componets;

namespace DwtReader.Objects
{
    /// <summary>
    /// Данные сущности.
    /// </summary>
    public class Entity
    {
        /// <summary>
        /// Размер.
        /// </summary>
        public long Size { get; set; } = default;

        /// <summary>
        /// Тип.
        /// </summary>
        public ObjectType Type { get; set; } = ObjectType.Unused;

        /// <summary>
        /// Дескриптор.
        /// </summary>
        public ObjectHandle Handle { get; set; } = new((default(byte), default(byte), default(uint)).ToTuple());

        /// <summary>
        /// Имеет ссылки?
        /// </summary>
        public bool HaveLink { get; set; } = false;

        /// <summary>
        /// Цвет (0 - 256).
        /// </summary>
        public uint Color { get; set; } = default;

        /// <summary>
        /// Линейный масштаб.
        /// </summary>
        public double LineTypeScale { get; set; } = default;

        /// <summary>
        /// Тип линии.
        /// </summary>
        public LineAndPlotType LineType { get; set; } = LineAndPlotType.ByBlock;

        /// <summary>
        /// Тип печати.
        /// </summary>
        public LineAndPlotType PlotStyle { get; set; } = LineAndPlotType.ByBlock;

        /// <summary>
        /// Тип материала.
        /// </summary>
        public MaterialType Material { get; set; } = MaterialType.ByBlock;

        /// <summary>
        /// Тень.
        /// </summary>
        public byte Shadow { get; set; } = default;

        /// <summary>
        /// Видимость (1 - невидимый).
        /// </summary>
        public ushort Invisible { get; set; } = default;

        /// <summary>
        /// Толщина линии.
        /// </summary>
        public byte LineWeight { get; set; } = default;

        /// <summary>
        /// Дескриптор родителя.
        /// </summary>
        public ObjectHandle Owner { get; set; } = new((default(byte), default(byte), default(uint)).ToTuple());

        /// <summary>
        /// Количество реакторов.
        /// </summary>
        public ushort NumberReactor { get; set; } = default;

        /// <summary>
        /// XDictFlag?
        /// </summary>
        public bool XDictFlag { get; set; } = default;

        /// <summary>
        /// Имеет полный визуальный стиль.
        /// </summary>
        public bool HasFullVisualStyle { get; set; } = default;

        /// <summary>
        /// Имеет лицевой визуальный стиль.
        /// </summary>
        public bool HasFaceVisualStyle { get; set; } = default;

        /// <summary>
        /// Имеет крайний визуальный стиль.
        /// </summary>
        public bool HasEdgeVisualStyle { get; set; } = default;
    }
}
