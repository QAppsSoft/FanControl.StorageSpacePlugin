using System.Collections.ObjectModel;
using System.Management.Automation;

namespace FanControl.StorageSpacePlugin.Infrastructure
{
    internal interface IPowerShellRunner
    {
        /// <summary>
        /// Auxiliary Property that helps to check if there was an error and 
        /// resets the error state
        /// </summary>
        bool HasError { get; set; }

        /// <summary>
        /// A helper Property to help you get the error code
        /// </summary>
        int ErrorCode { get; }

        /// <summary>
        /// Basic method of calling PowerShell a script where all commands 
        /// and their data must be presented as one line of text
        /// </summary>
        /// <param name="ps">PowerShell environment</param>
        /// <param name="psCommand">A single line of text containing commands 
        /// and their parameters (in text format)</param>
        /// <param name="outs">A collection of objects that contains the feedback</param>
        /// <returns>The method returns true when executed correctly 
        /// and false when some errors have occurred</returns>
        bool RunPS(PowerShell ps, string psCommand, out Collection<PSObject> outs);

        /// <summary>
        /// Method 2 cmdlet call where we can only give one command 
        /// at a time and its parameters are passed as Name/Value pairs,
        /// where values can be of any type
        /// </summary>
        /// <param name="ps">PowerShell environment</param>
        /// <param name="psCommand">Single command with no parameters</param>
        /// <param name="outs">A collection of objects that contains the feedback</param>
        /// <param name="parameters">A collection of parameter pairs 
        /// in the form Name/Value</param>
        /// <returns>The method returns true when executed correctly 
        /// and false when some errors have occurred</returns>
        bool RunPS(PowerShell ps, string psCommand, out Collection<PSObject> outs, params ParameterPair[] parameters);
    }
}