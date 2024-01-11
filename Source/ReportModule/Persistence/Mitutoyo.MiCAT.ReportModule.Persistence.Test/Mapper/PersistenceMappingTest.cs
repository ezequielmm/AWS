// <copyright file="PersistenceMappingTest.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using AutoMapper;
using Mitutoyo.MiCAT.ApplicationState;
using Mitutoyo.MiCAT.Core.Geometry;
using Mitutoyo.MiCAT.ReportModule.Domain.CADElements;
using Mitutoyo.MiCAT.ReportModule.Domain.CADElements.Callouts2D;
using Mitutoyo.MiCAT.ReportModule.Domain.CADElements.Callouts2D.FeatureCallouts;
using Mitutoyo.MiCAT.ReportModule.Domain.CADElements.Callouts3D;
using Mitutoyo.MiCAT.ReportModule.Domain.Components;
using Mitutoyo.MiCAT.ReportModule.Persistence.CADElements;
using Mitutoyo.MiCAT.ReportModule.Persistence.CADElements.Callouts2D;
using Mitutoyo.MiCAT.ReportModule.Persistence.CADElements.Callouts2D.FeatureCallouts;
using Mitutoyo.MiCAT.ReportModule.Persistence.CADElements.Callouts3D;
using Mitutoyo.MiCAT.ReportModule.Persistence.Components;
using Mitutoyo.MiCAT.ReportModule.Persistence.Components.TableView;
using Mitutoyo.MiCAT.ReportModule.Setup.Configurations;
using NUnit.Framework;
using Direction3D = Mitutoyo.MiCAT.ReportModule.Domain.CADElements.Direction3D;

namespace Mitutoyo.MiCAT.ReportModule.Persistence.Test.Mapper
{
   [ExcludeFromCodeCoverage]
   [TestFixture]
   public class PersistenceMappingTest
   {
      private IMapper Mapper;

      [SetUp]
      public virtual void SetUp()
      {
         Mapper = AutoMapperConfig.InitializeAutoMapper().CreateMapper();
      }

      [Test]
      public void Map_ReportTextBoxSOToReportText()
      {
         // Arrange
         var reportTextBoxExpectedSO = new ReportTextBoxSO { Id = Guid.NewGuid(), X = 12, Y = 15, Width = 100, Height = 110, Text = "Sample Test" };

         // Act
         ReportTextBox reportTextBoxActual = Mapper.Map<ReportTextBoxSO, ReportTextBox>(reportTextBoxExpectedSO);
         var placement = reportTextBoxActual.Placement;

         // Assert
         Assert.AreEqual(reportTextBoxExpectedSO.X, placement.X);
         Assert.AreEqual(reportTextBoxExpectedSO.Y, placement.Y);
         Assert.AreEqual(reportTextBoxExpectedSO.Width, placement.Width);
         Assert.AreEqual(reportTextBoxExpectedSO.Height, placement.Height);
         Assert.AreEqual(reportTextBoxExpectedSO.Text, reportTextBoxActual.Text);
      }

      [Test]
      public void Map_ReportTableViewSOToReportTableView()
      {
         // Arrange
         var reportTableViewSO = new ReportTableViewSO { Id = Guid.NewGuid(), X = 12, Y = 15, Width = 100, Height = 110, Columns = new List<ColumnSO>(), Filters = new List<FilterColumnSO>(), GroupBy = new List<string>(), Sorting = new List<SortingColumnSO>()};

         // Act
         ReportTableView reportTableViewExpected = Mapper.Map<ReportTableViewSO, ReportTableView>(reportTableViewSO);
         var placement = reportTableViewExpected.Placement;

         // Assert
         Assert.AreEqual(reportTableViewSO.Id, reportTableViewExpected.Id.UniqueValue.Value);
         Assert.AreEqual(reportTableViewSO.X, placement.X);
         Assert.AreEqual(reportTableViewSO.Y, placement.Y);
         Assert.AreEqual(reportTableViewSO.Width, placement.Width);
         Assert.AreEqual(reportTableViewSO.Height, placement.Height);
      }

      [Test]
      public void Map_ReportImageSOToReportImage()
      {
         // Arrange
         var id = Guid.NewGuid();
         var reportImageSO = new ReportImageSO() { Id = id, X = 12, Y = 15, Width = 100, Height = 150,  Image = "image" };

         // Act
         var reportImage = Mapper.Map<ReportImageSO, ReportImage>(reportImageSO);
         var placement = reportImage.Placement;

         // Assert
         Assert.AreEqual(reportImageSO.X, placement.X);
         Assert.AreEqual(reportImageSO.Y, placement.Y);
         Assert.AreEqual(reportImageSO.Width, placement.Width);
         Assert.AreEqual(reportImageSO.Height, placement.Height);
         Assert.AreEqual(reportImageSO.Image, reportImage.Image);
      }

