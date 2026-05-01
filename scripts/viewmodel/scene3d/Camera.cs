using System;
using Godot;
using Vector2 = Godot.Vector2;
using Vector3 = Godot.Vector3;

namespace ProceduralFoliageGenerator.ViewModel;

public partial class Camera : Camera3D
{
    /// <summary>
    ///     Velocity vector describing the movement direction and magnitude of the camera.
    /// </summary>
    public Vector3 Velocity { get; set; } = Vector3.Zero;

    /// <summary>
    ///     Constant modifier of the translation speed of the camera.
    /// </summary>
    public float SpeedModifier { get; set; } = 3;


    /// <summary>
    ///     Polar coordinates describing the look direction of the camera in global space.
    /// </summary>
    public Vector2 Uv { get; set; } = new(float.Pi / 2, -float.Pi / 2);

    /// <summary>
    ///     Relative changes made to the look direction of the camera since last input.
    /// </summary>
    public Vector2 DeltaUv { get; set; } = new(0, 0);

    /// <summary>
    ///     Cartesian coordinates describing the look direction of the camera in local space.
    /// </summary>
    public Vector3 LookDirection { get; set; }

    /// <summary>
    ///     Translates the position of the camera based on the velocity vector.
    /// </summary>
    /// <param name="velocity"></param>
    public void TranslateCamera(Vector3 velocity)
    {
        var globalTranslation = velocity.Y * Vector3.Up -
                                new Vector3(LookDirection.X, 0, LookDirection.Z).Normalized() * velocity.Z +
                                new Vector3(LookDirection.X, 0, LookDirection.Z).Normalized().Cross(Vector3.Up) *
                                velocity.X;

        GlobalTranslate(globalTranslation);
    }

    public override void _Process(double delta)
    {
        if (Velocity != Vector3.Zero) TranslateCamera(Velocity * (float)delta * SpeedModifier);

        if (DeltaUv != Vector2.Zero)
        {
            var x = Uv.X;
            var y = Uv.Y;
            x = x + DeltaUv.X * (float)delta;
            y = Math.Clamp(y + DeltaUv.Y * (float)delta, 0.3f, 3.11f);

            Uv = new Vector2(x, y);

            LookDirection = new Vector3(
                (float)(Math.Cos(Uv.X) * Math.Sin(Uv.Y)),
                (float)Math.Cos(Uv.Y),
                (float)(Math.Sin(Uv.X) * Math.Sin(Uv.Y))
            );
            LookDirection = LookDirection.Normalized();

            LookAt(Position + LookDirection);

            DeltaUv = Vector2.Zero;
        }

        base._Process(delta);
    }

    public override void _Input(InputEvent @event)
    {
        DeltaUv = Vector2.Zero;
        if (@event is InputEventMouseMotion mouseEvent && Input.IsActionPressed("Move Camera"))
            DeltaUv += mouseEvent.Relative;

        if (@event is InputEventKey)
        {
            Velocity = Vector3.Zero;
            if (Input.IsActionPressed("Forward")) Velocity += Vector3.Forward;

            if (Input.IsActionPressed("Backward")) Velocity += Vector3.Back;

            if (Input.IsActionPressed("Left")) Velocity += Vector3.Left;

            if (Input.IsActionPressed("Right")) Velocity += Vector3.Right;

            if (Input.IsActionPressed("Ascend")) Velocity += Vector3.Up;

            if (Input.IsActionPressed("Descend")) Velocity += Vector3.Down;
        }
    }
}