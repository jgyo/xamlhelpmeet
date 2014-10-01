using System;

using XamlHelpmeet.Extensions;
using XamlHelpmeet.Model;

namespace XamlHelpmeet.ReflectionLoader
{
using NLog;

using YoderZone.Extensions.NLog;

/** =========================================================================
    <summary>
    Remote response.
    </summary>
    <remarks>
    This is used to carry a response from one application domain to another
    in order to reflect the properites of an assembly.
    </remarks>
    <typeparam name="T"> Generic type parameter.</typeparam>
    -----------------------------------------------------------------------**/

[Serializable]
public class RemoteResponse<T>
{
    private static readonly Logger logger =
        SettingsHelper.CreateLogger();

    private readonly ResponseStatus _responseStatus = ResponseStatus.Success;
    private readonly Exception _exception;
    private readonly T _result;
    private readonly string _customMessage = string.Empty;

    /// <summary>
    ///     Initializes a new instance of the RemoteResponse class.
    /// </summary>
    /// <param name="Result">
    ///     The result.
    /// </param>
    /// <param name="Status">
    ///     The status.
    /// </param>
    /// <param name="Ex">
    ///     The exception.
    /// </param>
    /// <param name="CustomMessage">
    ///     A custom message describing the response.
    /// </param>

    public RemoteResponse(T Result, ResponseStatus Status, Exception Ex,
                          string CustomMessage)
    {
        _result = Result;
        _responseStatus = Status;
        _exception = Ex;
        _customMessage = CustomMessage;
    }

    public RemoteResponse(T Result, ResponseStatus Status,
                          string CustomMessage)
    {
        _result = Result;
        _responseStatus = Status;
        _customMessage = CustomMessage;
    }

    /// <summary>
    ///     Gets a custom message describing the response.
    /// </summary>
    /// <value>
    ///     A message describing the custom.
    /// </value>

    public string CustomMessage
    {
        get
        {
            return _customMessage;
        }
    }

    public string CustomMessageAndException
    {
        get
        {
            var msg = string.Empty;

            if (Exception != null)
            {
                msg = Exception.Message;
            }

            if (CustomMessage.IsNullOrEmpty())
            {
                return msg;
            }

            return string.Concat(CustomMessage, msg.IsNullOrEmpty() ?
                                 string.Empty :
                                 string.Format("Message : {0}", msg));
        }
    }
    /// <summary>
    ///     Gets the result.
    /// </summary>
    /// <value>
    ///     The result.
    /// </value>

    public T Result
    {
        get
        {
            return _result;
        }
    }

    /// <summary>
    ///     Gets the exception.
    /// </summary>
    /// <value>
    ///     The exception.
    /// </value>

    public Exception Exception
    {
        get
        {
            return _exception;
        }
    }

    /// <summary>
    ///     Gets the response status.
    /// </summary>
    /// <value>
    ///     The response status.
    /// </value>

    public ResponseStatus ResponseStatus
    {
        get
        {
            return _responseStatus;
        }
    }


}
}