      [Test]
      public void Map_Point3DToPoint3DSO()
      {
         // Arrange
         Point3D point3D = new Point3D() { X = 1.01f, Y = 1.02f, Z = 1.03f };

         // Act
         var point3DSO = Mapper.Map<Point3D, Point3DSO>(point3D);

         // Assert
         Assert.AreEqual(point3D.X, point3DSO.X);
         Assert.AreEqual(point3D.Y, point3DSO.Y);
         Assert.AreEqual(point3D.Z, point3DSO.Z);
      }
      [Test]
      public void Map_Point3DSOSOToPoint3D()
      {
         // Arrange
         Point3DSO point3DSO = new Point3DSO() { X = 1.01f, Y = 1.02f, Z = 1.03f };

         // Act
         var point3D = Mapper.Map<Point3DSO, Point3D>(point3DSO);

         // Assert
         Assert.AreEqual(point3DSO.X, point3D.X);
         Assert.AreEqual(point3DSO.Y, point3D.Y);
         Assert.AreEqual(point3DSO.Z, point3D.Z);
      }

      [Test]
      public void Map_Point2DToPoint2DSO()
      {
         // Arrange
         Point2D point2D = new Point2D() { X = 1.01f, Y = 1.02f };

         // Act
         var point2DSO = Mapper.Map<Point2D, Point2DSO>(point2D);

         // Assert
         Assert.AreEqual(point2D.X, point2DSO.X);
         Assert.AreEqual(point2D.Y, point2DSO.Y);
      }
      [Test]
      public void Map_Point2DSOSOToPoint2D()
      {
         // Arrange
         Point2DSO point2DSO = new Point2DSO() { X = 1.01f, Y = 1.02f };

         // Act
         var point2D = Mapper.Map<Point2DSO, Point2D>(point2DSO);

         // Assert
         Assert.AreEqual(point2DSO.X, point2D.X);
         Assert.AreEqual(point2DSO.Y, point2D.Y);
      }

      [Test]
      public void Map_AnchorToAnchorSO()
      {
         // Arrange
         var anchor = new Anchor(new Point3D(3.01f, 3.02f, 3.03f));

         // Act
         var anchorSO = Mapper.Map<Anchor, AnchorSO>(anchor);

         // Assert
         Assert.IsInstanceOf(typeof(Point3DSO), anchorSO.LeaderLineOrigin);
         Assert.AreEqual(anchor.LeaderLineOrigin.X, anchorSO.LeaderLineOrigin.X);
         Assert.AreEqual(anchor.LeaderLineOrigin.Y, anchorSO.LeaderLineOrigin.Y);
         Assert.AreEqual(anchor.LeaderLineOrigin.Z, anchorSO.LeaderLineOrigin.Z);
      }
      [Test]
      public void Map_AnchorSOToAnchor()
      {
         // Arrange
         var anchorSO = new AnchorSO() { LeaderLineOrigin = new Point3DSO() { X = 3.01f, Y = 3.02f, Z = 3.03f } };

         // Act
         var anchor = Mapper.Map<AnchorSO, Anchor>(anchorSO);

         // Assert
         Assert.IsInstanceOf(typeof(Point3D), anchor.LeaderLineOrigin);
         Assert.AreEqual(anchorSO.LeaderLineOrigin.X, anchor.LeaderLineOrigin.X);
         Assert.AreEqual(anchorSO.LeaderLineOrigin.Y, anchor.LeaderLineOrigin.Y);
         Assert.AreEqual(anchorSO.LeaderLineOrigin.Z, anchor.LeaderLineOrigin.Z);
      }

      [Test]
      public void Map_CameraToCameraSO()
      {
         // Arrange

         Camera camera = new Camera(
            new Point3D(1.01f, 1.02f, 1.03f),
            new Point3D(2.01f, 2.02f, 2.03f),
            new Direction3D(1, -1, 1),
            0.01f,
            0.02f,
            true);

         // Act
         var cameraSO = Mapper.Map<Camera, CameraSO>(camera);

         // Assert Camera
         Assert.AreEqual(camera.FieldHeight, cameraSO.FieldHeight);
         Assert.AreEqual(camera.FieldWidht, cameraSO.FieldWidht);
         Assert.AreEqual(camera.PerspectiveProjection, cameraSO.PerspectiveProjection);
         Assert.AreEqual(camera.Position.X, cameraSO.Position.X);
         Assert.AreEqual(camera.Position.Y, cameraSO.Position.Y);
         Assert.AreEqual(camera.Position.Z, cameraSO.Position.Z);
         Assert.AreEqual(camera.Target.X, cameraSO.Target.X);
         Assert.AreEqual(camera.Target.Y, cameraSO.Target.Y);
         Assert.AreEqual(camera.Target.Z, cameraSO.Target.Z);
         Assert.AreEqual(camera.UpVector.X, cameraSO.UpVector.X);
         Assert.AreEqual(camera.UpVector.Y, cameraSO.UpVector.Y);
         Assert.AreEqual(camera.UpVector.Z, cameraSO.UpVector.Z);
      }

