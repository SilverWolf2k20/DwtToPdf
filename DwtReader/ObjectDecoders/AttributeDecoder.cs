//-----------------------------------------------------------------------------
// <copyright file="AttributeDecoder.cs" company="SW Okolo IT">
// Copyright (c) SW Okolo IT. All rights reserved.
// </copyright>
// <summary>
// Содержит класс декодировщика атрибута.
// </summary>
//-----------------------------------------------------------------------------

using DwtReader.Core;
using DwtReader.ObjectDecoders.Core;
using DwtReader.Objects;
using DwtReader.Objects.Componets;

using Usl;

namespace DwtReader.ObjectDecoders
{
    /// <summary>
    /// Декодировщик атрибута.
    /// </summary>
    internal class AttributeDecoder : IEntityDecoder
    {
        public Entity Decode(DwtStream stream, int bitSize, Entity entity)
        {
            Logger.Info($"------------- Attribute ------------");

            var stringBuffer = (DwtStream)stream.Clone();

            EntityDecoder entityDecoder = new EntityDecoder();
            var attribute = (Objects.Attribute)entityDecoder.Decode(stream, bitSize, new Objects.Attribute());

            TextDecoder textDecoder = new TextDecoder();
            entityDecoder.NormalizationStringStream(stringBuffer, attribute.Size);
            attribute = (Objects.Attribute)textDecoder.CommonTextEntityData(stream, stringBuffer, attribute);

            // Common:
            attribute = (Objects.Attribute)entityDecoder.DecodeOwner(stream, attribute);

            attribute.Type = ObjectType.Attribute;
            // R2010+
            Logger.Debug($"Version:        {stream.GetRawChar()}");
            Logger.Debug($"СRC:            {stream.GetRawShort()}");

            return attribute;
        }
    }
}
