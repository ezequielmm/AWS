// <copyright file="HeaderFormFieldControllerTest.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System;
using System.Diagnostics.CodeAnalysis;
using Mitutoyo.MiCAT.ApplicationState;
using Mitutoyo.MiCAT.ApplicationState.Controllers;
using Mitutoyo.MiCAT.ReportModule.Domain.Components;
using Mitutoyo.MiCAT.ReportModuleApp.Controllers;
using NUnit.Framework;

namespace Mitutoyo.MiCAT.ReportModule.ControllerIntegration.Test.ControllerTests
{
   [TestFixture]
   [ExcludeFromCodeCoverage]
   public class HeaderFormFieldControllerTest : BaseAppStateTest
   {
      private HeaderFormFieldController _controller;

      private Id<ReportHeaderFormField> _reportHeaderFormFieldId;
      private ReportHeaderFormField _reportHeaderFormField;

      public ISnapShot BuildHelper(ISnapShot snapShot)
      {
         snapShot = snapShot.AddCollection<ReportHeaderFormField>(AppStateKinds.Undoable);

         return snapShot;
      }

      [SetUp]
      public virtual void Setup()
      {
         SetUpHelper(BuildHelper);

         _controller = new HeaderFormFieldController(_history);
      }

      [Test]
      public void HeaderFormFieldController_UpdateSelectedHeaderFormFieldWithNewIdShouldUpdateAppState()
      {
         _reportHeaderFormFieldId = new Id<ReportHeaderFormField>(Guid.NewGuid());
         _reportHeaderFormField = new ReportHeaderFormField(_reportHeaderFormFieldId, Guid.Empty, null, 0);

         var snapShot = _history.NextSnapShot(ControllerCall.Empty);
         snapShot = snapShot.AddItem(_reportHeaderFormField);
         _history.AddSnapShot(snapShot);

         var newSelectedFieldId = Guid.NewGuid();
         _controller.UpdateSelectedHeaderFormField(_reportHeaderFormFieldId, newSelectedFieldId);

         var result = _history.CurrentSnapShot.GetItem(_reportHeaderFormFieldId);

         Assert.IsNotNull(result);
         Assert.AreEqual(newSelectedFieldId, result.SelectedFieldId);
         Assert.IsNull(result.CustomLabel);
      }

      [Test]
      public void HeaderFormFieldController_UpdateSelectedHeaderFormFieldWithNewIdShouldResetCustomLabel()
      {
         _reportHeaderFormFieldId = new Id<ReportHeaderFormField>(Guid.NewGuid());
         _reportHeaderFormField = new ReportHeaderFormField(_reportHeaderFormFieldId, Guid.NewGuid(), "Custom Label", 0);

         var snapShot = _history.NextSnapShot(ControllerCall.Empty);
         snapShot = snapShot.AddItem(_reportHeaderFormField);
         _history.AddSnapShot(snapShot);

         var newSelectedFieldId = Guid.NewGuid();
         _controller.UpdateSelectedHeaderFormField(_reportHeaderFormFieldId, newSelectedFieldId);

         var result = _history.CurrentSnapShot.GetItem(_reportHeaderFormFieldId);

         Assert.IsNotNull(result);
         Assert.AreEqual(newSelectedFieldId, result.SelectedFieldId);
         Assert.IsNull(result.CustomLabel);
      }

      [Test]
      public void HeaderFormFieldController_UpdateSelectedHeaderFormFieldWithSameIdShouldNotUpdateAppState()
      {
         var selectedFieldId = Guid.NewGuid();
         _reportHeaderFormFieldId = new Id<ReportHeaderFormField>(Guid.NewGuid());
         _reportHeaderFormField = new ReportHeaderFormField(_reportHeaderFormFieldId, selectedFieldId, null, 0);

         var snapShot = _history.NextSnapShot(ControllerCall.Empty);
         snapShot = snapShot.AddItem(_reportHeaderFormField);
         _history.AddSnapShot(snapShot);

         _controller.UpdateSelectedHeaderFormField(_reportHeaderFormFieldId, selectedFieldId);

         var result = _history.CurrentSnapShot.GetItem(_reportHeaderFormFieldId);

         Assert.IsNotNull(result);
         Assert.AreEqual(selectedFieldId, result.SelectedFieldId);
         Assert.IsNull(result.CustomLabel);
      }

      [Test]
      public void HeaderFormFieldController_UpdateSelectedHeaderFormFieldLabelShouldUpdateAppState()
      {
         var selecedFieldId = Guid.NewGuid();
         _reportHeaderFormFieldId = new Id<ReportHeaderFormField>(Guid.NewGuid());
         _reportHeaderFormField = new ReportHeaderFormField(_reportHeaderFormFieldId, selecedFieldId, "Custom Label", 0);

         var snapShot = _history.NextSnapShot(ControllerCall.Empty);
         snapShot = snapShot.AddItem(_reportHeaderFormField);
         _history.AddSnapShot(snapShot);

         var newCustomLabel = "New Value";
         _controller.UpdateSelectedHeaderFormFieldLabel(_reportHeaderFormFieldId, newCustomLabel);

         var result = _history.CurrentSnapShot.GetItem(_reportHeaderFormFieldId);

         Assert.IsNotNull(result);
         Assert.AreEqual(selecedFieldId, result.SelectedFieldId);
         Assert.AreEqual(newCustomLabel, result.CustomLabel);
      }

      [Test]
      public void HeaderFormFieldController_UpdateSelectedHeaderFormFieldLabelWithSameCustomLabelShouldNotUpdateAppState()
      {
         var selectedFieldId = Guid.NewGuid();
         var customLabel = "Custom label";
         _reportHeaderFormFieldId = new Id<ReportHeaderFormField>(Guid.NewGuid());
         _reportHeaderFormField = new ReportHeaderFormField(_reportHeaderFormFieldId, selectedFieldId, customLabel, 0);

         var snapShot = _history.NextSnapShot(ControllerCall.Empty);
         snapShot = snapShot.AddItem(_reportHeaderFormField);
         _history.AddSnapShot(snapShot);

         _controller.UpdateSelectedHeaderFormFieldLabel(_reportHeaderFormFieldId, customLabel);

         var result = _history.CurrentSnapShot.GetItem(_reportHeaderFormFieldId);

         Assert.IsNotNull(result);
         Assert.AreEqual(selectedFieldId, result.SelectedFieldId);
         Assert.AreEqual(customLabel, result.CustomLabel);
      }
   }
}
