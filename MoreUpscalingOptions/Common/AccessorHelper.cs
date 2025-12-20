using System.Linq.Expressions;

namespace Mugnum.HasteMods.MoreUpscalingOptions.Common
{
	/// <summary>
	/// Field accessor helper.
	/// </summary>
    public static class AccessorHelper
    {
	    /// <summary>
	    /// Creates private field getter from an instance.
	    /// </summary>
	    /// <typeparam name="TInstanceType"> Instance type. </typeparam>
	    /// <typeparam name="TFieldType"> Field type. </typeparam>
	    /// <param name="fieldName"> Field name. </param>
	    /// <returns> Field getter delegate. </returns>
	    public static Func<TInstanceType, TFieldType> CreateFieldGetter<TInstanceType, TFieldType>(string fieldName)
		{
			var instance = Expression.Parameter(typeof(TInstanceType), "instance");
		    var field = Expression.Field(instance, fieldName);
		    return Expression.Lambda<Func<TInstanceType, TFieldType>>(field, instance).Compile();
	    }
	}
}
