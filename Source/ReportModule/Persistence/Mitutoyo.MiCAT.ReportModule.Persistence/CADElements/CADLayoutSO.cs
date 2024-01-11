// <copyright file="CADLayoutSO.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using Mitutoyo.MiCAT.ReportModule.Persistence.CADElements.Callouts2D;
using Mitutoyo.MiCAT.ReportModule.Persistence.CADElements.Callouts2D.FeatureCallouts;
using Mitutoyo.MiCAT.ReportModule.Persistence.CADElements.Callouts3D;

namespace Mitutoyo.MiCAT.ReportModule.Persistence.CADElements
{
   public class CADLayoutSO
   {
      public Guid Id { get; set; }
      public Guid PlanID { get; set; }
      public CameraSO Camera { get; set; }

      [XmlArray("Callouts2D")]
      [XmlArrayItem("Circle", typeof(CircleCalloutSO))]
      [XmlArrayItem("Cone", typeof(ConeCalloutSO))]
      [XmlArrayItem("Cylinder", typeof(CylinderCalloutSO))]
      [XmlArrayItem("Line", typeof(LineCalloutSO))]
      [XmlArrayItem("Plane", typeof(PlaneCalloutSO))]
      [XmlArrayItem("Point", typeof(PointCalloutSO))]
      [XmlArrayItem("Sphere", typeof(SphereCalloutSO))]
      public List<Callout2DSO> Callouts2D { get; set; }

      [XmlArray("Callouts3D")]
      [XmlArrayItem("Flatness", typeof(Callout3DSO))]
      public List<Callout3DSO> Callouts3D { get; set; }
   }
}