      [Test]
      public void Map_CameraSOToCamera()
      {
         // Arrange
         var cameraSO = new CameraSO()
         {
            FieldHeight = 0.01f,
            FieldWidht = 0.02f,
            PerspectiveProjection = true,
            Position = new Point3DSO() { X = 1.01f, Y = 1.02f, Z = 1.03f },
            Target = new Point3DSO() { X = 2.01f, Y = 2.02f, Z = 2.03f }
         };

         // Act
         var camera = Mapper.Map<CameraSO, Camera>(cameraSO);

         // Assert Camera
         Assert.AreEqual(cameraSO.FieldHeight, camera.FieldHeight);
         Assert.AreEqual(cameraSO.FieldWidht, camera.FieldWidht);
         Assert.AreEqual(cameraSO.PerspectiveProjection, camera.PerspectiveProjection);
         Assert.AreEqual(cameraSO.Position.X, camera.Position.X);
         Assert.AreEqual(cameraSO.Position.Y, camera.Position.Y);
         Assert.AreEqual(cameraSO.Position.Z, camera.Position.Z);
         Assert.AreEqual(cameraSO.Target.X, camera.Target.X);
         Assert.AreEqual(cameraSO.Target.Y, camera.Target.Y);
         Assert.AreEqual(cameraSO.Target.Z, camera.Target.Z);
      }

      [Test]
      public void Map_CircleCalloutToCircleCallout2DSO()
      {
         // Arrange
         var circleCallout = new CircleCallout(
            new Anchor(new Point3D(3.01f, 3.02f, 3.03f)),
            new Point2D(4.01f, 4.02f));

         // Act
         var circleCalloutSO = Mapper.Map<CircleCallout, CircleCalloutSO>(circleCallout);

         // Assert
         Assert.IsInstanceOf(typeof(AnchorSO), circleCalloutSO.Anchor);
         Assert.IsInstanceOf(typeof(Point2DSO), circleCalloutSO.Position);
         Assert.AreEqual(circleCallout.Position.X, circleCalloutSO.Position.X);
         Assert.AreEqual(circleCallout.Position.Y, circleCalloutSO.Position.Y);
         Assert.AreEqual(circleCallout.Anchor.LeaderLineOrigin.X, circleCalloutSO.Anchor.LeaderLineOrigin.X);
         Assert.AreEqual(circleCallout.Anchor.LeaderLineOrigin.Y, circleCalloutSO.Anchor.LeaderLineOrigin.Y);
         Assert.AreEqual(circleCallout.Anchor.LeaderLineOrigin.Z, circleCalloutSO.Anchor.LeaderLineOrigin.Z);
      }
      [Test]
      public void Map_CircleCalloutSOToCircleCallout()
      {
         // Arrange
         var circleCalloutSO = new CircleCalloutSO()
         {
            Anchor = new AnchorSO() { LeaderLineOrigin = new Point3DSO() { X = 3.01f, Y = 3.02f, Z = 3.03f } },
            Position = new Point2DSO() { X = 4.01f, Y = 4.02f }
         };

         // Act
         var coneCallout = Mapper.Map<CircleCalloutSO, CircleCallout>(circleCalloutSO);

         // Assert
         Assert.IsInstanceOf(typeof(Anchor), coneCallout.Anchor);
         Assert.IsInstanceOf(typeof(Point2D), coneCallout.Position);
         Assert.AreEqual(circleCalloutSO.Position.X, coneCallout.Position.X);
         Assert.AreEqual(circleCalloutSO.Position.Y, coneCallout.Position.Y);
         Assert.AreEqual(circleCalloutSO.Anchor.LeaderLineOrigin.X, coneCallout.Anchor.LeaderLineOrigin.X);
         Assert.AreEqual(circleCalloutSO.Anchor.LeaderLineOrigin.Y, coneCallout.Anchor.LeaderLineOrigin.Y);
         Assert.AreEqual(circleCalloutSO.Anchor.LeaderLineOrigin.Z, coneCallout.Anchor.LeaderLineOrigin.Z);
      }

