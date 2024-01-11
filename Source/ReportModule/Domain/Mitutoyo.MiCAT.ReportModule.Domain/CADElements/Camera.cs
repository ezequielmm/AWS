// <copyright file="Camera.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using Mitutoyo.MiCAT.Core.Geometry;

namespace Mitutoyo.MiCAT.ReportModule.Domain.CADElements
{
   public class Camera
   {
      public Point3D Position { get; }
      public Point3D Target { get; }
      public Direction3D UpVector { get; }
      public float FieldHeight { get; }
      public float FieldWidht { get; }

      /// <summary>
      /// If true, the Camera wil perform a Perspective Projection, otherwise will perform a Orthographic Projection
      /// </summary>
      public bool PerspectiveProjection { get; }

      public Camera() : this(Point3D.Ones, Point3D.Ones, new Direction3D(1, -1, 1), 0f, 0f, false)
      {
      }

      public Camera(Point3D position, Point3D target, Direction3D upVector, float fieldHeight, float fieldWidht, bool perspectiveProjection)
      {
         Position = position;
         Target = target;
         UpVector = upVector;
         FieldHeight = fieldHeight;
         FieldWidht = fieldWidht;
         PerspectiveProjection = perspectiveProjection;
      }
   }
}
