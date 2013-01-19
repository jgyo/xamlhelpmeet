using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization;

namespace XamlHelpmeet.UI.Utilities
{
	/// <summary>
	/// 	Class ObservableCollectionSerializationSurrogate to help serializing
	/// 	ObservableCollection&lt;&amp;gt instances.
	/// </summary>
	/// <seealso cref="T:System.Runtime.Serialization.ISerializationSurrogate"/>
	public sealed class ObservableCollectionSerializationSurrogate <T>
		: ISerializationSurrogate
	{
		private const string _itemsKey = "items";

		/// <summary>
		/// 	Populates the provided
		/// 	<see cref="T:System.Runtime.Serialization.SerializationInfo" /> with the data
		/// 	needed to serialize the ObservableCollection.
		/// </summary>
		/// <seealso cref="M:System.Runtime.Serialization.ISerializationSurrogate.GetObjectData(object,SerializationInfo,StreamingContext)"/>
		public void GetObjectData(object obj, SerializationInfo info, StreamingContext context)
		{
			if (!(obj is ObservableCollection<T>))
			{
				throw new ArgumentException("Unexpected object Type encountered. Should be ObservableCollection<T>.", "obj");
			}
			var oc = obj as ObservableCollection<T>;
			var items = new T[oc.Count];
			oc.CopyTo(items, 0);
			info.AddValue(_itemsKey, items);
		}

		/// <summary>
		/// 	Populates the ObservableCollection using the information in the
		/// 	<see cref="T:System.Runtime.Serialization.SerializationInfo" />.
		/// </summary>
		/// <seealso cref="M:System.Runtime.Serialization.ISerializationSurrogate.SetObjectData(object,SerializationInfo,StreamingContext,ISurrogateSelector)"/>
		public object SetObjectData(object obj, SerializationInfo info, StreamingContext context, ISurrogateSelector selector)
		{
			var items = info.GetValue(_itemsKey, typeof(T[])) as T[];
			return new ObservableCollection<T>(new List<T>(items));
		}
	}
}