      [Test]
      public void Map_ConeCalloutToConeCallout2DSO()
      {
         // Arrange
         var coneCallout = new ConeCallout(
            new Anchor(new Point3D(3.01f, 3.02f, 3.03f)),
            new Point2D(4.01f, 4.02f));

         // Act
         var coneCalloutSO = Mapper.Map<ConeCallout, ConeCalloutSO>(coneCallout);

         // Assert
         Assert.IsInstanceOf(typeof(AnchorSO), coneCalloutSO.Anchor);
         Assert.IsInstanceOf(typeof(Point2DSO), coneCalloutSO.Position);
         Assert.AreEqual(coneCallout.Position.X, coneCalloutSO.Position.X);
         Assert.AreEqual(coneCallout.Position.Y, coneCalloutSO.Position.Y);
         Assert.AreEqual(coneCallout.Anchor.LeaderLineOrigin.X, coneCalloutSO.Anchor.LeaderLineOrigin.X);
         Assert.AreEqual(coneCallout.Anchor.LeaderLineOrigin.Y, coneCalloutSO.Anchor.LeaderLineOrigin.Y);
         Assert.AreEqual(coneCallout.Anchor.LeaderLineOrigin.Z, coneCalloutSO.Anchor.LeaderLineOrigin.Z);
      }
      [Test]
      public void Map_ConeCalloutSOToConeCallout()
      {
         // Arrange
         var coneCalloutSO = new ConeCalloutSO()
         {
            Anchor = new AnchorSO() { LeaderLineOrigin = new Point3DSO() { X = 3.01f, Y = 3.02f, Z = 3.03f } },
            Position = new Point2DSO() { X = 4.01f, Y = 4.02f }
         };

         // Act
         var coneCallout = Mapper.Map<ConeCalloutSO, ConeCallout>(coneCalloutSO);

         // Assert
         Assert.IsInstanceOf(typeof(Anchor), coneCallout.Anchor);
         Assert.IsInstanceOf(typeof(Point2D), coneCallout.Position);
         Assert.AreEqual(coneCalloutSO.Position.X, coneCallout.Position.X);
         Assert.AreEqual(coneCalloutSO.Position.Y, coneCallout.Position.Y);
         Assert.AreEqual(coneCalloutSO.Anchor.LeaderLineOrigin.X, coneCallout.Anchor.LeaderLineOrigin.X);
         Assert.AreEqual(coneCalloutSO.Anchor.LeaderLineOrigin.Y, coneCallout.Anchor.LeaderLineOrigin.Y);
         Assert.AreEqual(coneCalloutSO.Anchor.LeaderLineOrigin.Z, coneCallout.Anchor.LeaderLineOrigin.Z);
      }

      [Test]
      public void Map_CylinderCalloutToCylinderCallout2DSO()
      {
         // Arrange
         var cylinderCallout = new CylinderCallout(
            new Anchor(new Point3D(3.01f, 3.02f, 3.03f)),
            new Point2D(4.01f, 4.02f));

         // Act
         var cylinderCalloutSO = Mapper.Map<CylinderCallout, CylinderCalloutSO>(cylinderCallout);

         // Assert
         Assert.IsInstanceOf(typeof(CylinderCalloutSO), cylinderCalloutSO);
         Assert.IsInstanceOf(typeof(AnchorSO), cylinderCalloutSO.Anchor);
         Assert.IsInstanceOf(typeof(Point2DSO), cylinderCalloutSO.Position);
         Assert.AreEqual(cylinderCallout.Position.X, cylinderCalloutSO.Position.X);
         Assert.AreEqual(cylinderCallout.Position.Y, cylinderCalloutSO.Position.Y);
         Assert.AreEqual(cylinderCallout.Anchor.LeaderLineOrigin.X, cylinderCalloutSO.Anchor.LeaderLineOrigin.X);
         Assert.AreEqual(cylinderCallout.Anchor.LeaderLineOrigin.Y, cylinderCalloutSO.Anchor.LeaderLineOrigin.Y);
         Assert.AreEqual(cylinderCallout.Anchor.LeaderLineOrigin.Z, cylinderCalloutSO.Anchor.LeaderLineOrigin.Z);
      }
      [Test]
      public void Map_CylinderCalloutSOToCylinderCallout()
      {
         // Arrange
         var cylinderCalloutSO = new CylinderCalloutSO()
         {
            Anchor = new AnchorSO() { LeaderLineOrigin = new Point3DSO() { X = 3.01f, Y = 3.02f, Z = 3.03f } },
            Position = new Point2DSO() { X = 4.01f, Y = 4.02f }
         };

         // Act
         var cylinderCallout = Mapper.Map<CylinderCalloutSO, CylinderCallout>(cylinderCalloutSO);

         // Assert
         Assert.IsInstanceOf(typeof(CylinderCallout), cylinderCallout);
         Assert.IsInstanceOf(typeof(Anchor), cylinderCallout.Anchor);
         Assert.IsInstanceOf(typeof(Point2D), cylinderCallout.Position);
         Assert.AreEqual(cylinderCalloutSO.Position.X, cylinderCallout.Position.X);
         Assert.AreEqual(cylinderCalloutSO.Position.Y, cylinderCallout.Position.Y);
         Assert.AreEqual(cylinderCalloutSO.Anchor.LeaderLineOrigin.X, cylinderCallout.Anchor.LeaderLineOrigin.X);
         Assert.AreEqual(cylinderCalloutSO.Anchor.LeaderLineOrigin.Y, cylinderCallout.Anchor.LeaderLineOrigin.Y);
         Assert.AreEqual(cylinderCalloutSO.Anchor.LeaderLineOrigin.Z, cylinderCallout.Anchor.LeaderLineOrigin.Z);
      }

