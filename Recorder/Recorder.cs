using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace YoderTools
{
	public class Recorder
	{
		private static Recorder _instance;

		private bool _disable = false;

		private bool _displayTitle = true;

		private Encoding _encoding = Encoding.UTF8;

		private string _filename = "Recorder.log";

		private string _lineEnd = string.Empty;

		private string _lineStart = string.Empty;

		private string _newLine = Environment.NewLine;

		private string _path = Environment.CurrentDirectory;

		private string _title1 = @"YoderTools Recorder v. 1.0{0}";

		private string _title2 = @"=========================={0}";

		private string _title3 = @"{0:yyyy, MM, dd: HH:mm:ss.fff}{1}";

		/// <summary>
		/// 	Prevents a default instance of the Recorder
		/// 	class from being created.
		/// </summary>
		private Recorder()
		{
			// This object should be access through
			// the static Instance property.
		}

		/// <summary>
		/// 	Gets the singleton Recorder instance.
		/// </summary>
		/// <value>
		/// 	The instance.
		/// </value>
		public static Recorder Instance
		{
			get
			{
				if (_instance == null)
					_instance = new Recorder();
				return _instance;
			}
		}

		/// <summary>
		/// 	Gets the full path to the log file.
		/// </summary>
		/// <returns>
		/// 	The full path.
		/// </returns>
		public string GetFullPath()
		{
			return LogFilePath();
		}

		/// <summary>
		/// 	Sets the encoding of the log file.
		/// </summary>
		/// <param name="encoding">
		/// 	The encoding.
		/// </param>
		public void SetEncoding(Encoding encoding)
		{
			_encoding = encoding;
		}

		/// <summary>
		/// 	Sets the filename of the log file to save.
		/// </summary>
		/// <exception cref="ArgumentException">
		/// 	Thrown when one or more arguments have unsupported or illegal values.
		/// </exception>
		/// <param name="filename">
		/// 	Filename of the file.
		/// </param>
		public void SetFilename(string filename)
		{
			if (_disable)
				return;

			foreach (var ch in Path.GetInvalidFileNameChars())
			{
				if (filename.Contains(ch))
					throw new ArgumentException("Illegal character found in arguement.", "filename");
			}
			_filename = filename;
		}

		/// <summary>
		/// 	Sets the line end text. This value will be
		/// 	included on each line after the data text
		/// 	and before the new line characters.
		/// </summary>
		/// <param name="lineEnd">
		/// 	The line end.
		/// </param>
		public void SetLineEnd(string lineEnd)
		{
			_lineEnd = lineEnd;
		}

		/// <summary>
		/// 	Sets line start characters. This value will
		/// 	be printed before data characters on every
		/// 	line. Include "{0}" in the characters to have
		/// 	the current date/time printed.
		/// </summary>
		/// <param name="lineStart">
		/// 	The line start.
		/// </param>
		public void SetLineStart(string lineStart)
		{
			_lineStart = lineStart;
		}

		/// <summary>
		/// 	Sets the new line characters. The default is
		/// 	Environment.NewLine. These characters are used
		/// 	at the end of lines to cause the next line to
		/// 	be printed on the next line. It is recommended
		/// 	that this value not be changed.
		/// </summary>
		/// <param name="newLine">
		/// 	The new line.
		/// </param>
		public void SetNewLine(string newLine)
		{
			_newLine = newLine;
		}

		/// <summary>
		/// 	Sets the directory path where the log file will be saved.
		/// </summary>
		/// <exception cref="ArgumentException">
		/// 	Thrown when one or more arguments have unsupported or illegal values.
		/// </exception>
		/// <param name="path">
		/// 	Full pathname of the directory.
		/// </param>
		public void SetPath(string path)
		{
			if (_disable)
				return;

			foreach (var ch in Path.GetInvalidPathChars())
			{
				if (path.Contains(ch))
					throw new ArgumentException("Illegal character found in argument.", "path");
			}
			if (!Directory.Exists(path))
			{
				var result = MessageBox.Show("The directory does not exist. Click \"OK\" to create, or \"Cancel\" to abandon the directory change.", "Create Directory", MessageBoxButtons.OKCancel);
				if (result == DialogResult.Cancel)
					return;
				try
				{
					Directory.CreateDirectory(path);
				}
				catch (Exception ex)
				{
					MessageBox.Show("An error occured while trying to create the directory. The directory path will not be changed.", "Directory Creation Error");
					return;
				}
			}
			_path = path;
		}

		/// <summary>
		/// 	Writes a line of text to the log file.
		/// </summary>
		/// <param name="format">
		/// 	Describes the format to use.
		/// </param>
		/// <param name="args">
		/// 	A variable-length parameters list containing arguments.
		/// </param>
		public void WriteLine(string format, params object[] args)
		{
			if (_disable)
				return;

			try
			{
				if (_displayTitle)
				{
					File.AppendAllText(LogFilePath(), String.Format(_title1, _newLine), _encoding);
					File.AppendAllText(LogFilePath(), String.Format(_title2, _newLine), _encoding);
					File.AppendAllText(LogFilePath(), String.Format(_title3, DateTime.Now, _newLine), _encoding);
					_displayTitle = false;
				}

				format = string.Format(format, args);
				File.AppendAllText(LogFilePath(), GetLine(format), _encoding);
			}
			catch
			{
				_disable = true;
			}
		}

		private string GetLine(string data)
		{
			return string.Format(_lineStart + data + _lineEnd + _newLine, DateTime.Now);
		}

		private string LogFilePath()
		{
			if (!_path.EndsWith("\\"))
				_path += "\\";
			return string.Format("{0}{1}", _path, _filename);
		}
	}
}