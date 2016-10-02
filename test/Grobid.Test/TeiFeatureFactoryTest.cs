﻿using System.Xml.Linq;

using ApprovalTests;
using ApprovalTests.Reporters;

using Grobid.NET;

using Xunit;

namespace Grobid.Test
{
    [UseReporter(typeof(DiffReporter))]
    public class TeiFeatureFactoryTest
    {
        [Fact]
        public void TeiFeatureFactoryAddress00()
        {
            var tei = this.InsertXmlSnippetIntoTei("<address>Urbana, IL 61801 Urbana, IL 61801 <lb/></address>");

            var testSubject = new TeiFeatureFactory();
            Approvals.Verify(testSubject.Create(XDocument.Parse(tei)));
        }

        [Fact]
        public void TeiFeatureFactoryAffiliation00()
        {
            var tei = this.InsertXmlSnippetIntoTei("<byline><affiliation>Department of Computer Science Department of Computer Science <lb/>University of Illinois University of Illinois <lb/></affiliation></byline>");

            var testSubject = new TeiFeatureFactory();
            Approvals.Verify(testSubject.Create(XDocument.Parse(tei)));
        }

        [Fact]
        public void TeiFeatureFactoryAuthor00()
        {
            var tei = this.InsertXmlSnippetIntoTei("<byline><docAuthor>Amitabh B.Sinha Laxmikant V. Kale<lb/></docAuthor></byline>");

            var testSubject = new TeiFeatureFactory();
            Approvals.Verify(testSubject.Create(XDocument.Parse(tei)));
        }

        [Fact]
        public void TeiFeatureFactoryEmail00()
        {
            var tei = this.InsertXmlSnippetIntoTei("<email>email: sinha@cs.uiuc.edu </email> <email>email: kale@cs.uiuc.edu <lb/></email>");

            var testSubject = new TeiFeatureFactory();
            Approvals.Verify(testSubject.Create(XDocument.Parse(tei)));
        }

        [Fact]
        public void TeiFeatureFactoryTitle00()
        {
            var tei = this.InsertXmlSnippetIntoTei("<docTitle><titlePart>The wizard quickly jinxed the gnomes before they vaporized</titlePart></docTitle>");

            var testSubject = new TeiFeatureFactory();
            Approvals.Verify(testSubject.Create(XDocument.Parse(tei)));
        }

        [Fact]
        public void TeiFeatureFactoryTitle01()
        {
            var tei = this.InsertXmlSnippetIntoTei("<docTitle><titlePart type='main'>The wizard quickly jinxed the gnomes before they vaporized</titlePart></docTitle>");

            var testSubject = new TeiFeatureFactory();
            Approvals.Verify(testSubject.Create(XDocument.Parse(tei)));
        }


        private string InsertXmlSnippetIntoTei(string s)
        {
            var tei = $@"
<?xml version='1.0' encoding='utf-8'?>
<tei>
  <text xml:lang='en'>
    <front>
     {s}
    </front>
  </text>
</tei>";
            return tei.Trim();
        }
    }
}
