//-----------------------------------------------------------------------------
// <copyright file="EntityDecoderFactory.cs" company="SW Okolo IT">
// Copyright (c) SW Okolo IT. All rights reserved.
// </copyright>
// <summary>
// Содержит класс фабрики декодировщиков сущностей.
// </summary>
//-----------------------------------------------------------------------------

using DwtReader.ObjectDecoders.ControlsDecoders;
using DwtReader.Objects.Componets;

namespace DwtReader.ObjectDecoders.Core
{
    /// <summary>
    /// Фабрика декодировщиков сущностей.
    /// </summary>
    internal class EntityDecoderFactory
    {
        private Dictionary<ObjectType, IEntityDecoder> _decoders = new();

        /// <summary>
        /// Конструктор фабрики, добавляющий декодировщики сущностей.
        /// </summary>
        public EntityDecoderFactory()
        {
            Add(ObjectType.Text,            new TextDecoder());
            Add(ObjectType.AttributeDefine, new AttributeDefineDecoder());
            Add(ObjectType.Block,           new BlockDecoder());
            Add(ObjectType.Line,            new LineDecoder());
            Add(ObjectType.LwPolyLine,      new LwPolyLineDecoder());
            Add(ObjectType.BlockControl,    new BlockControlDecoder());
            Add(ObjectType.BlockHeader,     new BlockHeaderDecoder());
            Add(ObjectType.Attribute,       new AttributeDecoder());
            Add(ObjectType.LayerControl,    new LayerControlDecoder());
            Add(ObjectType.Layer,           new LayerDecoder());
            Add(ObjectType.XRecord,         new XRecordDecoder());
            Add(ObjectType.Circle,          new CircleDecoder());
        }

        /// <summary>
        /// Создает декодировщик исходя из типа сущности.
        /// </summary>
        /// <param name="type">Тип сущности</param>
        /// <returns>Декодировщик</returns>
        public IEntityDecoder? Create(ObjectType type)
        {
            if (!_decoders.ContainsKey(type))
                return null;
            return _decoders[type];
        }
        /// <s
        /// ummary>
        /// Добавляет декодировщик в фабрику.
        /// </summary>
        /// <param name="type">Тип сущности</param>
        /// <param name="decoder">Декодировщик</param>
        public void Add(ObjectType type, IEntityDecoder decoder)
            => _decoders.Add(type, decoder);
    }
}
