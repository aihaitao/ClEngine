using System;
using System.ComponentModel;
using Microsoft.Xna.Framework;
using MonoGame.Extended;

namespace ClEngine.Particle.Core
{
	[Flags]
	internal enum TransformFlags : byte
	{
		WorldMatrixIsDirty = 1 << 0,
		LocalMatrixIsDirty = 1 << 1,
		All = WorldMatrixIsDirty | LocalMatrixIsDirty
	}

	/// <summary>
	///     Represents the base class for the position, rotation, and scale of a game object in two-dimensions or
	///     three-dimensions.
	/// </summary>
	/// <typeparam name="TMatrix">The type of the matrix.</typeparam>
	/// <remarks>
	///     <para>
	///         Every game object has a transform which is used to store and manipulate the position, rotation and scale
	///         of the object. Every transform can have a parent, which allows to apply position, rotation and scale to game
	///         objects hierarchically.
	///     </para>
	///     <para>
	///         This class shouldn't be used directly. Instead use either of the derived classes; <see cref="Transform2D" /> or
	///         Transform3D.
	///     </para>
	/// </remarks>
	[EditorBrowsable(EditorBrowsableState.Never)]
	public abstract class BaseTransform<TMatrix>
		where TMatrix : struct
	{
		private TransformFlags _flags = TransformFlags.All; // dirty flags, set all dirty flags when created
		private TMatrix _localMatrix; // model space to local space
		private BaseTransform<TMatrix> _parent; // parent
		private TMatrix _worldMatrix; // local space to world space

		// internal contructor because people should not be using this class directly; they should use Transform2D or Transform3D
		internal BaseTransform()
		{
		}

		/// <summary>
		///     Gets the model-to-local space <see cref="Matrix2D" />.
		/// </summary>
		/// <value>
		///     The model-to-local space <see cref="Matrix2D" />.
		/// </value>
		[DisplayName("模型到当前空间"), Description("获取模型到当前空间"), Category("Transform")]
		public TMatrix LocalMatrix
		{
			get
			{
				RecalculateLocalMatrixIfNecessary(); // attempt to update local matrix upon request if it is dirty
				return _localMatrix;
			}
		}

		/// <summary>
		///     Gets the local-to-world space <see cref="Matrix2D" />.
		/// </summary>
		/// <value>
		///     The local-to-world space <see cref="Matrix2D" />.
		/// </value>
		[DisplayName("当前到世界的空间"), Description("获取当前到世界的空间"), Category("Transform")]
		public TMatrix WorldMatrix
		{
			get
			{
				RecalculateWorldMatrixIfNecessary(); // attempt to update world matrix upon request if it is dirty
				return _worldMatrix;
			}
		}

		/// <summary>
		///     Gets or sets the parent instance.
		/// </summary>
		/// <value>
		///     The parent instance.
		/// </value>
		/// <remarks>
		///     <para>
		///         Setting <see cref="Parent" /> to a non-null instance enables this instance to
		///         inherit the position, rotation, and scale of the parent instance. Setting <see cref="Parent" /> to
		///         <code>null</code> disables the inheritance altogether for this instance.
		///     </para>
		/// </remarks>
		[EditorBrowsable(EditorBrowsableState.Never), DisplayName("父实例"), Description("获取或设置父实例"), Category("Transform")]
		public BaseTransform<TMatrix> Parent
		{
			get { return _parent; }
			set
			{
				if (_parent == value)
					return;

				var oldParentTransform = Parent;
				_parent = value;
				OnParentChanged(oldParentTransform, value);
			}
		}

		public event Action TransformBecameDirty; // observer pattern for when the world (or local) matrix became dirty
		public event Action TranformUpdated; // observer pattern for after the world (or local) matrix was re-calculated

		/// <summary>
		///     Gets the model-to-local space <see cref="Matrix2D" />.
		/// </summary>
		/// <param name="matrix">The model-to-local space <see cref="Matrix2D" />.</param>
		public void GetLocalMatrix(out TMatrix matrix)
		{
			RecalculateLocalMatrixIfNecessary();
			matrix = _localMatrix;
		}

		/// <summary>
		///     Gets the local-to-world space <see cref="Matrix2D" />.
		/// </summary>
		/// <param name="matrix">The local-to-world space <see cref="Matrix2D" />.</param>
		public void GetWorldMatrix(out TMatrix matrix)
		{
			RecalculateWorldMatrixIfNecessary();
			matrix = _worldMatrix;
		}

		protected internal void LocalMatrixBecameDirty()
		{
			_flags |= TransformFlags.LocalMatrixIsDirty;
		}

		protected internal void WorldMatrixBecameDirty()
		{
			_flags |= TransformFlags.WorldMatrixIsDirty;
			TransformBecameDirty?.Invoke();
		}

		private void OnParentChanged(BaseTransform<TMatrix> oldParent, BaseTransform<TMatrix> newParent)
		{
			var parent = oldParent;
			while (parent != null)
			{
				parent.TransformBecameDirty -= ParentOnTransformBecameDirty;
				parent = parent.Parent;
			}

			parent = newParent;
			while (parent != null)
			{
				parent.TransformBecameDirty += ParentOnTransformBecameDirty;
				parent = parent.Parent;
			}
		}

		private void ParentOnTransformBecameDirty()
		{
			_flags |= TransformFlags.All;
		}

		private void RecalculateWorldMatrixIfNecessary()
		{
			if ((_flags & TransformFlags.WorldMatrixIsDirty) == 0)
				return;

			RecalculateLocalMatrixIfNecessary();
			RecalculateWorldMatrix(ref _localMatrix, out _worldMatrix);

			_flags &= ~TransformFlags.WorldMatrixIsDirty;
			TranformUpdated?.Invoke();
		}

		protected internal abstract void RecalculateWorldMatrix(ref TMatrix localMatrix, out TMatrix matrix);

		private void RecalculateLocalMatrixIfNecessary()
		{
			if ((_flags & TransformFlags.LocalMatrixIsDirty) == 0)
				return;

			RecalculateLocalMatrix(out _localMatrix);

			_flags &= ~TransformFlags.LocalMatrixIsDirty;
			WorldMatrixBecameDirty();
		}

		protected internal abstract void RecalculateLocalMatrix(out TMatrix matrix);
	}

