// <copyright file="VMDraggableElementTests.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System.Diagnostics.CodeAnalysis;
using Mitutoyo.MiCAT.ReportModule.GUI.VisualElements.ReportElements;
using Moq;
using NUnit.Framework;

namespace Mitutoyo.MiCAT.ReportModule.GUI.Test.VisualElements.ReportElements
{
   [TestFixture]
   [ExcludeFromCodeCoverage]
   public class VMDraggableElementTests
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
         var element = new Element(Service);

         Assert.IsNotNull(element);
         Assert.IsNotNull(element.StartDragCommand);
         Assert.IsNotNull(element.DragCommand);
         Assert.IsNotNull(element.CompleteDragCommand);
         Assert.AreEqual(true, element.IsDraggable);
         Assert.AreEqual(false, element.IsDragging);
      }

      [Test]
      public void StartDrag_OnDraggableElement_ShouldStartDragging()
      {
         var element = new Element(Service);
         element.StartDrag();

         Assert.IsTrue(element.IsDragging);
      }

      [Test]
      [TestCase(0, 10)]
      [TestCase(10, 0)]
      [TestCase(10, 10)]
      public void Drag_OnDraggableElement_ShouldUpdateVisualValues(int deltaX, int deltaY)
      {
         var element = new Element(Service);
         element.StartDrag();
         element.Drag(deltaX, deltaY);

         Assert.IsTrue(element.IsDragging);
         Assert.AreEqual(deltaX, element.VisualX);
         Assert.AreEqual(deltaY, element.VisualY);
      }

      [Test]
      [TestCase(0, 10)]
      [TestCase(10, 0)]
      [TestCase(10, 10)]
      public void Drag_OnDraggableElement_WhichIsNotDragging_ShouldDoNothing(int deltaX, int deltaY)
      {
         var element = new Element(Service);

         element.Drag(deltaX, deltaY);

         Assert.IsFalse(element.IsDragging);
         Assert.AreEqual(0, element.VisualX);
         Assert.AreEqual(0, element.VisualY);
      }

      [Test]
      public void CompleteDrag_OnDraggableElement_WhichIsDragging_ShouldCallSetPosition()
      {
         var element = new Element(Service);
         element.StartDrag();
         element.CompleteDrag();

         Assert.IsFalse(element.IsDragging);
         Mock
            .Get(Service)
            .Verify(x => x.SetPosition(), Times.Once);
      }

      [Test]
      public void CompleteDrag_OnDraggableElement_WhichIsNotDragging_ShouldDoNothing()
      {
         var element = new Element(Service);
         element.CompleteDrag();

         Assert.IsFalse(element.IsDragging);
         Mock
            .Get(Service)
            .Verify(x => x.SetPosition(), Times.Never);
      }

      public interface IServiceFake
      {
         void SetPosition();
      }

      public class Element : VMDraggablePlacement
      {
         private readonly IServiceFake _service;

         public Element(IServiceFake service) : base()
         {
            _service = service;
         }

         protected override void OnCompleteDrag()
         {
            _service.SetPosition();
         }

         protected override void SetSelected()
         {
            return;
         }
      }
   }
}
