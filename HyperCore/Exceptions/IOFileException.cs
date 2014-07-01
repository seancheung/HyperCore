using System;

namespace HyperCore.Exceptions
{
	public class IOFileException : HyperException
	{
		public string Input { get; set; }
		public string Output { get; set; }
		public string Path { get; set; }

		/// <summary>
		/// Initializes a new instance of the IOException class
		/// </summary>
		public IOFileException()
		{

		}

		/// <summary>
		/// Initializes a new instance of the IOFileException class with parameters
		/// </summary>
		public IOFileException(string message, Exception inner)
			: base(message, inner)
		{
			
		}

		/// <summary>
		/// Initializes a new instance of the IOException class with parameters
		/// </summary>
		/// <param name="path"></param>
		/// <param name="message"></param>
		/// <param name="inner"></param>
		public IOFileException(string path, string message, Exception inner)
			: base("{0}\nPath: {1}", inner, message, path)
		{
			Path = path;
		}

		/// <summary>
		/// Initializes a new instance of the IOException class with parameters
		/// </summary>
		/// <param name="input"></param>
		/// <param name="output"></param>
		/// <param name="message"></param>
		/// <param name="inner"></param>
		public IOFileException(string input, string output, string message, Exception inner)
			: base("{0}\nInput: {1}\nOutput: {2}", inner, message, input, output)
		{
			Input = input;
			Output = output;
		}
	}
}
