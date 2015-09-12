﻿using FluentAssertions;
using Grobid.NET;
using Xunit;

namespace Grobid.Test
{
    public class TextBlockFactoryTest
    {
        [Fact]
        public void Test()
        {
            var pageBlockFactory = new PageBackFactory();
            var pageBlocks = pageBlockFactory.Create(Sample.Pdf.OpenEssenseLinq());

            var testSubject = new TextBlockFactory();
            var textBlocks = testSubject.Create(pageBlocks[0]);
            textBlocks.Should().HaveCount(118);

            textBlocks[0].Text.Should().Be("The essence of language-integrated query\n");
            textBlocks[1].Text.Should().Be("James Cheney\n");
        }
    }
}