      [Test]
      public void Map_LineCalloutToLineCallout2DSO()
      {
         // Arrange
         var lineCallout = new LineCallout(
            new Anchor(new Point3D(3.01f, 3.02f, 3.03f)),
            new Point2D(4.01f, 4.02f));

         // Act
         var lineCalloutSO = Mapper.Map<LineCallout, LineCalloutSO>(lineCallout);

         // Assert
         Assert.IsInstanceOf(typeof(AnchorSO), lineCalloutSO.Anchor);
         Assert.IsInstanceOf(typeof(Point2DSO), lineCalloutSO.Position);
         Assert.AreEqual(lineCallout.Position.X, lineCalloutSO.Position.X);
         Assert.AreEqual(lineCallout.Position.Y, lineCalloutSO.Position.Y);
         Assert.AreEqual(lineCallout.Anchor.LeaderLineOrigin.X, lineCalloutSO.Anchor.LeaderLineOrigin.X);
         Assert.AreEqual(lineCallout.Anchor.LeaderLineOrigin.Y, lineCalloutSO.Anchor.LeaderLineOrigin.Y);
         Assert.AreEqual(lineCallout.Anchor.LeaderLineOrigin.Z, lineCalloutSO.Anchor.LeaderLineOrigin.Z);
      }
      [Test]
      public void Map_LineCalloutSOToLineCallout()
      {
         // Arrange
         var lineCalloutSO = new LineCalloutSO()
         {
            Anchor = new AnchorSO() { LeaderLineOrigin = new Point3DSO() { X = 3.01f, Y = 3.02f, Z = 3.03f } },
            Position = new Point2DSO() { X = 4.01f, Y = 4.02f }
         };

         // Act
         var lineCallout = Mapper.Map<LineCalloutSO, LineCallout>(lineCalloutSO);

         // Assert
         Assert.IsInstanceOf(typeof(Anchor), lineCallout.Anchor);
         Assert.IsInstanceOf(typeof(Point2D), lineCallout.Position);
         Assert.AreEqual(lineCalloutSO.Position.X, lineCallout.Position.X);
         Assert.AreEqual(lineCalloutSO.Position.Y, lineCallout.Position.Y);
         Assert.AreEqual(lineCalloutSO.Anchor.LeaderLineOrigin.X, lineCallout.Anchor.LeaderLineOrigin.X);
         Assert.AreEqual(lineCalloutSO.Anchor.LeaderLineOrigin.Y, lineCallout.Anchor.LeaderLineOrigin.Y);
         Assert.AreEqual(lineCalloutSO.Anchor.LeaderLineOrigin.Z, lineCallout.Anchor.LeaderLineOrigin.Z);
      }

      [Test]
      public void Map_PlaneCalloutToPlaneCallout2DSO()
      {
         // Arrange
         var planeCallout = new PlaneCallout(
            new Anchor(new Point3D(3.01f, 3.02f, 3.03f)),
            new Point2D(4.01f, 4.02f));

         // Act
         var planeCalloutSO = Mapper.Map<PlaneCallout, PlaneCalloutSO>(planeCallout);

         // Assert
         Assert.IsInstanceOf(typeof(AnchorSO), planeCalloutSO.Anchor);
         Assert.IsInstanceOf(typeof(Point2DSO), planeCalloutSO.Position);
         Assert.AreEqual(planeCallout.Position.X, planeCalloutSO.Position.X);
         Assert.AreEqual(planeCallout.Position.Y, planeCalloutSO.Position.Y);
         Assert.AreEqual(planeCallout.Anchor.LeaderLineOrigin.X, planeCalloutSO.Anchor.LeaderLineOrigin.X);
         Assert.AreEqual(planeCallout.Anchor.LeaderLineOrigin.Y, planeCalloutSO.Anchor.LeaderLineOrigin.Y);
         Assert.AreEqual(planeCallout.Anchor.LeaderLineOrigin.Z, planeCalloutSO.Anchor.LeaderLineOrigin.Z);
      }
      [Test]
      public void Map_PlaneCalloutSOToPlaneCallout()
      {
         // Arrange
         var planeCalloutSO = new PlaneCalloutSO()
         {
            Anchor = new AnchorSO() { LeaderLineOrigin = new Point3DSO() { X = 3.01f, Y = 3.02f, Z = 3.03f } },
            Position = new Point2DSO() { X = 4.01f, Y = 4.02f }
         };

         // Act
         var planeCallout = Mapper.Map<PlaneCalloutSO, PlaneCallout>(planeCalloutSO);

         // Assert
         Assert.IsInstanceOf(typeof(Anchor), planeCallout.Anchor);
         Assert.IsInstanceOf(typeof(Point2D), planeCallout.Position);
         Assert.AreEqual(planeCalloutSO.Position.X, planeCallout.Position.X);
         Assert.AreEqual(planeCalloutSO.Position.Y, planeCallout.Position.Y);
         Assert.AreEqual(planeCalloutSO.Anchor.LeaderLineOrigin.X, planeCallout.Anchor.LeaderLineOrigin.X);
         Assert.AreEqual(planeCalloutSO.Anchor.LeaderLineOrigin.Y, planeCallout.Anchor.LeaderLineOrigin.Y);
         Assert.AreEqual(planeCalloutSO.Anchor.LeaderLineOrigin.Z, planeCallout.Anchor.LeaderLineOrigin.Z);
      }

