using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace EzyPlanData
{
    /// <summary>
    /// Cross-platform replacement for the original WinForms-bound
    /// <c>SortableBindingList&lt;T&gt;</c> (which derived from <c>BindingList&lt;T&gt;</c>
    /// and implemented <c>ITypedList</c> for DataGridView binding).
    /// Same API surface used by the ported code (Add/Count/indexer,
    /// sorted-by-names constructor, <c>ForceChangeEvent</c>); sorting is
    /// expressed with LINQ over property names instead of
    /// <c>PropertyDescriptorCollection</c>.
    /// </summary>
    public class SortableBindingList<T> : ObservableCollection<T>
    {
        private readonly string[] _sortNames;

        public SortableBindingList()
        {
        }

        public SortableBindingList(string[] names)
            : this()
        {
            _sortNames = names;
            SortProperties(names);
        }

        protected void SortProperties(string[] names)
        {
            if (names == null || names.Length == 0 || Count <= 1)
            {
                return;
            }
            List<T> sorted = new List<T>(this);
            PropertyDescriptorCollection props = TypeDescriptor.GetProperties(typeof(T));
            sorted.Sort(delegate (T a, T b)
            {
                foreach (string name in names)
                {
                    PropertyDescriptor p = props.Find(name, ignoreCase: true);
                    if (p == null)
                    {
                        continue;
                    }
                    object va = p.GetValue(a);
                    object vb = p.GetValue(b);
                    int c = Comparer<object>.Default.Compare(va, vb);
                    if (c != 0)
                    {
                        return c;
                    }
                }
                return 0;
            });
            for (int i = 0; i < sorted.Count; i++)
            {
                Move(IndexOf(sorted[i]), i);
            }
        }

        /// <summary>Notifies listeners that the whole list changed (replaces BindingList Reset event).</summary>
        public void ForceChangeEvent()
        {
            OnCollectionChanged(new System.Collections.Specialized.NotifyCollectionChangedEventArgs(
                System.Collections.Specialized.NotifyCollectionChangedAction.Reset));
        }

        public string[] SortNames => _sortNames;
    }
}