	public class Transform2D : BaseTransform<Matrix2D>, IMovable, IRotatable, IScalable
	{
		private Vector2 _position;
		private float _rotation;
		private Vector2 _scale = Vector2.One;

		/// <summary>
		///     Gets the world position.
		/// </summary>
		/// <value>
		///     The world position.
		/// </value>
		[DisplayName("世界的位置"), Description("获得世界的位置"), Category("Transform")]
		public Vector2 WorldPosition => WorldMatrix.Translation;

		/// <summary>
		///     Gets the world scale.
		/// </summary>
		/// <value>
		///     The world scale.
		/// </value>
		[DisplayName("世界的比例"), Description("获得世界的比例"), Category("Transform")]
		public Vector2 WorldScale => WorldMatrix.Scale;

		/// <summary>
		///     Gets the world rotation angle in radians.
		/// </summary>
		/// <value>
		///     The world rotation angle in radians.
		/// </value>
		[DisplayName("以弧度表示的世界旋转角度"), Description("以弧度获取世界旋转角度"), Category("Transform")]
		public float WorldRotation => WorldMatrix.Rotation;

		/// <summary>
		///     Gets or sets the local position.
		/// </summary>
		/// <value>
		///     The local position.
		/// </value>
		[DisplayName("位置"),Description("获取或设置位置"),Category("Transform")]
		public Vector2 Position
		{
			get { return _position; }
			set
			{
				_position = value;
				LocalMatrixBecameDirty();
				WorldMatrixBecameDirty();
			}
		}

		/// <summary>
		///     Gets or sets the local rotation angle in radians.
		/// </summary>
		/// <value>
		///     The local rotation angle in radians.
		/// </value>
		[DisplayName("以弧度表示的旋转角度"), Description("获取或设置以弧度表示的旋转角度"), Category("Transform")]
		public float Rotation
		{
			get { return _rotation; }
			set
			{
				_rotation = value;
				LocalMatrixBecameDirty();
				WorldMatrixBecameDirty();
			}
		}

		/// <summary>
		///     Gets or sets the local scale.
		/// </summary>
		/// <value>
		///     The local scale.
		/// </value>
		[DisplayName("比例"), Description("获取或设置比例"), Category("Transform")]
		public Vector2 Scale
		{
			get { return _scale; }
			set
			{
				_scale = value;
				LocalMatrixBecameDirty();
				WorldMatrixBecameDirty();
			}
		}

		protected internal override void RecalculateWorldMatrix(ref Matrix2D localMatrix, out Matrix2D matrix)
		{
			if (Parent != null)
			{
				Parent.GetWorldMatrix(out matrix);
				Matrix2D.Multiply(ref localMatrix, ref matrix, out matrix);
			}
			else
			{
				matrix = localMatrix;
			}
		}

		protected internal override void RecalculateLocalMatrix(out Matrix2D matrix)
		{
			matrix = Matrix2D.CreateScale(_scale) *
					 Matrix2D.CreateRotationZ(_rotation) *
					 Matrix2D.CreateTranslation(_position);
		}
	}
}