      [Test]
      public void Map_PointCalloutToPointCallout2DSO()
      {
         // Arrange
         var pointCallout = new PointCallout(
            new Anchor(new Point3D(3.01f, 3.02f, 3.03f)),
            new Point2D(4.01f, 4.02f));

         // Act
         var pointCalloutSO = Mapper.Map<PointCallout, PointCalloutSO>(pointCallout);

         // Assert
         Assert.IsInstanceOf(typeof(AnchorSO), pointCalloutSO.Anchor);
         Assert.IsInstanceOf(typeof(Point2DSO), pointCalloutSO.Position);
         Assert.AreEqual(pointCallout.Position.X, pointCalloutSO.Position.X);
         Assert.AreEqual(pointCallout.Position.Y, pointCalloutSO.Position.Y);
         Assert.AreEqual(pointCallout.Anchor.LeaderLineOrigin.X, pointCalloutSO.Anchor.LeaderLineOrigin.X);
         Assert.AreEqual(pointCallout.Anchor.LeaderLineOrigin.Y, pointCalloutSO.Anchor.LeaderLineOrigin.Y);
         Assert.AreEqual(pointCallout.Anchor.LeaderLineOrigin.Z, pointCalloutSO.Anchor.LeaderLineOrigin.Z);
      }
      [Test]
      public void Map_PointCalloutSOToPointCallout()
      {
         // Arrange
         var pointCalloutSO = new PointCalloutSO()
         {
            Anchor = new AnchorSO() { LeaderLineOrigin = new Point3DSO() { X = 3.01f, Y = 3.02f, Z = 3.03f } },
            Position = new Point2DSO() { X = 4.01f, Y = 4.02f }
         };

         // Act
         var pointCallout = Mapper.Map<PointCalloutSO, PointCallout>(pointCalloutSO);

         // Assert
         Assert.IsInstanceOf(typeof(Anchor), pointCallout.Anchor);
         Assert.IsInstanceOf(typeof(Point2D), pointCallout.Position);
         Assert.AreEqual(pointCalloutSO.Position.X, pointCallout.Position.X);
         Assert.AreEqual(pointCalloutSO.Position.Y, pointCallout.Position.Y);
         Assert.AreEqual(pointCalloutSO.Anchor.LeaderLineOrigin.X, pointCallout.Anchor.LeaderLineOrigin.X);
         Assert.AreEqual(pointCalloutSO.Anchor.LeaderLineOrigin.Y, pointCallout.Anchor.LeaderLineOrigin.Y);
         Assert.AreEqual(pointCalloutSO.Anchor.LeaderLineOrigin.Z, pointCallout.Anchor.LeaderLineOrigin.Z);
      }

