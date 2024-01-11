// <copyright file="VMResizableElementTests.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System;
using System.Diagnostics.CodeAnalysis;
using Mitutoyo.MiCAT.ApplicationState;
using Mitutoyo.MiCAT.ReportModule.GUI.VisualElements.ReportElements;
using Moq;
using NUnit.Framework;

namespace Mitutoyo.MiCAT.ReportModule.GUI.Test.VisualElements.ReportElements
{
   [ExcludeFromCodeCoverage]
   public class VMResizableElementTests
   {
      private IServiceFake Service { get; set; }

      [SetUp]
      public void SetUp()
      {
         Service = Mock.Of<IServiceFake>();
      }

      [Test]
      public void Constructor_ShouldInitializeValues()
      {
         var id = Mock.Of<IItemId>();
         var element = new Element(Service);

         Assert.IsNotNull(element);
         Assert.IsNotNull(element.StartResizeCommand);
         Assert.IsNotNull(element.ResizeCommand);
         Assert.IsNotNull(element.CompleteResizeCommand);
         Assert.AreEqual(ResizeType.All, element.ResizeType);
         Assert.AreEqual(true, element.IsResizable);
         Assert.AreEqual(true, element.IsHorizontalResizable);
         Assert.AreEqual(true, element.IsVerticalResizable);
         Assert.AreEqual(false, element.IsResizing);
         Assert.AreEqual(0, element.MinWidth);
         Assert.AreEqual(0, element.MinHeight);
         Assert.AreEqual(int.MaxValue, element.MaxWidth);
         Assert.AreEqual(int.MaxValue, element.MaxHeight);
      }

      [Test]
      public void StartResize_OnResizableElement_ShouldStartResizing()
      {
         var element = new Element(Service);
         element.StartResize();

         Assert.IsTrue(element.IsResizing);
         Assert.IsFalse(element.IsDragging);
      }

      [Test]
      public void StartResize_OnNotResizableElement_ShouldDoNothing()
      {
         var element = new Element(Service) { ResizeType = ResizeType.None, };
         element.StartResize();

         Assert.IsFalse(element.IsResizing);
      }

      [Test]
      [TestCase(0, 10)]
      [TestCase(10, 0)]
      [TestCase(10, 10)]
      public void Resize_ShouldUpdateVisuals(int horizontalDelta, int verticalDelta)
      {
         var element = new Element(Service);

         element.StartResize();
         element.Resize(horizontalDelta, false, verticalDelta, false);

         Assert.IsTrue(element.IsResizing);
         Assert.AreEqual(horizontalDelta, element.VisualWidth);
         Assert.AreEqual(verticalDelta, element.VisualHeight);
      }

      [Test]
      [TestCase(0, 10)]
      [TestCase(10, 0)]
      [TestCase(10, 10)]
      public void Resize_WhenElementIsHorizontalResizable_ShouldUpdateVisualXAndVisualWidth(
         int horizontalDelta,
         int verticalDelta)
      {
         var element = new Element(Service) { ResizeType = ResizeType.Horizontal, };

         element.StartResize();
         element.Resize(horizontalDelta, false, verticalDelta, false);

         Assert.IsTrue(element.IsResizing);
         Assert.IsTrue(element.IsHorizontalResizable);
         Assert.IsFalse(element.IsVerticalResizable);
         Assert.AreEqual(horizontalDelta, element.VisualWidth);
         Assert.AreEqual(0, element.VisualHeight);
      }

      [Test]
      [TestCase(0, 10)]
      [TestCase(10, 0)]
      [TestCase(10, 10)]
      public void Resize_WhenElementIsVerticalResizable_ShouldUpdateVisualYAndVisualHeight(
         int horizontalDelta,
         int verticalDelta)
      {
         var element = new Element(Service) { ResizeType = ResizeType.Vertical, };

         element.StartResize();
         element.Resize(horizontalDelta, false, verticalDelta, false);

         Assert.IsTrue(element.IsResizing);
         Assert.IsFalse(element.IsHorizontalResizable);
         Assert.IsTrue(element.IsVerticalResizable);
         Assert.AreEqual(0, element.VisualWidth);
         Assert.AreEqual(verticalDelta, element.VisualHeight);
      }

      [Test]
      [TestCase(0)]
      [TestCase(10)]
      [TestCase(30)]
      public void ResizeFromLeft_ShouldUpdateVisualX(int horizontalDelta)
      {
         var element = new Element(Service);

         element.StartResize();
         element.Resize(horizontalDelta, true, 0, false);

         Assert.IsTrue(element.IsResizing);
         Assert.AreEqual(horizontalDelta, element.VisualWidth);
         Assert.AreEqual(-horizontalDelta, element.VisualX);
      }

      [Test]
      [TestCase(0)]
      [TestCase(10)]
      [TestCase(30)]
      public void ResizeFromTop_ShouldUpdateVisualY(int verticalDelta)
      {
         var element = new Element(Service);

         element.StartResize();
         element.Resize(0, false, verticalDelta, true);

         Assert.IsTrue(element.IsResizing);
         Assert.AreEqual(verticalDelta, element.VisualHeight);
         Assert.AreEqual(-verticalDelta, element.VisualY);
      }

