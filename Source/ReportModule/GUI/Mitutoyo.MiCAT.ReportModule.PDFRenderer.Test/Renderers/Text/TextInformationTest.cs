// <copyright file="TextInformationTest.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System.Diagnostics.CodeAnalysis;
using System.Windows;
using Mitutoyo.MiCAT.ReportModule.PDFRenderer.Renderers.Text;
using Moq;
using NUnit.Framework;

namespace Mitutoyo.MiCAT.ReportModule.PDFRenderer.Test.Renderers.Text
{
   [ExcludeFromCodeCoverage]
   [TestFixture]
   public class TextInformationTest
   {
      private Mock<ITextSetting> _defaultTextSetting1;
      private Mock<ITextSetting> _defaultTextSetting2;

      [SetUp]
      public void SetUp()
      {
         _defaultTextSetting1 = new Mock<ITextSetting>();
         _defaultTextSetting2 = new Mock<ITextSetting>();
      }

      [Test]
      public void ConstructorAndPropertiesTest()
      {
         //Arrange
         TextInformation sut;

         //Act
         sut = new TextInformation("test1", _defaultTextSetting1.Object, 10f, 20f, 30f, 40f, 0, 0, TextWrapping.NoWrap);

         //Assert
         Assert.AreEqual("test1", sut.Text);
         Assert.AreEqual(_defaultTextSetting1.Object, sut.Settings);
         Assert.AreEqual(10, sut.Width);
         Assert.AreEqual(20, sut.Height);
         Assert.AreEqual(30, sut.XPosition);
         Assert.AreEqual(40, sut.YPosition);
      }

      [Test]
      public void DefaultPositionIs00()
      {
         //Arrange
         TextInformation sut;

         //Act
         sut = new TextInformation("test1", _defaultTextSetting1.Object, 10, 10, TextWrapping.NoWrap);

         //Assert
         Assert.AreEqual(0, sut.XPosition);
         Assert.AreEqual(0, sut.YPosition);
      }

      [Test]
      [TestCase(0f, 1f, false)]
      [TestCase(1f, 0f, false)]
      [TestCase(1f, 1f, false)]
      [TestCase(0f, 0f, true)]
      public void IsZeroPositionTest(float x, float y, bool expected)
      {
         //Arrange
         var textInformation1 = new TextInformation("test1", _defaultTextSetting1.Object, 100, 50, x, y, 0, 0, TextWrapping.NoWrap);

         //Act
         var actual = textInformation1.IsZeroPosition;

         //Assert
         Assert.AreEqual(expected, actual);
      }

      [Test]
      [TestCase(true, true)]
      [TestCase(false, false)]
      public void AppendDueSettingsTest(bool sameSettings, bool expected)
      {
         //Arrange
         var textInformation1 = new TextInformation("test1", _defaultTextSetting1.Object, 100, 50, 100, 10, 0, 0, TextWrapping.NoWrap);
         var textInformation2 = new TextInformation("test2", _defaultTextSetting2.Object, 100, 50, 200, 10, 0, 0, TextWrapping.NoWrap);
         _defaultTextSetting1.Setup(s => s.SameAs(_defaultTextSetting2.Object)).Returns(sameSettings);

         //Act
         var appended = textInformation1.TryAndAppend(textInformation2);

         //Assert
         Assert.AreEqual(expected, appended);
         _defaultTextSetting1.Verify(s => s.SameAs(_defaultTextSetting2.Object), Times.Once);
      }

      [Test]
      [TestCase(1f, 20.001f, 21f, false, 1f)]
      [TestCase(1f, 20.0001f, 21f, true, 401.0001f)]
      [TestCase(0.998f, 20f, 21f, false, .998f)]
      [TestCase(0.999786f, 20f, 21f, true, 400.999786f)]
      [TestCase(1f, 20f, 21.001f, false, 1f)]
      [TestCase(1f, 20f, 21.0004f, true, 401f)]
      public void AppendBecausePositionIsVeryCloseTest(float width1, float xPosition1, float xPosition2, bool appendExpected, float widthExpected)
      {
         AppendTest(width1, 100, xPosition1, 50, 400, 100, xPosition2, 50, appendExpected, widthExpected);
      }

      [Test]
      public void DontAppendBecauseDifferentY()
      {
         AppendTest(10, 10, 100, 50, 10, 10, 110, 60, false, 10);
      }
      [Test]
      public void DontAppendBecauseDifferentHeight()
      {
         AppendTest(10, 10, 100, 50, 10, 20, 110, 50, false, 10);
      }

      private void AppendTest(float width1, float height1, float xPosition1, float yPosition1, float width2, float height2, float xPosition2, float yPosition2, bool appendExpected, float widthExpected)
      {
         //Arrange
         var textExpected = appendExpected ? "test1test2" : "test1";

         var textInformation1 = new TextInformation("test1", _defaultTextSetting1.Object, width1, height1, xPosition1, yPosition1, 0, 0, TextWrapping.NoWrap);
         var textInformation2 = new TextInformation("test2", _defaultTextSetting2.Object, width2, height2, xPosition2, yPosition2, 0, 0, TextWrapping.NoWrap);
         _defaultTextSetting1.Setup(s => s.SameAs(_defaultTextSetting2.Object)).Returns(true);

         //Act
         var actual = textInformation1.TryAndAppend(textInformation2);

         //Assert
         Assert.AreEqual(appendExpected, actual);
         Assert.AreEqual(widthExpected, textInformation1.Width);
         Assert.AreEqual(textExpected, textInformation1.Text);
      }
   }
}
