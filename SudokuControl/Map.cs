using System;
using System.Collections.Generic;

namespace Sudoku {
	public class Map<T1, T2> {
		internal readonly Dictionary<T1, T2> _forward = new();
		internal readonly Dictionary<T2, T1> _reverse = new();

		public Map() {
			Forward = new Indexer<T1, T2>(_forward);
			Reverse = new Indexer<T2, T1>(_reverse);
		}

		public class Indexer<T3, T4> {
			internal readonly Dictionary<T3, T4> _dictionary;
			public Indexer(Dictionary<T3, T4> dictionary) {
				_dictionary = dictionary;
			}
			public T4 this[T3 index] {
				get => _dictionary[index];
				set => _dictionary[index] = value;
			}
			public Boolean TryGetValue(T3 key, out T4 value) {
				return _dictionary.TryGetValue(key, out value);
			}
			public Boolean ContainsKey(T3 key) {
				return _dictionary.ContainsKey(key);
			}
		}

		public void Add(T1 t1, T2 t2) {
			_forward.Add(t1, t2);
			_reverse.Add(t2, t1);
		}

		public Indexer<T1, T2> Forward { get; internal set; }
		public Indexer<T2, T1> Reverse { get; internal set; }
	}
}