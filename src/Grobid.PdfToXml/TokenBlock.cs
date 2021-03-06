﻿using System;
using System.Linq;
using System.Text;

using iTextSharp.text;

namespace Grobid.PdfToXml
{
    public class TokenBlock : IEquatable<TokenBlock>
    {
        public static readonly TokenBlock Empty = new TokenBlock { IsEmpty = true };

        public int Id { get; set; }

        public float X { get; set; }
        public float Y { get; set; }
        public float Height { get; set; }
        public float Width { get; set; }
        public int Angle { get; set; }
        public float Base { get; set; }
        public Rectangle BoundingRectangle { get; set; }
        public string FontColor { get; set; }
        public FontFlags FontFlags { get; set; }
        public FontName FontName { get; set; }
        public float FontSize { get; set; }
        public bool IsEmpty { get; set; }
        public int Rotation { get; set; }
        public string Text { get; set; }

        public bool IsBold => this.FontName.IsBold || this.FontFlags.HasFlag(FontFlags.Bold);
        public bool IsItalic => this.FontName.IsItalic || this.FontFlags.HasFlag(FontFlags.Italic);
        public bool IsSymbolic => this.FontFlags.HasFlag(FontFlags.Symbolic);

        public static TokenBlock Merge(TokenBlock[] tokenBlocks)
        {
            var mergedTokenBlock = tokenBlocks[0];
            mergedTokenBlock.Text = String.Join(String.Empty, tokenBlocks.Select(x => x.Text)).Normalize();

            mergedTokenBlock.Text = mergedTokenBlock.Text.Replace("ﬁ", "fi");
            mergedTokenBlock.Text = mergedTokenBlock.Text.Replace("ﬂ", "fl");
            mergedTokenBlock.Text = mergedTokenBlock.Text.Replace("’", "'");

            mergedTokenBlock.BoundingRectangle = new Rectangle(
                tokenBlocks.First().BoundingRectangle.Left,
                tokenBlocks.First().BoundingRectangle.Bottom,
                tokenBlocks.Last().BoundingRectangle.Right,
                tokenBlocks.Last().BoundingRectangle.Top);

            mergedTokenBlock.Width = mergedTokenBlock.BoundingRectangle.Width;

            return mergedTokenBlock;
        }

        public TokenBlock[] Tokenize()
        {
            var tokenBlocks = this
                .Text
                .SplitWithDelims(Constants.FullPunctuation)
                .Select(this.CloneWithText)
                .ToArray();

            return tokenBlocks;
        }

        private TokenBlock CloneWithText(string text)
        {
            return new TokenBlock
            {
                FontFlags = this.FontFlags,
                FontColor = this.FontColor,
                FontName = this.FontName,
                FontSize = this.FontSize,
                Angle = this.Angle,
                Rotation = this.Rotation,
                IsEmpty = this.IsEmpty,
                Base = this.Base,
                BoundingRectangle = this.BoundingRectangle,
                Height = this.Height,
                Width = this.Width,
                X = this.X,
                Y = this.Y,
                Text = text,
            };
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;
                hash = hash * 23 + this.X.GetHashCode();
                hash = hash * 23 + this.Y.GetHashCode();
                hash = hash * 23 + this.Height.GetHashCode();
                hash = hash * 23 + this.Width.GetHashCode();
                hash = hash * 23 + this.Text.GetHashCode();

                return hash;
            }
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            var other = obj as TokenBlock;
            return other != null && this.Equals(other);
        }

        public bool Equals(TokenBlock other)
        {
            // TODO: compare with a epsilon
            return
                this.X == other.X &&
                this.Y == other.Y &&
                this.Height == other.Height &&
                this.Width == other.Width &&
                this.Text == other.Text;
        }

        public static bool operator==(TokenBlock lhs, TokenBlock rhs)
        {
            if (System.Object.ReferenceEquals(lhs, rhs))
            {
                return true;
            }

            if ((object)lhs == null || (object)rhs == null)
            {
                return false;
            }

            return lhs.Equals(rhs);
        }

        public static bool operator!=(TokenBlock lhs, TokenBlock rhs)
        {
            return !(lhs == rhs);
        }
    }
}