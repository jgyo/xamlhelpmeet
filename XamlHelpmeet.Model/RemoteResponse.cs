﻿using System;
using System.Collections.Generic;
using System.Linq;
using XamlHelpmeet;
using XamlHelpmeet.Extentions;

namespace XamlHelpmeet.Model
{
	public class RemoteResponse<T>
	{
		private readonly ResponseStatus _responseStatus = ResponseStatus.Success;
		private readonly Exception _exception;
		private readonly T _result;
		private readonly string _customMessage = string.Empty;

		/// <summary>
		/// 	Initializes a new instance of the RemoteResponse class.
		/// </summary>
		/// <param name="Result">
		/// 	The result.
		/// </param>
		/// <param name="Status">
		/// 	The status.
		/// </param>
		/// <param name="Ex">
		/// 	The exception.
		/// </param>
		/// <param name="customMessage">
		/// 	A custom message describing the response.
		/// </param>

		public RemoteResponse(T Result, ResponseStatus Status, Exception Ex, string customMessage)
		{
			_result = Result;
			_responseStatus = Status;
			_exception = Ex;
			_customMessage = customMessage;
		}

		/// <summary>
		/// 	Gets a custom message describing the response.
		/// </summary>
		/// <value>
		/// 	A message describing the custom.
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
		/// 	Gets the result.
		/// </summary>
		/// <value>
		/// 	The result.
		/// </value>

		public T Result
		{
			get
			{
				return _result;
			}
		}

		/// <summary>
		/// 	Gets the exception.
		/// </summary>
		/// <value>
		/// 	The exception.
		/// </value>

		public Exception Exception
		{
			get
			{
				return _exception;
			}
		}

		/// <summary>
		/// 	Gets the response status.
		/// </summary>
		/// <value>
		/// 	The response status.
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
