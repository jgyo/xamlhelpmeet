// file:    UIControlFactory\SerializableObservableCollection.cs
//
// summary: Implements the SerializableObservableCollection class
using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;

namespace XamlHelpmeet.UI.UIControlFactory
{
using NLog;

using YoderZone.Extensions.NLog;

/*
    I added this class in order to implement serialization on the ControlProperties
    property in UIControl since I could not successfully translate Shifflett's code
    into C#. This is necessary because ObservableCollection cannot be serialized
    directly. This web page explains:

    http://kentb.blogspot.com/2007/11/serializing-observablecollection.html

    I opted to go with the alternative solution because the original solution
    was not able to retreive the contents of the collection, although it would
    create the collection with null members.
*/

/// <summary>
///     An ObservableCollection that may be serialized.
/// </summary>
/// <seealso cref="T:System.Collections.ObjectModel.ObservableCollection{T}"/>
/// <seealso cref="T:System.ComponentModel.INotifyPropertyChanged"/>
[Serializable]
public class SerializableObservableCollection<T> :
    ObservableCollection<T>, INotifyPropertyChanged
{
    private static readonly Logger logger =
        SettingsHelper.CreateLogger();

    [field: NonSerialized]
    private PropertyChangedEventHandler _propertyChangedEventHandler;

    /// <summary>
    ///     Occurs when the collection changes.
    /// </summary>
    [field: NonSerialized]
    public override event NotifyCollectionChangedEventHandler
    CollectionChanged;

    event PropertyChangedEventHandler INotifyPropertyChanged.PropertyChanged
    {
        add
        {
            _propertyChangedEventHandler = Delegate.Combine(
                                               _propertyChangedEventHandler, value) as PropertyChangedEventHandler;
        }
        remove
        {
            _propertyChangedEventHandler = Delegate.Remove(
                                               _propertyChangedEventHandler, value) as PropertyChangedEventHandler;
        }
    }

    /// <summary>
    ///     Raises the collection changed event.
    /// </summary>
    /// <param name="e">
    ///     Event information to send to registered event handlers.
    /// </param>
    protected override void OnCollectionChanged(
        NotifyCollectionChangedEventArgs e)
    {
        var h = CollectionChanged;
        if (h == null)
        { return; }
        h(this, e);
    }

    /// <summary>
    ///     Raises the property changed event.
    /// </summary>
    /// <param name="e">
    ///     Event information to send to registered event handlers.
    /// </param>
    protected override void OnPropertyChanged(PropertyChangedEventArgs e)
    {
        var h = _propertyChangedEventHandler;
        if (h == null)
        { return; }
        h(this, e);
    }
}
}