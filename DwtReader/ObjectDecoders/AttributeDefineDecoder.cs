//-----------------------------------------------------------------------------
// <copyright file="AttributeDefineDecoder.cs" company="SW Okolo IT">
// Copyright (c) SW Okolo IT. All rights reserved.
// </copyright>
// <summary>
// Содержит класс декодировщика определенного атрибута.
// </summary>
//-----------------------------------------------------------------------------

using DwtReader.Components;
using DwtReader.Core;
using DwtReader.ObjectDecoders.Core;
using DwtReader.Objects;
using DwtReader.Objects.Componets;

using Usl;

namespace DwtReader.ObjectDecoders
{
    /// <summary>
    /// Декодировщик определенного атрибута.
    /// </summary>
    internal class AttributeDefineDecoder : IEntityDecoder
    {
        public Entity Decode(DwtStream stream, int bitSize, Entity entity)
        {
            Logger.Info($"--------- Attribute define ---------");

            var stringBuffer = (DwtStream)stream.Clone();

            EntityDecoder entityDecoder = new EntityDecoder();
            var attribute = (AttributeDefine)entityDecoder.Decode(stream, bitSize, new AttributeDefine());

            TextDecoder textDecoder = new TextDecoder();
            entityDecoder.NormalizationStringStream(stringBuffer, attribute.Size);
            attribute = (AttributeDefine)textDecoder.CommonTextEntityData(stream, stringBuffer, attribute);

            // Common:
            attribute = (AttributeDefine)entityDecoder.DecodeOwner(stream, attribute);
            attribute.Type = ObjectType.AttributeDefine;
            // R2010+:
            attribute.Version = stream.GetRawChar();
            // Common:
            attribute.Prompt = stringBuffer.GetVariableText(DwtVersion.AC1027, false).TrimEnd('\0');

            Logger.Debug($"Version:        {attribute.Version}");
            Logger.Trace($"Prompt:         {attribute.Prompt}");
            Logger.Debug($"CRC:            {stream.GetRawShort()}");

            return attribute;
        }
    }
}
