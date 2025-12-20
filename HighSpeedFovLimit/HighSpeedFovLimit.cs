using Landfall.Modding;
using Mugnum.HasteMods.HighSpeedFovLimit.Settings;
using System.Linq.Expressions;
using System.Reflection;

namespace Mugnum.HasteMods.HighSpeedFovLimit;

/// <summary>
/// High speed FOV limit mod.
/// </summary>
[LandfallPlugin]
public class HighSpeedFovLimit
{
	/// <summary>
	/// Max fov value.
	/// </summary>
	public static float MaxFov = MaxFovSetting.DefaultValue;

	/// <summary>
	/// "cam" private field getter from <see cref="CameraMovement"/> class.
	/// </summary>
	private static readonly Func<CameraMovement, MainCamera> CamGetter = CreateFieldGetter<CameraMovement, MainCamera>("cam");

	/// <summary>
	/// Constructor.
	/// </summary>
	static HighSpeedFovLimit()
	{
		On.CameraMovement.Update += OnCameraMovementUpdate;
	}

	/// <summary>
	/// Handler for <see cref="CameraMovement.Update"/> method.
	/// </summary>
	/// <param name="orig"> Original method. </param>
	/// <param name="self"> Instance. </param>
	private static void OnCameraMovementUpdate(On.CameraMovement.orig_Update orig, CameraMovement self)
	{
		orig(self);
		var cam = CamGetter(self);
		cam.cam.fieldOfView = Math.Min(cam.cam.fieldOfView, MaxFov);
	}

	/// <summary>
	/// Creates private field getter from an instance.
	/// </summary>
	/// <typeparam name="TInstanceType"> Instance type. </typeparam>
	/// <typeparam name="TFieldType"> Field type. </typeparam>
	/// <param name="fieldName"> Field name. </param>
	/// <returns> Field getter delegate. </returns>
	internal static Func<TInstanceType, TFieldType> CreateFieldGetter<TInstanceType, TFieldType>(string fieldName)
	{
		var fieldInfo = typeof(TInstanceType).GetField(fieldName, BindingFlags.Instance | BindingFlags.NonPublic)
			?? throw new MissingFieldException(typeof(TInstanceType).FullName, fieldName);

		var instance = Expression.Parameter(typeof(TInstanceType), "instance");
		var field = Expression.Field(instance, fieldInfo);
		return Expression.Lambda<Func<TInstanceType, TFieldType>>(field, instance).Compile();
	}
}
