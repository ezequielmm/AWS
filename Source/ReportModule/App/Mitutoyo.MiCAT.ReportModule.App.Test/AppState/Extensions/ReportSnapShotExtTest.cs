// <copyright file="ReportSnapShotExtTest.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Mitutoyo.MiCAT.ApplicationState;
using Mitutoyo.MiCAT.ApplicationState.Controllers;
using Mitutoyo.MiCAT.ReportModule.App.Test.AppStateHelper;
using Mitutoyo.MiCAT.ReportModule.Domain.Components;
using Mitutoyo.MiCAT.ReportModule.Domain.Components.ReportSections;
using Mitutoyo.MiCAT.ReportModule.Domain.Data;
using Mitutoyo.MiCAT.ReportModuleApp.AppState.Extensions;
using NUnit.Framework;

namespace Mitutoyo.MiCAT.ReportModule.App.Test.AppState.Extensions
{
   [ExcludeFromCodeCoverage]
   public class ReportSnapShotTestEntity : BaseStateEntity<ReportSnapShotTestEntity>
   {
      public string Value { get; set; }

      public ReportSnapShotTestEntity(Id<ReportSnapShotTestEntity> id) : base(id)
      {
      }
   }

   [ExcludeFromCodeCoverage]
   [TestFixture]
   public class ReportSnapShotExtTest : BaseAppStateTest
   {
      public static ISnapShot BuildHelper(ISnapShot snapShot)
      {
         snapShot = snapShot.AddCollection<ReportTextBox>(AppStateKinds.Undoable);
         snapShot = snapShot.AddCollection<ReportImage>(AppStateKinds.Undoable);
         snapShot = snapShot.AddCollection<ReportHeaderForm>(AppStateKinds.Undoable);
         snapShot = snapShot.AddCollection<ReportTableView>(AppStateKinds.Undoable);
         snapShot = snapShot.AddCollection<ReportTessellationView>(AppStateKinds.Undoable);
         snapShot = snapShot.AddCollection<AllCharacteristicTypes>(AppStateKinds.Undoable);
         snapShot = snapShot.AddCollection<ReportBody>(AppStateKinds.Undoable);

         return snapShot;
      }

      [SetUp]
      public virtual void SetUp()
      {
         SetUpHelper(BuildHelper);
      }

      [Test]
      public void OnGetAllReportComponents()
      {
         var snapshot = _history.NextSnapShot(ControllerCall.Empty);

         var body = new ReportBody();
         snapshot = snapshot.AddItem(body);

         var reportPlacement = new ReportComponentPlacement(body.Id, 10, 20, 30, 40);

         ReportHeaderForm reportHeaderForm = new ReportHeaderForm(reportPlacement, new Id<ReportHeaderFormRow>[] { });
         ReportImage reportImage = new ReportImage(reportPlacement);
         ReportTableView reportTableView = new ReportTableView(reportPlacement);
         ReportTessellationView reportTessellationView = new ReportTessellationView(reportPlacement);
         ReportTextBox reportTextBox = new ReportTextBox(reportPlacement);

         snapshot = snapshot.AddItem(reportHeaderForm);
         snapshot = snapshot.AddItem(reportImage);
         snapshot = snapshot.AddItem(reportTableView);
         snapshot = snapshot.AddItem(reportTessellationView);
         snapshot = snapshot.AddItem(reportTextBox);

         _history.AddSnapShot(snapshot);

         // Act
         var reportComponents = _history.CurrentSnapShot.GetAllReportComponents();

         // Assert
         Assert.AreEqual(reportComponents.Count(), 5);
         Assert.IsTrue(reportComponents.Contains(reportHeaderForm));
         Assert.IsTrue(reportComponents.Contains(reportImage));
         Assert.IsTrue(reportComponents.Contains(reportTableView));
         Assert.IsTrue(reportComponents.Contains(reportTessellationView));
         Assert.IsTrue(reportComponents.Contains(reportTextBox));
      }