      [Test]
      public void Map_SphereCalloutToSphereCallout2DSO()
      {
         // Arrange
         var sphereCallout = new SphereCallout(
            new Anchor(new Point3D(3.01f, 3.02f, 3.03f)),
            new Point2D(4.01f, 4.02f));

         // Act
         var sphereCalloutSO = Mapper.Map<SphereCallout, SphereCalloutSO>(sphereCallout);

         // Assert
         Assert.IsInstanceOf(typeof(AnchorSO), sphereCalloutSO.Anchor);
         Assert.IsInstanceOf(typeof(Point2DSO), sphereCalloutSO.Position);
         Assert.AreEqual(sphereCallout.Position.X, sphereCalloutSO.Position.X);
         Assert.AreEqual(sphereCallout.Position.Y, sphereCalloutSO.Position.Y);
         Assert.AreEqual(sphereCallout.Anchor.LeaderLineOrigin.X, sphereCalloutSO.Anchor.LeaderLineOrigin.X);
         Assert.AreEqual(sphereCallout.Anchor.LeaderLineOrigin.Y, sphereCalloutSO.Anchor.LeaderLineOrigin.Y);
         Assert.AreEqual(sphereCallout.Anchor.LeaderLineOrigin.Z, sphereCalloutSO.Anchor.LeaderLineOrigin.Z);
      }
      [Test]
      public void Map_SphereCalloutSOToSphereCallout()
      {
         // Arrange
         var sphereCalloutSO = new SphereCalloutSO()
         {
            Anchor = new AnchorSO() { LeaderLineOrigin = new Point3DSO() { X = 3.01f, Y = 3.02f, Z = 3.03f } },
            Position = new Point2DSO() { X = 4.01f, Y = 4.02f }
         };

         // Act
         var sphereCallout = Mapper.Map<SphereCalloutSO, SphereCallout>(sphereCalloutSO);

         // Assert
         Assert.IsInstanceOf(typeof(Anchor), sphereCallout.Anchor);
         Assert.IsInstanceOf(typeof(Point2D), sphereCallout.Position);
         Assert.AreEqual(sphereCalloutSO.Position.X, sphereCallout.Position.X);
         Assert.AreEqual(sphereCalloutSO.Position.Y, sphereCallout.Position.Y);
         Assert.AreEqual(sphereCalloutSO.Anchor.LeaderLineOrigin.X, sphereCallout.Anchor.LeaderLineOrigin.X);
         Assert.AreEqual(sphereCalloutSO.Anchor.LeaderLineOrigin.Y, sphereCallout.Anchor.LeaderLineOrigin.Y);
         Assert.AreEqual(sphereCalloutSO.Anchor.LeaderLineOrigin.Z, sphereCallout.Anchor.LeaderLineOrigin.Z);
      }

