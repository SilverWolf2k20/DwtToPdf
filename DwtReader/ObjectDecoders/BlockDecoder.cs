//-----------------------------------------------------------------------------
// <copyright file="BlockDecoder.cs" company="SW Okolo IT">
// Copyright (c) SW Okolo IT. All rights reserved.
// </copyright>
// <summary>
// Содержит класс декодировщика блока.
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
    /// Декодировщик блока.
    /// </summary>
    internal class BlockDecoder : IEntityDecoder
    {
        public Entity Decode(DwtStream stream, int bitSize, Entity entity)
        {
            Logger.Info($"--------------- Block --------------");

            var stringBuffer = (DwtStream)stream.Clone();
            EntityDecoder entityDecoder = new EntityDecoder();
            var block = (Block)entityDecoder.Decode(stream, bitSize, new Block());

            block.Type = ObjectType.Block;

            // Common:
            entityDecoder.NormalizationStringStream(stringBuffer, block.Size);
            block.Name = stringBuffer.GetVariableText(DwtVersion.AC1027, false).TrimEnd('\0');

            Logger.Info($"Name:           {block.Name}");

            return block;
        }
    }
}
