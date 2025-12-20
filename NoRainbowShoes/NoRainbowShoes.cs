using Landfall.Modding;
using System.Linq.Expressions;
using System.Reflection;
using UnityEngine;

namespace Mugnum.HasteMods.NoRainbowShoes;

/// <summary>
/// No rainbow shoes mod.
/// </summary>
[LandfallPlugin]
public class NoRainbowShoes
{
	/// <summary>
	/// "player" private field getter from <see cref="VFX_BoostShoes"/> class.
	/// </summary>
	private static readonly Func<VFX_BoostShoes, PlayerCharacter> PlayerGetter;

	/// <summary>
	/// "gradient" private field getter from <see cref="VFX_BoostShoes"/> class.
	/// </summary>
	private static readonly Func<VFX_BoostShoes, Gradient> GradientGetter;

	/// <summary>
	/// "rend" private field getter from <see cref="VFX_BoostShoes"/> class.
	/// </summary>
	private static readonly Func<VFX_BoostShoes, MeshRenderer> RendGetter;

	/// <summary>
	/// Constructor.
	/// </summary>
	static NoRainbowShoes()
	{
		PlayerGetter = CreateFieldGetter<VFX_BoostShoes, PlayerCharacter>("player");
		GradientGetter = CreateFieldGetter<VFX_BoostShoes, Gradient>("gradient");
		RendGetter = CreateFieldGetter<VFX_BoostShoes, MeshRenderer>("rend");
		On.VFX_BoostShoes.Update += OnBoostShoesUpdate;
	}

	/// <summary>
	/// Handler for <see cref="VFX_BoostShoes.Update"/> method.
	/// </summary>
	/// <param name="orig"> Original method. </param>
	/// <param name="self"> Instance. </param>
	private static void OnBoostShoesUpdate(On.VFX_BoostShoes.orig_Update orig, VFX_BoostShoes self)
	{
		const float FullBright = 1f;
		const string MaterialColorName = "_Color2";

		orig(self);
		var boost = Math.Min(PlayerGetter(self).data.GetBoost(), FullBright);
		var color = GradientGetter(self).Evaluate(boost);
		RendGetter(self).material.SetColor(MaterialColorName, color);
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