      [Test]
      public void Map_CADLayoutToCadLayoutSO()
      {
         // Arrange
         CADLayout cadLayout = new CADLayout(
            new Id<CADLayout>(Guid.NewGuid()),
            Guid.NewGuid(),
            new Camera(
               new Point3D(1.01f, 1.02f, 1.03f),
               new Point3D(2.01f, 2.02f, 2.03f),
               new Direction3D(1, -1, 1),
               0.01f,
               0.02f,
               true
            ),
            new List<Callout2D>(),
            new List<Callout3D>());

         cadLayout.Callouts2D.Add(new CircleCallout(new Anchor(new Point3D(3.01f, 3.02f, 3.03f)), new Point2D(4.01f, 4.02f)));
         cadLayout.Callouts2D.Add(new ConeCallout(new Anchor(new Point3D(3.01f, 3.02f, 3.03f)), new Point2D(4.01f, 4.02f)));
         cadLayout.Callouts2D.Add(new CylinderCallout(new Anchor(new Point3D(3.01f, 3.02f, 3.03f)), new Point2D(4.01f, 4.02f)));
         cadLayout.Callouts2D.Add(new LineCallout(new Anchor(new Point3D(3.01f, 3.02f, 3.03f)), new Point2D(4.01f, 4.02f)));
         cadLayout.Callouts2D.Add(new PlaneCallout(new Anchor(new Point3D(3.01f, 3.02f, 3.03f)), new Point2D(4.01f, 4.02f)));
         cadLayout.Callouts2D.Add(new PointCallout(new Anchor(new Point3D(3.01f, 3.02f, 3.03f)), new Point2D(4.01f, 4.02f)));
         cadLayout.Callouts2D.Add(new SphereCallout(new Anchor(new Point3D(3.01f, 3.02f, 3.03f)), new Point2D(4.01f, 4.02f)));
         cadLayout.Callouts3D.Add(new Callout3D(new Anchor(new Point3D(5.01f, 5.02f, 5.03f)), new Point3D(6.01f, 6.02f, 6.03f)));

         // Act
         var cadLayoutSO = Mapper.Map<CADLayout, CADLayoutSO>(cadLayout);

         // Assert Camera
         Assert.IsInstanceOf(typeof(CameraSO), cadLayoutSO.Camera);
         // Assert Callouts2D List
         Assert.IsInstanceOf(typeof(CircleCalloutSO), cadLayoutSO.Callouts2D[0]);
         Assert.IsInstanceOf(typeof(ConeCalloutSO), cadLayoutSO.Callouts2D[1]);
         Assert.IsInstanceOf(typeof(CylinderCalloutSO), cadLayoutSO.Callouts2D[2]);
         Assert.IsInstanceOf(typeof(LineCalloutSO), cadLayoutSO.Callouts2D[3]);
         Assert.IsInstanceOf(typeof(PlaneCalloutSO), cadLayoutSO.Callouts2D[4]);
         Assert.IsInstanceOf(typeof(PointCalloutSO), cadLayoutSO.Callouts2D[5]);
         Assert.IsInstanceOf(typeof(SphereCalloutSO), cadLayoutSO.Callouts2D[6]);
         // Assert Callouts3D List
         Assert.IsInstanceOf(typeof(Callout3DSO), cadLayoutSO.Callouts3D[0]);
      }
      [Test]
      public void Map_CADLayoutSOToCadLayout()
      {
         // Arrange
         CADLayoutSO cadLayoutSO = new CADLayoutSO() { Callouts2D = new List<Callout2DSO>(), Callouts3D = new List<Callout3DSO>() };
         cadLayoutSO.Camera = new CameraSO()
         {
            FieldHeight = 0.01f,
            FieldWidht = 0.02f,
            PerspectiveProjection = true,
            Position = new Point3DSO() { X = 1.01f, Y = 1.02f, Z = 1.03f },
            Target = new Point3DSO() { X = 2.01f, Y = 2.02f, Z = 2.03f }
         };
         cadLayoutSO.Callouts2D.Add(new CircleCalloutSO()
         {
            Anchor = new AnchorSO() { LeaderLineOrigin = new Point3DSO() { X = 3.01f, Y = 3.02f, Z = 3.03f } },
            Position = new Point2DSO() { X = 4.01f, Y = 4.02f }
         });
         cadLayoutSO.Callouts2D.Add(new ConeCalloutSO()
         {
            Anchor = new AnchorSO() { LeaderLineOrigin = new Point3DSO() { X = 3.01f, Y = 3.02f, Z = 3.03f } },
            Position = new Point2DSO() { X = 4.01f, Y = 4.02f }
         });
         cadLayoutSO.Callouts2D.Add(new CylinderCalloutSO()
         {
            Anchor = new AnchorSO() { LeaderLineOrigin = new Point3DSO() { X = 3.01f, Y = 3.02f, Z = 3.03f } },
            Position = new Point2DSO() { X = 4.01f, Y = 4.02f }
         });
         cadLayoutSO.Callouts2D.Add(new LineCalloutSO()
         {
            Anchor = new AnchorSO() { LeaderLineOrigin = new Point3DSO() { X = 3.01f, Y = 3.02f, Z = 3.03f } },
            Position = new Point2DSO() { X = 4.01f, Y = 4.02f }
         });
         cadLayoutSO.Callouts2D.Add(new PlaneCalloutSO()
         {
            Anchor = new AnchorSO() { LeaderLineOrigin = new Point3DSO() { X = 3.01f, Y = 3.02f, Z = 3.03f } },
            Position = new Point2DSO() { X = 4.01f, Y = 4.02f }
         });
         cadLayoutSO.Callouts2D.Add(new PointCalloutSO()
         {
            Anchor = new AnchorSO() { LeaderLineOrigin = new Point3DSO() { X = 3.01f, Y = 3.02f, Z = 3.03f } },
            Position = new Point2DSO() { X = 4.01f, Y = 4.02f }
         });
         cadLayoutSO.Callouts2D.Add(new SphereCalloutSO()
         {
            Anchor = new AnchorSO() { LeaderLineOrigin = new Point3DSO() { X = 3.01f, Y = 3.02f, Z = 3.03f } },
            Position = new Point2DSO() { X = 4.01f, Y = 4.02f }
         });
         cadLayoutSO.Callouts3D.Add(new Callout3DSO()
         {
            Anchor = new AnchorSO() { LeaderLineOrigin = new Point3DSO() { X = 5.01f, Y = 5.02f, Z = 5.03f } },
            Position = new Point3DSO() { X = 6.01f, Y = 6.02f, Z = 6.03f }
         });

         // Act
         var cadLayout = Mapper.Map<CADLayoutSO, CADLayout>(cadLayoutSO);

         // Assert Camera
         Assert.IsInstanceOf(typeof(Camera), cadLayout.Camera);
         // Assert Callouts2D List
         Assert.IsInstanceOf(typeof(CircleCallout), cadLayout.Callouts2D[0]);
         Assert.IsInstanceOf(typeof(ConeCallout), cadLayout.Callouts2D[1]);
         Assert.IsInstanceOf(typeof(CylinderCallout), cadLayout.Callouts2D[2]);
         Assert.IsInstanceOf(typeof(LineCallout), cadLayout.Callouts2D[3]);
         Assert.IsInstanceOf(typeof(PlaneCallout), cadLayout.Callouts2D[4]);
         Assert.IsInstanceOf(typeof(PointCallout), cadLayout.Callouts2D[5]);
         Assert.IsInstanceOf(typeof(SphereCallout), cadLayout.Callouts2D[6]);
         // Assert Callouts3D List
         Assert.IsInstanceOf(typeof(Callout3D), cadLayout.Callouts3D[0]);
      }
   }
}
