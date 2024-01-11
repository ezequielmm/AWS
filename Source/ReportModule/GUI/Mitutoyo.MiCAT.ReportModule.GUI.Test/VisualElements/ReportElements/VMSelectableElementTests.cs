// <copyright file="VMSelectableElementTests.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System.Diagnostics.CodeAnalysis;
using Mitutoyo.MiCAT.ApplicationState;
using Mitutoyo.MiCAT.ReportModule.GUI.VisualElements.ReportElements;
using Moq;
using NUnit.Framework;

namespace Mitutoyo.MiCAT.ReportModule.GUI.Test.VisualElements.ReportElements
{
   [ExcludeFromCodeCoverage]
   public class VMSelectableElementTests
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
         Assert.IsNotNull(element.SelectCommand);
         Assert.AreEqual(true, element.IsSelectable);
         Assert.AreEqual(false, element.IsSelected);
      }

      [Test]
      public void Select_UnselectedSelectableElement_ShouldCallSetSelectedAsync()
      {
         var element = new Element(Service);

         element.Select(null);

         Mock
            .Get(Service)
            .Verify(x => x.SetSelectedAsync(), Times.Once);
      }

      [Test]
      public void Select_SelectedSelectableElement_ShouldDoNothing()
      {
         var element = new Element(Service, true);

         element.Select(null);

         Mock
            .Get(Service)
            .Verify(x => x.SetSelectedAsync(), Times.Never);
      }

      [Test]
      public void Select_UnselectedUnselectableElement_ShouldDoNothing()
      {
         var element = new Element(Service) { IsSelectable = false, };

         element.Select(null);

         Mock
            .Get(Service)
            .Verify(x => x.SetSelectedAsync(), Times.Never);
      }

      public interface IServiceFake
      {
         void SetSelectedAsync();
      }

      public class Element : VMSelectablePlacement
      {
         private readonly IServiceFake _service;

         public Element(IServiceFake service, bool isSelected) : base()
         {
            _service = service;
            IsSelected = isSelected;
         }

         public Element(IServiceFake service) : this(service, false)
         {
         }

         protected override void SetSelected()
         {
            _service.SetSelectedAsync();
         }
      }
   }
}