      [Test]
      [TestCase(100, 100, -1)]
      [TestCase(100, 0, -50)]
      [TestCase(100, 30, -50)]
      public void DecreaseWidth_FromRightSide_ShouldRespectMinWidthConstraint(
         int actualWidth, int minWidth, int delta)
      {
         var element = new Element(Service, actualWidth, 0)
         {
            MinWidth = minWidth,
         };

         element.StartResize();
         element.Resize(delta, false, 0, false);

         Assert.AreEqual(Math.Max(actualWidth + delta, minWidth), element.VisualWidth);
      }

      [Test]
      [TestCase(100, 100, -1)]
      [TestCase(100, 0, -50)]
      [TestCase(100, 30, -50)]
      public void DecreaseWidth_FromLeftSide_ShouldRespectMinWidthConstraint(
         int actualWidth, int minWidth, int delta)
      {
         var element = new Element(Service, actualWidth, 0)
         {
            MinWidth = minWidth,
         };

         element.StartResize();
         element.Resize(delta, true, 0, false);

         Assert.AreEqual(Math.Max(actualWidth + delta, minWidth), element.VisualWidth);
      }

      [Test]
      [TestCase(100, 100, -1)]
      [TestCase(100, 0, -50)]
      [TestCase(100, 30, -50)]
      public void DecreaseHeight_FromBottomSide_ShouldRespectMinHeightConstraint(
         int actualHeight, int minHeight, int delta)
      {
         var element = new Element(Service, 0, actualHeight)
         {
            MinHeight = minHeight,
         };

         element.StartResize();
         element.Resize(0, false, delta, false);

         Assert.AreEqual(Math.Max(actualHeight + delta, minHeight), element.VisualHeight);
      }

      [Test]
      [TestCase(100, 100, -1)]
      [TestCase(100, 0, -50)]
      [TestCase(100, 30, -50)]
      public void DecreaseHeight_FromTopSide_ShouldRespectMinHeightConstraint(
         int actualHeight, int minHeight, int delta)
      {
         var element = new Element(Service, 0, actualHeight)
         {
            MinHeight = minHeight,
         };

         element.StartResize();
         element.Resize(0, false, delta, true);

         Assert.AreEqual(Math.Max(actualHeight + delta, minHeight), element.VisualHeight);
      }

      [Test]
      [TestCase(100, 100, 1)]
      [TestCase(100, 200, 50)]
      [TestCase(100, 170, 50)]
      [TestCase(100, 130, 50)]
      public void IncreaseWidth_FromRightSide_ShouldRespectMaxWidthConstraint(
         int actualWidth, int maxWidth, int delta)
      {
         var element = new Element(Service, actualWidth, 0)
         {
            MaxWidth = maxWidth,
         };

         element.StartResize();
         element.Resize(delta, false, 0, false);

         Assert.AreEqual(Math.Min(actualWidth + delta, maxWidth), element.VisualWidth);
      }

      [Test]
      [TestCase(100, 100, 1)]
      [TestCase(100, 200, 50)]
      [TestCase(100, 170, 50)]
      [TestCase(100, 130, 50)]
      public void IncreaseWidth_FromLeftSide_ShouldRespectMaxWidthConstraint(
         int actualWidth, int maxWidth, int delta)
      {
         var element = new Element(Service, actualWidth, 0)
         {
            MaxWidth = maxWidth,
         };

         element.StartResize();
         element.Resize(delta, true, 0, false);

         Assert.AreEqual(Math.Min(actualWidth + delta, maxWidth), element.VisualWidth);
      }

      [Test]
      [TestCase(100, 100, 1)]
      [TestCase(100, 200, 50)]
      [TestCase(100, 170, 50)]
      [TestCase(100, 130, 50)]
      public void IncreaseHeight_FromBottomSide_ShouldRespectMaxHegihtConstraint(
         int actualHeight, int maxHeight, int delta)
      {
         Element element = new Element(Service, 0, actualHeight)
         {
            MaxHeight = maxHeight,
         };

         element.StartResize();
         element.Resize(0, false, delta, false);

         Assert.AreEqual(Math.Min(actualHeight + delta, maxHeight), element.VisualHeight);
      }

      [Test]
      [TestCase(100, 100, 1)]
      [TestCase(100, 200, 50)]
      [TestCase(100, 170, 50)]
      [TestCase(100, 130, 50)]
      public void IncreaseHeight_FromTopSide_ShouldRespectMaxHegihtConstraint(
         int actualHeight, int maxHeight, int delta)
      {
         Element element = new Element(Service, 0, actualHeight)
         {
            MaxHeight = maxHeight,
         };

         element.StartResize();
         element.Resize(0, false, delta, true);

         Assert.AreEqual(Math.Min(actualHeight + delta, maxHeight), element.VisualHeight);
      }

      public interface IServiceFake
      {
         void SetSize();
      }

      public class Element : VMResizablePlacement
      {
         private readonly IServiceFake _service;

         public Element(IServiceFake service, int width, int height)
            : base()
         {
            _service = service;
            VisualWidth = width;
            VisualHeight = height;
         }

         public Element(IServiceFake service) : this(service, 0, 0)
         {
         }

         protected override void SetSelected()
         {
         }
         protected override void OnCompleteDrag()
         {
            throw new NotImplementedException();
         }
         public override void CompleteResize()
         {
            _service.SetSize();
         }
      }
   }
}
