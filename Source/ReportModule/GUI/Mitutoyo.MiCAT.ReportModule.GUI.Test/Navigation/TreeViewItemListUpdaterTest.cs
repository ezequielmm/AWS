// <copyright file="TreeViewItemListUpdaterTest.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Windows.Input;
using Mitutoyo.MiCAT.ReportModule.GUI.Navigation.TreeView;
using Mitutoyo.MiCAT.ReportModule.GUI.Utilities;
using NUnit.Framework;

namespace Mitutoyo.MiCAT.ReportModule.GUI.Test.Navigation
{
   [ExcludeFromCodeCoverage]
   [TestFixture]
   public class TreeViewItemListUpdaterTest
   {
      private class FakeTreeViewNode : IVMNodeTreeViewItem
      {
         public Guid Id { get; set; }
         public string Name { get; set; }

         public ObservableCollection<IVMNodeTreeViewItem> ChildrenVms { get; set; }

         public IAsyncCommand DeleteItemCommand { get; set; }

         public IAsyncCommand SelectItemCommand { get; set; }

         public bool CanDeleteItem { get; set; }
         public bool IsSelected { get; set; }
         public bool IsExpanded { get; set; }
         public bool IsLoading { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

         ICommand IVMNodeTreeViewItem.SelectItemCommand => throw new NotImplementedException();
      }

      private ObservableCollection<IVMNodeTreeViewItem> _existingTreeViewNodes;
      private Guid firstItemId = Guid.NewGuid();
      private Guid secondItemId = Guid.NewGuid();
      private Guid thirdItemId = Guid.NewGuid();
      private Guid fourthItemId = Guid.NewGuid();
      private Guid fifthItemId = Guid.NewGuid();
      private Guid sixthItemId = Guid.NewGuid();

      [SetUp]
      public void Setup()
      {
         _existingTreeViewNodes = new ObservableCollection<IVMNodeTreeViewItem>()
         {
            new FakeTreeViewNode() { Id = firstItemId, ChildrenVms = new ObservableCollection<IVMNodeTreeViewItem>(), },
            new FakeTreeViewNode() { Id = secondItemId, ChildrenVms = new ObservableCollection<IVMNodeTreeViewItem>(), },
            new FakeTreeViewNode() { Id = thirdItemId, ChildrenVms = new ObservableCollection<IVMNodeTreeViewItem>(), },
            new FakeTreeViewNode() { Id = fourthItemId, ChildrenVms = new ObservableCollection<IVMNodeTreeViewItem>(), }
         };
      }

      [Test]
      public void ShouldRemoveOldTreeViewNodes()
      {
         // Arrange
         var sut = new TreeViewItemListUpdater();
         var newItems = new List<IVMNodeTreeViewItem>()
         {
            new FakeTreeViewNode() { Id = secondItemId, ChildrenVms = new ObservableCollection<IVMNodeTreeViewItem>(), },
            new FakeTreeViewNode() { Id = fourthItemId, ChildrenVms = new ObservableCollection<IVMNodeTreeViewItem>(), },
         };

         // Act
         sut.UpdateTreeViewItems(_existingTreeViewNodes, newItems);

         //Assert
         Assert.AreEqual(2, _existingTreeViewNodes.Count);
         Assert.AreEqual(secondItemId, _existingTreeViewNodes[0].Id);
         Assert.AreEqual(fourthItemId, _existingTreeViewNodes[1].Id);
      }

      [Test]
      public void ShouldAddNewTreeViewNodes()
      {
         // Arrange
         var sut = new TreeViewItemListUpdater();

         var addedItems = new List<FakeTreeViewNode>()
         {
            new FakeTreeViewNode() { Id = fifthItemId, ChildrenVms = new ObservableCollection<IVMNodeTreeViewItem>(), },
            new FakeTreeViewNode() { Id = sixthItemId, ChildrenVms = new ObservableCollection<IVMNodeTreeViewItem>(), },
         };
         var newItems = new List<IVMNodeTreeViewItem>(_existingTreeViewNodes);
         newItems.AddRange(addedItems);

         // Act
         sut.UpdateTreeViewItems(_existingTreeViewNodes, newItems);

         //Assert
         Assert.AreEqual(6, _existingTreeViewNodes.Count);
         Assert.AreEqual(firstItemId, _existingTreeViewNodes[0].Id);
         Assert.AreEqual(secondItemId, _existingTreeViewNodes[1].Id);
         Assert.AreEqual(thirdItemId, _existingTreeViewNodes[2].Id);
         Assert.AreEqual(fourthItemId, _existingTreeViewNodes[3].Id);
         Assert.AreEqual(fifthItemId, _existingTreeViewNodes[4].Id);
         Assert.AreEqual(sixthItemId, _existingTreeViewNodes[5].Id);
      }

      [Test]
      public void ShouldAddAndRemoveTreeViewNodes()
      {
         // Arrange
         var sut = new TreeViewItemListUpdater();

         var addedItems = new List<FakeTreeViewNode>()
         {
            new FakeTreeViewNode() { Id = fifthItemId, ChildrenVms = new ObservableCollection<IVMNodeTreeViewItem>(), },
            new FakeTreeViewNode() { Id = sixthItemId, ChildrenVms = new ObservableCollection<IVMNodeTreeViewItem>(), },
         };
         var newItems = new List<IVMNodeTreeViewItem>(_existingTreeViewNodes);
         newItems.RemoveAt(1);
         newItems.RemoveAt(2);
         newItems.AddRange(addedItems);

         // Act
         sut.UpdateTreeViewItems(_existingTreeViewNodes, newItems);

         //Assert
         Assert.AreEqual(4, _existingTreeViewNodes.Count);
         Assert.AreEqual(firstItemId, _existingTreeViewNodes[0].Id);
         Assert.AreEqual(thirdItemId, _existingTreeViewNodes[1].Id);
         Assert.AreEqual(fifthItemId, _existingTreeViewNodes[2].Id);
         Assert.AreEqual(sixthItemId, _existingTreeViewNodes[3].Id);
      }
   }
}
