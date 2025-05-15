using System.Collections.Generic;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Systolic.UI.Models;

public partial class ObservableKeyValuePair<TKey, TValue> : ObservableObject
{
	[ObservableProperty] private TKey _key;

	[ObservableProperty] private TValue _value;

	public ObservableKeyValuePair(TKey key, TValue value)
	{
		Key = key;
		Value = value;
	}

	public static implicit operator KeyValuePair<TKey, TValue>(
		ObservableKeyValuePair<TKey, TValue> observableKeyValuePair)
	{
		return new KeyValuePair<TKey, TValue>(observableKeyValuePair.Key, observableKeyValuePair.Value);
	}

	public static implicit operator ObservableKeyValuePair<TKey, TValue>(KeyValuePair<TKey, TValue> keyValuePair)
	{
		return new ObservableKeyValuePair<TKey, TValue>(keyValuePair.Key, keyValuePair.Value);
	}
}