      [Test]
      public void OnAddOrUpdateSnapShotWhenComponentAlreadyExists()
      {
         // Arrange
         var snapshot = _history.NextSnapShot(ControllerCall.Empty);

         var body = new ReportBody();
         snapshot = snapshot.AddItem(body);

         var reportPlacement = new ReportComponentPlacement(body.Id, 10, 20, 30, 40);

         ReportTextBox component = new ReportTextBox(reportPlacement);
         snapshot = snapshot.AddItem(component);
         _history.AddSnapShot(snapshot);

         var updatedComponent = component.WithText("UpdatedText");

         // Act
         snapshot = _history.CurrentSnapShot.UpdateItem(component, updatedComponent);

         var resultComponent = snapshot.GetItem(component.Id);

         // Assert
         Assert.AreEqual(resultComponent.Text, "UpdatedText");
      }

      [Test]
      public void OnAddOrUpdateSnapShotWhenComponentDoNotExist()
      {
         // Arrange
         var snapshot = _history.NextSnapShot(ControllerCall.Empty);
         _history.AddSnapShot(snapshot);

         var body = new ReportBody();
         snapshot = snapshot.AddItem(body);

         var reportPlacement = new ReportComponentPlacement(body.Id, 10, 20, 30, 40);

         ReportTextBox component = new ReportTextBox(reportPlacement);
         snapshot = snapshot.AddItem(component);

         var updatedComponent = component.WithText("UpdatedText");

         // Act
         snapshot = snapshot.UpdateItem(component, updatedComponent);

         var resultComponent = snapshot.GetItem(component.Id);

         // Assert
         Assert.AreEqual(resultComponent.Text, "UpdatedText");
      }

      [Test]
      public void GetAllReportComponentOfType()
      {
         var snapshot = _history.NextSnapShot(ControllerCall.Empty);

         var body = new ReportBody();
         snapshot = snapshot.AddItem(body);

         var reportImagePlacement = new ReportComponentPlacement(body.Id, 10, 20, 30, 40);

         ReportImage reportImage = new ReportImage(reportImagePlacement);
         ReportTextBox reportTextBox1 = new ReportTextBox(reportImagePlacement);
         ReportTextBox reportTextBox2 = new ReportTextBox(reportImagePlacement);

         snapshot = snapshot.AddItem(reportImage);
         snapshot = snapshot.AddItem(reportTextBox1);
         snapshot = snapshot.AddItem(reportTextBox2);

         _history.AddSnapShot(snapshot);

         // Act
         var imageComponents = _history.CurrentSnapShot.GetAllReportComponents().Where(c => c is ReportImage);
         var textboxComponents = _history.CurrentSnapShot.GetAllReportComponents().Where(c => c is ReportTextBox);

         // Assert
         Assert.AreEqual(imageComponents.Count(), 1);
         Assert.AreEqual(imageComponents.ElementAt(0), reportImage);

         Assert.AreEqual(textboxComponents.Count(), 2);
         Assert.IsTrue(textboxComponents.Contains(reportTextBox1));
         Assert.IsTrue(textboxComponents.Contains(reportTextBox2));
      }

      [Test]
      public void GetSelectedMeasurementResultShouldReturnMeasurementResultState()
      {
         var measurementResultState = new AllCharacteristicTypes(System.Collections.Immutable.ImmutableList.Create<string>("Characteristic 1", "Characteristic 2"));

         var newSnapShot = _history.NextSnapShot(ControllerCall.Empty);
         newSnapShot = newSnapShot.AddItem(measurementResultState);
         _history.AddSnapShot(newSnapShot);

         var currentMeasurementeResultState = _history.CurrentSnapShot.GetItems<AllCharacteristicTypes>().SingleOrDefault();
         Assert.AreEqual(currentMeasurementeResultState.CharacteristicTypes.Count(), 2);
      }
   }
}
